def repository_template name, keytype
    name_downcase = name.downcase
return <<template
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Needletail.SampleModel.Model.Entity;
using Needletail.DataAccess;

namespace #{@solution_name_sans_extension}.Repositories
{
    public class #{name}Repository : IDisposable, IDataSource<#{name}, #{keytype}> 
    {
        DBTableDataSourceBase<#{name}, #{keytype}> dataSource;

        string ConnectionString { get; set; }

        string tableName;
        private string TableName 
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(tableName))
                { 
                }
                return tableName;
            }
            set 
            {
                tableName = value;
            }
        }

        public #{name}Repository()
        {
            this.TableName = typeof(#{name}).Name;
            this.ConnectionString = "Default";
            InitializeDataSource();
        }

        public #{name}Repository(string connectionString)
        {
            this.TableName = typeof(#{name}).Name;
            this.ConnectionString = connectionString;
            InitializeDataSource();
        }

        public #{name}Repository(string connectionString,string tableName)
        {
            this.ConnectionString = connectionString;
            this.TableName = tableName;
            InitializeDataSource();
        }

        private void InitializeDataSource()
        {
            dataSource = new DBTableDataSourceBase<#{name}, #{keytype}>(this.ConnectionString, this.TableName);
        }

        public bool Delete(object where, Needletail.DataAccess.Engines.FilterType filterType)
        {
            return this.dataSource.Delete(where: where, filterType: filterType);
        }

        public bool Delete(object where)
        {
            return this.dataSource.Delete(where: where);
        }

        public bool DeleteEntity(#{name} item)
        {
            return this.dataSource.DeleteEntity(item: item);
        }

        public IEnumerable<#{name}> GetAll()
        {
            return this.dataSource.GetAll();
        }

        public IEnumerable<#{name}> GetAll(object orderBy)
        {
            return this.dataSource.GetAll(orderBy: orderBy);
        }

        public IEnumerable<#{name}> GetMany(string select, string where, string orderBy)
        {
            return this.dataSource.GetMany(select: select, where: where, orderBy: orderBy);
        }

        public IEnumerable<#{name}> GetMany(object where)
        {
            return this.dataSource.GetMany(where: where);
        }

        public IEnumerable<#{name}> GetMany(object where, object orderBy)
        {
            return this.dataSource.GetMany(where: where, orderBy: orderBy);
        }

        public IEnumerable<#{name}> GetMany(object where, Needletail.DataAccess.Engines.FilterType filterType, object orderBy, int? topN)
        {
            return this.dataSource.GetMany(where: where, filterType: filterType, orderBy: orderBy, topN: topN);
        }

        public IEnumerable<#{name}> GetMany(object where, object orderBy, Needletail.DataAccess.Engines.FilterType filterType, int page, int pageSize)
        {
            return this.dataSource.GetMany(where: where, orderBy: orderBy, filterType: filterType, page: page, pageSize: pageSize);
        }

        public IEnumerable<#{name}> GetMany(string where, string orderBy, Dictionary<string, object> args, int page, int pageSize)
        {
            return this.dataSource.GetMany(where: where, orderBy: orderBy, args: args, page: page, pageSize: pageSize);
        }

        public IEnumerable<#{name}> GetMany(string where, string orderBy, Dictionary<string, object> args, int? topN)
        {
            return this.dataSource.GetMany(where: where, orderBy: orderBy, args: args, topN: topN);
        }

        public IEnumerable<Needletail.DataAccess.Entities.DynamicEntity> Join(string selectColumns, string joinQuery, string whereQuery, string orderBy, Dictionary<string, object> args)
        {
            return this.dataSource.Join(selectColumns: selectColumns, joinQuery: joinQuery, whereQuery: whereQuery, orderBy: orderBy, args: args);
        }

        public IEnumerable<T> JoinGetTyped<T>(string selectColumns, string joinQuery, string whereQuery, string orderBy, Dictionary<string, object> args)
        {
            return this.dataSource.JoinGetTyped<T>(selectColumns: selectColumns, joinQuery: joinQuery, whereQuery: whereQuery, orderBy: orderBy, args: args);
        }

        public #{name} GetSingle(object where)
        {
            return this.dataSource.GetSingle(where: where);
        }

        public #{name} GetSingle(object where, Needletail.DataAccess.Engines.FilterType filterType)
        {
            return this.dataSource.GetSingle(where: where, filterType: filterType);
        }

        public #{name} GetSingle(string where, Dictionary<string, object> args)
        {
            return this.dataSource.GetSingle(where: where, args: args);
        }

        public #{keytype} Insert(#{name} newItem)
        {
            return this.dataSource.Insert(newItem: newItem);
        }

        public bool Update(object item)
        {
            return this.dataSource.Update(item: item);
        }

        public bool UpdateWithWhere(object values, object where, Needletail.DataAccess.Engines.FilterType filterType)
        {
            return this.dataSource.UpdateWithWhere(values: values, where: where, filterType: filterType);
        }

        public bool UpdateWithWhere(object values, object where)
        {
            return this.dataSource.UpdateWithWhere(values: values, where: where);
        }

        public void ExecuteNonQuery(string query, Dictionary<string, object> args)
        {
            this.dataSource.ExecuteNonQuery(query: query, args: args);
        }

        public T ExecuteScalar<T>(string query, Dictionary<string, object> args)
        {
            return this.dataSource.ExecuteScalar<T>(query: query, args: args);
        }

        public void Dispose()
        {
            this.dataSource.Dispose();
        }
    }
}
template
end