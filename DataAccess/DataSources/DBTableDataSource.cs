using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Configuration;
using Needletail.DataAccess.Attributes;
using System.Dynamic;
using System.Reflection;
using Needletail.DataAccess.Engines;
using System.ComponentModel;
using System.Data;
using Needletail.DataAccess.Entities;
using System.IO;
using System.Threading.Tasks;
using Needletail.DataAccess.Helper;

namespace Needletail.DataAccess {
    public class DBTableDataSourceBase<E, K> : IDisposable, IDataSourceAsync<E, K>, IDataSource<E, K> 
        where E : class
    {


        #region Events

        protected delegate void BeforeRunCommandDelegate(DbCommand cmd);
        protected event BeforeRunCommandDelegate BeforeRunCommand;

        #endregion


        //used to syncronize async requests
        static Dictionary<string, DbConnection> Locks = new Dictionary<string, DbConnection>();

        private DbProviderFactory factory;
        private DbConnection connection;

        private string ConnectionString { get; set; }
        private string TableName { get; set; }
        private string ConnectionKey { get { return string.Format("{0}:{1}", ConnectionString, TableName); } }
        private string Key { get; set; }
        private bool InsertKey { get; set; }
        private PropertyInfo[] EProperties {get;set;}
        private IDBMSEngine DBMSEngineHelper { get; set; }
        TypeConverter converter;

        /// <summary>
        /// Default ctor
        /// </summary>
        public DBTableDataSourceBase()
        {
            DBTableDataSourceInitializer("Default", typeof(E).Name);
        }


        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tableName"></param>
        public DBTableDataSourceBase(string connectionString, string tableName) 
        {
            DBTableDataSourceInitializer(connectionString, tableName);
        }
        

        private void DBTableDataSourceInitializer(string connectionString, string tableName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("tableName");
            }

            ConnectionString = connectionString;
            TableName = tableName;

            var cn = ConfigurationManager.ConnectionStrings[connectionString];
            if (cn == null)
            {
                throw new Exception("connection string does not exists");
            }


            //prepare the factory
            factory = DbProviderFactories.GetFactory(cn.ProviderName);
            if (factory == null)
            {
                throw new Exception("Cannot get provider");
            }

            //get the provider
            var parts = cn.ProviderName.Split(new char[] { '.' });
            string provider = string.Empty;
            for (int x = parts.Length - 1; x >= 0; x--)
            {
                //add the part until the part is not a number
                int tmp;
                if (!int.TryParse(parts[x], out tmp))
                {
                    provider = parts[x];
                    break;
                }

            }
            string engineName = string.Format("Needletail.DataAccess.Engines.{0}Engine", provider);
            Type engine = Type.GetType(engineName, false, true);
            //if the engine was not found, search for all needletail assemblies
            if (engine == null)
            {
                var di = new DirectoryInfo(".");
                var files = di.GetFiles();
                foreach (var f in files)
                {
                    if (!f.Name.ToLower().Contains("needletail") || f.Extension != ".dll")
                        continue;
                    var ass = Assembly.LoadFrom(f.FullName);
                    //class, assembly
                    var engineObj = ass.GetTypes().FirstOrDefault(t => t.FullName == engineName);
                    if (engineObj != null)
                    {
                        engine = Type.GetType(engineObj.AssemblyQualifiedName, false, true);
                        break;
                    }
                }
            }
            if (engine == null)
                throw new Exception("Cannot find the engine type");

            DBMSEngineHelper = Activator.CreateInstance(engine) as IDBMSEngine;
            if (DBMSEngineHelper == null)
            {
                throw new Exception("Database engine cannot be infered, be sure that you have a specific engine for this provider");
            }





            //check if the lock exists, if not, create id
            if (DBMSEngineHelper.NeedLockOnConnection)
            {
                if (!Locks.ContainsKey(ConnectionKey))
                {
                    connection = factory.CreateConnection();
                    connection.ConnectionString = cn.ConnectionString;
                    Locks.Add(ConnectionKey, connection);
                }
                else
                {
                    connection = Locks[ConnectionKey];
                }
            }
            else
            {
                connection = factory.CreateConnection();
                connection.ConnectionString = cn.ConnectionString;
            }

            //find the key
            var props = typeof(E).GetProperties();
            foreach (var p in props)
            {
                var attrs = p.GetCustomAttributes(typeof(TableKeyAttribute), true);
                if (attrs.Length > 0)
                {
                    Key = p.Name;
                    if ((attrs[0] as TableKeyAttribute).CanInsertKey)
                    {
                        this.InsertKey = true;
                    }
                    break;
                }
            }
            this.EProperties = props;



            lock (connection)
            {
                DBMSEngineHelper.PrepareEngine(cn.ConnectionString, connection);
            }
        }

        public async Task<K> InsertAsync(E newItem)
        {
            var cmd = factory.CreateCommand();
            cmd.Connection = connection;
            bool insertKey = false;
            object keyValue = null;
            //Build the query
            StringBuilder mainQuery = new StringBuilder();
            StringBuilder valsQuery = new StringBuilder();
            mainQuery.AppendFormat("INSERT INTO [{0}] (", TableName);
            valsQuery.Append("VALUES (");
            for (int x = 0; x < this.EProperties.Length; x++)
            {
                var p = this.EProperties[x];
                //do not include the ID
                if (p.Name != this.Key || InsertKey)
                {
                    //set both
                    mainQuery.Append(p.Name);
                    valsQuery.AppendFormat("@{0}", p.Name);
                    //add the parameter
                    AddParameter(p.Name, p.GetValue(newItem, null), cmd);

                    if (x <= this.EProperties.Length - 2)
                    {
                        mainQuery.Append(",");
                        valsQuery.Append(",");
                    }

                    if (p.Name == this.Key)
                        keyValue = p.GetValue(newItem, null);
                }
            }
            mainQuery.Append(")");//Close the values
            valsQuery.Append(")");//Close the values
            mainQuery.AppendFormat(" {0}", valsQuery.ToString()); // if needed

            //execute it
            cmd.CommandText = mainQuery.ToString();
            if (connection.State != ConnectionState.Closed) connection.Close();
            connection.Open();
            if (BeforeRunCommand != null) BeforeRunCommand(cmd);
            cmd.Prepare();
            var newId = await cmd.ExecuteScalarAsync();
            
            if (newId == null)
            {
                cmd.CommandText = " SELECT @@IDENTITY From [" + TableName + "]"; //To select the indentity
                if (BeforeRunCommand != null) BeforeRunCommand(cmd);
                cmd.Prepare();
                newId =  await cmd.ExecuteScalarAsync(); //fix this
                    
                if (newId.ToString() == string.Empty && InsertKey)
                {
                    newId = keyValue;
                }
            }
            connection.Close();

            if (newId == DBNull.Value)
                return default(K);
            return (K)Convert.ChangeType(newId, typeof(K));

        }
       
        public K Insert(E newItem) {
            return AsyncHelpers.RunSync<K>(() => InsertAsync(newItem)); ;
        }

        public async Task<IEnumerable<E>> GetAllAsync()
        {
            return await GetAllAsync(string.Empty);
        }

        public IEnumerable<E> GetAll()
        {
            return AsyncHelpers.RunSync<IEnumerable<E>>(() => GetAllAsync());;
        }

        private async Task<IEnumerable<E>> GetAllAsync(string orderBy, DbCommand cmd = null)
        {

            IList<E> list = new List<E>();
            if (cmd == null)
            {
                cmd = factory.CreateCommand();
            }
            
            cmd.Connection = connection;
            //Set the command
            orderBy = !string.IsNullOrWhiteSpace(orderBy) ? string.Format(" ORDER BY {0}", orderBy) : string.Empty;
            cmd.CommandText = string.Format("SELECT * FROM [{0}] {1}", TableName, orderBy);
            return await CreateListFromCommandAsync(cmd);
        }

        public async Task<IEnumerable<E>> GetAllAsync(object orderBy)
        {
            var cmd = factory.CreateCommand();
            StringBuilder oq;
            if (orderBy.GetType() == typeof(string))
                oq = this.OrderByBuilder(orderBy, cmd);
            else
                oq = new StringBuilder();
            
            return await GetAllAsync(oq.ToString());
        }
        
        public IEnumerable<E> GetAll(object orderBy)
        {
            var cmd = factory.CreateCommand();
            StringBuilder oq = this.OrderByBuilder(orderBy, cmd);
            return AsyncHelpers.RunSync<IEnumerable<E>>(() => GetAllAsync(oq.ToString()));
        }

        public async Task<bool> UpdateAsync(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item cannot be null");
            }
            var cmd = factory.CreateCommand();
            cmd.Connection = connection;
            bool keyFound = false;
            var uq = GetUpdateString(item, cmd, ref keyFound);
            //Add the where
            if (!keyFound)
            {
                throw new Exception("Cannot determine the value for the primary Key");
            }

            uq.AppendFormat(" WHERE [{0}] = @{0}", this.Key);
            //execute it
            cmd.CommandText = uq.ToString();
            if (BeforeRunCommand != null) BeforeRunCommand(cmd);
            if (connection.State != ConnectionState.Closed) connection.Close();
            connection.Open();
            cmd.Prepare();
            var result = (int)await cmd.ExecuteNonQueryAsync();
            connection.Close();
            return result > 0;
        }


        /// <summary>
        /// This updates the record and uses the key as the only filter
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Update(object item) {
            return AsyncHelpers.RunSync<bool>(() => UpdateAsync(item));
        }

        public async Task<bool> UpdateWithWhereAsync(object values, object where)
        {
            return await UpdateWithWhereAsync(values, where, FilterType.AND);
        }
        public bool UpdateWithWhere(object values, object where)
        {
            return UpdateWithWhere(values,where, FilterType.AND);
        }

        public async Task<bool> UpdateWithWhereAsync(object values, object where, FilterType filterType) 
        {
            if (values == null)
            {
                throw new ArgumentNullException("values cannot be null");
            }
            var cmd = factory.CreateCommand();
            cmd.Connection = connection;
            bool keyFound = false;
            var uq = GetUpdateString(values, cmd, ref keyFound);
            //create the where
            var wb = WhereBuilder(where, cmd, filterType.ToString());
            //add it to the rest of the query
            uq.AppendFormat(" {0} {1} ", string.IsNullOrWhiteSpace(wb.ToString()) ? "" : "WHERE", wb);

            //execute it
            cmd.CommandText = uq.ToString();
            if (BeforeRunCommand != null) BeforeRunCommand(cmd);
            if (connection.State != ConnectionState.Closed) connection.Close();
            connection.Open();
            cmd.Prepare();
            var result = (int)await cmd.ExecuteNonQueryAsync();
            connection.Close();
            return result > 0;
        }

        public bool UpdateWithWhere(object values, object where,FilterType filterType ) {
            return AsyncHelpers.RunSync<bool>(() => UpdateWithWhereAsync(values, where, filterType));
        }

        public async Task<bool> DeleteEntityAsync(E item)
        {
            return await DeleteAsync(item, FilterType.AND);
        }
        public bool DeleteEntity(E item)
        {
            return Delete(item, FilterType.AND);
        }

        public async Task<bool> DeleteAsync(object where)
        {
            return await DeleteAsync(where, FilterType.AND);  
        }

        public bool Delete(object where)
        {
            return Delete(where, FilterType.AND);
        }

        public async Task<bool> DeleteAsync(object where, FilterType filterType)
        {
            var cmd = factory.CreateCommand();
            cmd.Connection = connection;
            string dq = string.Format("DELETE FROM [{0}] ", this.TableName);

            var wq = WhereBuilder(where, cmd, filterType.ToString());
            if (string.IsNullOrWhiteSpace(wq.ToString()))
                wq.Insert(0, dq);
            else
                wq.Insert(0, string.Format("{0} WHERE ", dq));

            //execute it
            cmd.CommandText = wq.ToString();
            if (BeforeRunCommand != null) BeforeRunCommand(cmd);
            if (connection.State != ConnectionState.Closed) connection.Close();
            connection.Open();
            cmd.Prepare();

            var result = (int) await cmd.ExecuteNonQueryAsync();
            connection.Close();
            return result > 0;
        }
        
        public bool Delete(object where, FilterType filterType) {
            return AsyncHelpers.RunSync<bool>(() => DeleteAsync(where, filterType));
        }

        public async Task<IEnumerable<E>> GetManyAsync(string select, string where, string orderBy)
        {
            if (string.IsNullOrWhiteSpace(select))
                throw new ArgumentNullException("Select");
            var cmd = factory.CreateCommand();
            cmd.Connection = connection;
            //Set the command
            cmd.CommandText = string.Format("{0} FROM [{1}] {2} {3}", select, this.TableName, string.IsNullOrWhiteSpace(where) ? "" : string.Format(" WHERE {0} ", where), string.IsNullOrWhiteSpace(orderBy) ? "" : string.Format(" Order By {0} ", orderBy));

            return await CreateListFromCommandAsync(cmd);
        }

        public IEnumerable<E> GetMany(string select, string where, string orderBy)
        {
            return AsyncHelpers.RunSync<IEnumerable<E>>(() => GetManyAsync(select, where, orderBy));
        }


        public async Task<IEnumerable<E>> GetManyAsync(string where, string orderBy, Dictionary<string, object> args, int? topN)
        {
            if (where == null)
            {
                throw new ArgumentNullException("where");
            }
            var cmd = factory.CreateCommand();
            cmd.Connection = connection;
            //Add the parameters
            AddParameters(cmd, args);
            //Set the command
            cmd.CommandText = this.DBMSEngineHelper.GetQueryTemplateForTop("*", TableName, where, orderBy, topN);

            return await CreateListFromCommandAsync(cmd);
        }

        /// <summary>
        /// get a list of elements
        /// </summary>
        /// <param name="where">The where string</param>
        /// <param name="args">parameters names and values being used in the query</param>
        /// <param name="topN">if this null, it will get all the elements that match the query</param>
        public IEnumerable<E> GetMany(string where,string orderBy, Dictionary<string,object> args,int? topN) 
        {
            return AsyncHelpers.RunSync<IEnumerable<E>>(() => GetManyAsync(where, orderBy, args, topN));
        }


        public virtual async Task<IEnumerable<E>> GetManyAsync(string where, string orderBy, Dictionary<string, object> args, int page, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(where))
            {
                throw new ArgumentNullException("where");
            }
            var cmd = factory.CreateCommand();
            //lock (connection)
            //{
            cmd.Connection = connection;
            //Add the parameters
            AddParameters(cmd, args);

            //Set the command
            cmd.CommandText = this.DBMSEngineHelper.GetQueryForPagination("*", TableName, where, orderBy, pageSize, page, this.Key);

            return await CreateListFromCommandAsync(cmd);
            //}
        }

        public virtual IEnumerable<E> GetMany(string where,string orderBy, Dictionary<string, object> args, int page, int pageSize)
        {
            return AsyncHelpers.RunSync<IEnumerable<E>>(() => GetManyAsync(where, orderBy, args, page, pageSize));
        }


        public async Task<IEnumerable<E>> GetManyAsync(object where, FilterType filterType, object orderBy, int? topN)
        {

            if (where == null)
            {
                throw new ArgumentNullException("filter");
            }
            IList<E> list = new List<E>();
            var cmd = factory.CreateCommand();
            cmd.Connection = connection;
            StringBuilder qb = WhereBuilder(where, cmd, filterType.ToString(), false);
            StringBuilder oq = this.OrderByBuilder(orderBy, cmd);

            cmd.CommandText = this.DBMSEngineHelper.GetQueryTemplateForTop("*", TableName, qb.ToString(), oq.ToString(), topN);

            return await CreateListFromCommandAsync(cmd);
        }

        /// <summary>
        /// Get a list of elements that match the query
        /// </summary>
        /// <param name="filter">The filter to be used, use like this: new { Name = "Me", Age = 34 }</param>
        /// <param name="filterType">chain the filter with AND/OR</param>
        /// <param name="topN">if this null, it will get all the elements that match the query</param>
        public IEnumerable<E> GetMany(object where,FilterType filterType,object orderBy,int? topN) 
        {
            return AsyncHelpers.RunSync<IEnumerable<E>>(() => GetManyAsync(where, filterType, orderBy, topN));
        }

        public virtual async Task<IEnumerable<E>> GetManyAsync(object where, object orderBy, FilterType filterType, int page, int pageSize)
        {
            if (where == null)
            {
                throw new ArgumentNullException("filter");
            }
            IList<E> list = new List<E>();
            var cmd = factory.CreateCommand();
            cmd.Connection = connection;
            StringBuilder qb = WhereBuilder(where, cmd, filterType.ToString(), false);
            StringBuilder oq = this.OrderByBuilder(orderBy, cmd);
            cmd.CommandText = this.DBMSEngineHelper.GetQueryForPagination("*", TableName, qb.ToString(), oq.ToString(), pageSize, page, this.Key);
            return await CreateListFromCommandAsync(cmd);
        }

        public virtual IEnumerable<E> GetMany(object where, object orderBy,FilterType filterType, int page, int pageSize)
        {
            return AsyncHelpers.RunSync<IEnumerable<E>>(()=>GetManyAsync(where, orderBy, filterType, page, pageSize));
        }

        public async Task<IEnumerable<E>> GetManyAsync(object where)
        {
            return await GetManyAsync(where, FilterType.AND, null, null);
        }

        public IEnumerable<E> GetMany(object where) 
        {
            return GetMany(where, FilterType.AND, null, null);
        }

        public async Task<IEnumerable<E>> GetManyAsync(object where, object orderBy)
        {
            return await GetManyAsync(where, FilterType.AND, orderBy, null);
        }
        public IEnumerable<E> GetMany(object where,object orderBy)
        {
            return GetMany(where, FilterType.AND, orderBy, null);
        }

        public async Task<E> GetSingleAsync(object where)
        {
            return await GetSingleAsync(where: where, filterType: FilterType.AND);
        }

        public E GetSingle(object where)
        {
            return GetSingle(where: where, filterType: FilterType.AND);
        }

        public async Task<E> GetSingleAsync(string where, Dictionary<string, object> args)
        {
            var singleE = await GetManyAsync(where, string.Empty, args, 1);
            return singleE.FirstOrDefault();
        }

        public E GetSingle(string where, Dictionary<string,object> args) {
            var singleE = GetMany(where,string.Empty,args,1);
            return singleE.FirstOrDefault();
        }

        public async Task<E> GetSingleAsync(object where, FilterType filterType)
        {
            var singleE = await GetManyAsync(where, filterType, null, 1);
            return singleE.FirstOrDefault();
        }

        public E GetSingle(object where,FilterType filterType) {
            var singleE = GetMany(where,filterType,null,1);
            return singleE.FirstOrDefault();
        }


        public async Task<IEnumerable<T>> JoinGetTypedAsync<T>(string selectColumns, string joinQuery, string whereQuery, string orderBy, Dictionary<string, object> args)
        {
            if (string.IsNullOrWhiteSpace(selectColumns))
            {
                throw new ArgumentNullException("selectQuery");
            }
            if (string.IsNullOrWhiteSpace(joinQuery))
            {
                throw new ArgumentNullException("joinQuery");
            }

            //create the query
            var cmd = factory.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = string.Format("SELECT {0} FROM [{1}] {2} {3} {4}", selectColumns, this.TableName, joinQuery, string.IsNullOrWhiteSpace(whereQuery) ? "" : string.Format(" WHERE {0} ", whereQuery), orderBy);
            //add the parameters
            AddParameters(cmd, args);

            //return the data
            return await CreateUnknownItemListFromCommandTypedAsync<T>(cmd);
        }

        public IEnumerable<T> JoinGetTyped<T>(string selectColumns, string joinQuery, string whereQuery, string orderBy, Dictionary<string, object> args)
        {
            return AsyncHelpers.RunSync<IEnumerable<T>>(() => JoinGetTypedAsync<T>(selectColumns, joinQuery, whereQuery, orderBy, args));
        }

        public async Task<IEnumerable<DynamicEntity>> JoinAsync(string selectColumns, string joinQuery, string whereQuery, string orderBy, Dictionary<string, object> args)
        {
            if (string.IsNullOrWhiteSpace(selectColumns))
            {
                throw new ArgumentNullException("selectQuery");
            }
            if (string.IsNullOrWhiteSpace(joinQuery))
            {
                throw new ArgumentNullException("joinQuery");
            }


            //create the query
            var cmd = factory.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = string.Format("SELECT {0} FROM [{1}] {2} {3} {4}", selectColumns, this.TableName, joinQuery, string.IsNullOrWhiteSpace(whereQuery) ? "" : string.Format(" WHERE {0} ", whereQuery), orderBy);
            //add the parameters
            AddParameters(cmd, args);

            //return the data
            return await CreateUnknownItemListFromCommandAsync(cmd);
        }

        public IEnumerable<DynamicEntity> Join(string selectColumns, string joinQuery, string whereQuery, string orderBy, Dictionary<string, object> args)
        {
            return AsyncHelpers.RunSync<IEnumerable<DynamicEntity>>(() => JoinAsync(selectColumns, joinQuery, whereQuery, orderBy, args));
        }


        #region local helpers


        private StringBuilder GetUpdateString(object item, DbCommand cmd, ref bool keyFound) {
            var props = item.GetType().GetProperties();
            StringBuilder uq = new StringBuilder(string.Format("UPDATE [{0}] SET ", this.TableName));
            for(int x= 0; x< props.Length; x++) {
                var p = props[x];
                //do not update the key
                if (p.Name != this.Key)
                {
                    uq.AppendFormat(" {0} = @{0} ", p.Name);
                    //add the parameter
                    AddParameter(p.Name, p.GetValue(item, null), cmd);

                    // we have the key
                    if (p.Name == this.Key)
                    {
                        keyFound = true;
                    }
                    if (x < props.Length - 1)
                    {
                        uq.Append(",");
                    }
                }
                else
                {
                    //add the key as a parameter
                    keyFound = true;
                    AddParameter(p.Name, p.GetValue(item, null), cmd);
                }
            }
            return uq;
        }


        private StringBuilder WhereBuilder(object where, DbCommand cmd,string separator,bool validateNullWhere = true) {
            if (where == null && validateNullWhere) {
                throw new ArgumentNullException("Where");
            }
            if (cmd == null) {
                throw new ArgumentNullException("cmd");
            }
            if (string.IsNullOrWhiteSpace(separator)) {
                throw new ArgumentNullException("separator");
            }

            StringBuilder w = new StringBuilder();
            if (!validateNullWhere && where == null) {
                return w;
            }
            //create the where
            var props = where.GetType().GetProperties();
            for (int x = 0; x < props.Length; x++) {
                var p = props[x];
                var name = p.Name;
                string parameterName;
                string query = SQLTokens.BuildCompare(ref name, out parameterName, ref separator);

                if (x > 0)
                {
                    w.AppendFormat(" {0} {1}", separator,query);
                }
                else
                {
                    w.AppendFormat(query);
                }

                //add the parameter
                var newParam = AddParameter(parameterName, p.GetValue(where, null), cmd);
                if (newParam != null)
                    w.Replace(parameterName, newParam);
                
            }
            return w;
        }


        private StringBuilder OrderByBuilder(object orderBy, DbCommand cmd)
        {
            
            if (cmd == null)
            {
                throw new ArgumentNullException("cmd");
            }

            StringBuilder w = new StringBuilder();
            if (orderBy == null) {
                return w;  
            }
            //create the where
            var props = orderBy.GetType().GetProperties();
            for (int x = 0; x < props.Length; x++)
            {
                var p = props[x];
                var direction = p.GetValue(orderBy).ToString().ToUpper();
                if (direction == "ASC" || direction == "DESC")
                {
                    w.AppendFormat(" {0} {1} ", p.Name, direction);
                    if (x < props.Length - 1)
                    {
                        w.Append(",");
                    }
                }
            }
            return w;
        }


        private async Task<IEnumerable<E>> CreateListFromCommandAsync(DbCommand cmd) {
            IList<E> list = new List<E>();
            if (connection.State != ConnectionState.Closed) connection.Close();
            connection.Open();
            if (BeforeRunCommand != null) BeforeRunCommand(cmd);
               cmd.Prepare();
            //fill the collection
            using (var reader = await cmd.ExecuteReaderAsync()) {
                var cols = new List<string>();
                if(reader.Read()){
                    for(int x=0; x<reader.FieldCount; x++){
                       cols.Add(reader.GetName(x)); 
                    }
                }
                if (cols.Count > 0)
                {
                    do
                    {
                        var item = Activator.CreateInstance<E>();
                        foreach (var p in this.EProperties)
                        {
                            if (p.CanWrite && cols.IndexOf(p.Name) > -1 && reader[p.Name] != DBNull.Value)
                            {
                                //check if the property exists in the DB
                                p.SetValue(item, reader[p.Name], null);
                            }

                        }
                        list.Add(item);
                    } while (reader.Read());
                }
            }
            connection.Close();

            return list;
        }

        private async Task<IEnumerable<DynamicEntity>> CreateUnknownItemListFromCommandAsync(DbCommand cmd)
        {
            List<DynamicEntity> list = new List<DynamicEntity>();
            if (connection.State != ConnectionState.Closed) connection.Close();
            connection.Open();
            if (BeforeRunCommand != null) BeforeRunCommand(cmd);
            cmd.Prepare();
            
            //fill the collection
            using (var reader = await cmd.ExecuteReaderAsync()) {
                var cols = new List<string>();
                for (int x = 0; x < reader.FieldCount; x++) {
                    cols.Add(reader.GetName(x));
                }
                while (reader.Read()) {

                    DynamicEntity item = new DynamicEntity(cols);
                    reader.GetValues((item as DynamicEntity).Values);
                    //reader.GetValues(new object[] {} );
                    list.Add(item);
                }
            }
            connection.Close();

            return list;
        }


        private async Task<IEnumerable<T>> CreateUnknownItemListFromCommandTypedAsync<T>(DbCommand cmd)
        {
            List<T> list = new List<T>();
            if (connection.State != ConnectionState.Closed) connection.Close();
            connection.Open();
            if (BeforeRunCommand != null) BeforeRunCommand(cmd);
            cmd.Prepare();
            
            //fill the collection
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                var cols = new List<string>();

                for (int x = 0; x < reader.FieldCount; x++)
                {
                    cols.Add(reader.GetName(x));
                }
                var tType  = typeof(T);

                while (reader.Read())
                {
                    //get the property
                    var item = Activator.CreateInstance<T>();
                    for(var x=0 ;x<cols.Count;x++)
                    {
                        var p = tType.GetProperty(cols[x]);
                        if (p != null && reader.GetValue(x) != DBNull.Value)
                        {
                            p.SetValue(item,reader.GetValue(x), null);
                        }

                    }
                    list.Add(item);
                    
                }
            }
            connection.Close();

            return list;
        }



        /// <summary>
        /// add a single parameter to the cmd
        /// </summary>
        private string AddParameter(string parameterName, object value, DbCommand cmd) {
                
            var param = factory.CreateParameter();
            param.ParameterName = !parameterName.StartsWith("@") ? string.Format("@{0}", parameterName) : parameterName;
            if (value!= null && value.GetType().IsArray)
            {
                //create the list
                Array values = value as Array;
                var newValue = new StringBuilder();
                var parameters = new Dictionary<string,object>();
                string newParamName;
                for (int x = 0; x < values.Length; x++)
                {
                    var val = values.GetValue(x);
                    newParamName = string.Format("{0}{1}", parameterName, x);
                    newValue.Append(newParamName);
                    if (x < (values.Length - 1))
                        newValue.Append(",");
                    parameters.Add(newParamName, val);
                }
                AddParameters(cmd, parameters);
                return newValue.ToString();
            }
            else
            { 
                param.Value = value != null ? value : DBNull.Value;
                //The dbType and the rest of the info
                this.DBMSEngineHelper.ConfigureParameterForValue(param, value);

                param.Direction = System.Data.ParameterDirection.Input;
                param.Size = param.Value != null ? param.Value.ToString().Length + 1 : 1;
                cmd.Parameters.Add(param);
            }

            return null;
        }


        /// <summary>
        /// Add the parameters
        /// </summary>
        private void AddParameters(DbCommand cmd,Dictionary<string,object> args)
        {
            //add the parameters
            if (args != null) {
                foreach (var k in args.Keys) {
                    AddParameter(k,args[k],cmd);                    
                }
            }
        }

        #endregion


        public async Task ExecuteNonQueryAsync(string query, Dictionary<string, object> args)
        {
            var cmd = factory.CreateCommand();
                cmd.Connection = connection;
                cmd.CommandText = query;
                //add the parameters
                AddParameters(cmd, args);
                if (connection.State != ConnectionState.Closed) connection.Close();
                //execute the query
                connection.Open();
                if (BeforeRunCommand != null) BeforeRunCommand(cmd);
                cmd.Prepare();                
                await cmd.ExecuteNonQueryAsync();
                connection.Close();
        }

        public void ExecuteNonQuery(string query, Dictionary<string, object> args)
        {
            AsyncHelpers.RunSync(() => ExecuteNonQueryAsync(query, args));
        }

        public async Task<T> ExecuteScalarAsync<T>(string query, Dictionary<string, object> args)
        {
            var cmd = factory.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = query;
            //add the parameters
            AddParameters(cmd, args);
            if (connection.State != ConnectionState.Closed) connection.Close();
            connection.Open();
            if (BeforeRunCommand != null) BeforeRunCommand(cmd);
            //execute the query
            cmd.Prepare();

            var t = await cmd.ExecuteScalarAsync();
            connection.Close();
            if (t == DBNull.Value || t == null)
                return default(T);
            return (T)t;
        }

        public T ExecuteScalar<T>(string query, Dictionary<string, object> args)
        {
            return AsyncHelpers.RunSync<T>(() => ExecuteScalarAsync<T>(query, args));
        }


        /// <summary>
        /// Release internal objects and dispose the connection
        /// </summary>
        public void Dispose()
        {
            this.factory = null;
            if (connection.State != ConnectionState.Closed) connection.Close();
            this.connection.Dispose();
            this.converter = null;
            GC.Collect();
        }
    }
}
