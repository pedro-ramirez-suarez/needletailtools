using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Needletail.DataAccess;
using Needletail.DataAccess.Entities;

namespace DataAccess.Repository.Hybrid
{
    /// <summary>
    /// A Hybrid Repository, this stores the newer data and the most used in a SQLCE DB, and the rest in a SQL Server DB
    /// </summary>
    /// <typeparam name="T">The class</typeparam>
    /// <typeparam name="K">Key type</typeparam>
    public class SQLCE_MSSQL<E,K>: IDataSource<E,K>  where E:class
    {

        #region ctor

        public SQLCE_MSSQL(string sqlCE_ConnectionString, string mssql_ConnecitonString,string table)
        {
            _SqlCE_ConnectionString = sqlCE_ConnectionString;
            _MSSQL_ConnectionString = mssql_ConnecitonString;
            _Table = table;
            _SqlCEContext = new DBTableDataSourceBase<E, K>(_SqlCE_ConnectionString, table);
            _MSSQLContext = new DBTableDataSourceBase<E, K>(_MSSQL_ConnectionString, table);
        }

        #endregion

        #region local fields and properties
        string _SqlCE_ConnectionString;
        string _MSSQL_ConnectionString;
        string _Table;

        DBTableDataSourceBase<E, K> _SqlCEContext;

        DBTableDataSourceBase<E, K> _MSSQLContext;

        #endregion



        public bool Delete(object where)
        {
            return Delete(where, Needletail.DataAccess.Engines.FilterType.AND);
        }

        public bool Delete(object where, Needletail.DataAccess.Engines.FilterType filterType )
        {
            //delete in both context
            var result1 = _MSSQLContext.Delete(where, filterType);
            var result2 = _SqlCEContext.Delete(where, filterType);
            return result1 || result2;
        }

        public bool DeleteEntity(E item)
        {
            return Delete(item, Needletail.DataAccess.Engines.FilterType.AND);
        }


        public IEnumerable<E> GetAll()
        {
            return GetAll(false);
        }

        public IEnumerable<E> GetAll(bool onlySelCE)
        {
            var result2 = _SqlCEContext.GetAll();
            if (!onlySelCE)
            {
                var result1 = _MSSQLContext.GetAll();
                result2 = result2.Union(result1);
            }
            return result2;
        }

        public IEnumerable<E> GetAll(object orderBy)
        {
            return GetAll(orderBy, false);
        }
        public IEnumerable<E> GetAll(object orderBy, bool onlySelCE)
        {
            var result2 = _SqlCEContext.GetAll(orderBy);
            if (!onlySelCE)
            {
                var result1 = _MSSQLContext.GetAll(orderBy);
                result2= result2.Union(result1);
            }
            
            return result2;
        }

        public IEnumerable<E> GetMany(string select, string where, string orderBy)
        {
            var result1 = _MSSQLContext.GetMany(select,where,orderBy);
            var result2 = _SqlCEContext.GetMany(select, where, orderBy);
            return result2.Union(result1);
        }

        public IEnumerable<E> GetMany(object where)
        {
            var result1 = _MSSQLContext.GetMany(where);
            var result2 = _SqlCEContext.GetMany(where);
            return result2.Union(result1);
        }

        public IEnumerable<E> GetMany(object where, object orderBy)
        {
            var result1 = _MSSQLContext.GetMany(where,orderBy);
            var result2 = _SqlCEContext.GetMany(where,orderBy);
            return result2.Union(result1);
        }

        public IEnumerable<E> GetMany(object where, Needletail.DataAccess.Engines.FilterType filterType, object orderBy, int? topN)
        {
            var result1 = _MSSQLContext.GetMany(where, filterType, orderBy, topN);
            var result2 = _SqlCEContext.GetMany(where, filterType, orderBy, topN);
            return result2.Union(result1);
        }

        public IEnumerable<E> GetMany(object where, object orderBy, Needletail.DataAccess.Engines.FilterType filterType, int page, int pageSize)
        {
            var result1 = _MSSQLContext.GetMany(where, orderBy, filterType, page,pageSize);
            var result2 = _SqlCEContext.GetMany(where, orderBy, filterType, page, pageSize);
            return result2.Union(result1);
        }

        public IEnumerable<E> GetMany(string where, string orderBy, Dictionary<string, object> args, int page, int pageSize)
        {
            var result1 = _MSSQLContext.GetMany(where, orderBy, args, page, pageSize);
            var result2 = _SqlCEContext.GetMany(where, orderBy, args, page, pageSize);
            return result2.Union(result1);
        }

        public IEnumerable<E> GetMany(string where, string orderBy, Dictionary<string, object> args, int? topN)
        {
            var result1 = _MSSQLContext.GetMany(where, orderBy, args, topN);
            var result2 = _SqlCEContext.GetMany(where, orderBy, args, topN);
            return result2.Union(result1);
        }

        public IEnumerable<DynamicEntity> Join(string selectColumns, string joinQuery, string whereQuery, string orderBy, Dictionary<string, object> args)
        {
            var result1 = _MSSQLContext.Join(selectColumns, joinQuery, whereQuery, orderBy, args);
            var result2 = _SqlCEContext.Join(selectColumns, joinQuery, whereQuery, orderBy, args);
            return result2.Union(result1);
        }

        public IEnumerable<T> JoinGetTyped<T>(string selectColumns, string joinQuery, string whereQuery, string orderBy, Dictionary<string, object> args)
        {
            var result1 = _MSSQLContext.JoinGetTyped<T>(selectColumns, joinQuery, whereQuery, orderBy, args);
            var result2 = _SqlCEContext.JoinGetTyped<T>(selectColumns, joinQuery, whereQuery, orderBy, args);
            return result2.Union(result1);
        }

        public E GetSingle(object where)
        {
            return GetSingle(where: where, filterType: Needletail.DataAccess.Engines.FilterType.AND);
        }

        public E GetSingle(object where, Needletail.DataAccess.Engines.FilterType filterType = Needletail.DataAccess.Engines.FilterType.AND)
        {
            var result1 = _SqlCEContext.GetSingle(where, filterType);
            if (result1 == null)
            { 
                //search in the MSSQL
                result1 = _MSSQLContext.GetSingle(where, filterType);
                //add it to the sqlCE
                if(result1 !=null)                
                    _SqlCEContext.Insert(result1);
            }
            return result1;
        }

        public E GetSingle(string where, Dictionary<string, object> args)
        {
            var result1 = _SqlCEContext.GetSingle(where,args);
            if (result1 == null)
            {
                //search in the MSSQL
                result1 = _MSSQLContext.GetSingle(where,args);
                //add it to the sqlCE
                if (result1 != null)
                {
                    _SqlCEContext.Insert(result1);
                    //delete from MSSQL
                    _MSSQLContext.Delete(result1);
                }
            }
            return result1;
        }

        public K Insert(E newItem)
        {
            //add just to the SQLCE
            return _SqlCEContext.Insert(newItem);
        }

        public bool Update(object item)
        {
            var result1 = _MSSQLContext.Update(item);
            var result2 = _SqlCEContext.Update(item);
            return result1 || result2;
        }

        public bool UpdateWithWhere(object values, object where, Needletail.DataAccess.Engines.FilterType filterType)
        {
            var result1 = _MSSQLContext.UpdateWithWhere(values, where, filterType);
            var result2 = _SqlCEContext.UpdateWithWhere(values, where, filterType);
            return result1 || result2;
        }

        public bool UpdateWithWhere(object values, object where)
        {
            return UpdateWithWhere(values, where, Needletail.DataAccess.Engines.FilterType.AND);
        }


        public void ExecuteNonQuery(string query, Dictionary<string, object> args)
        {
            _MSSQLContext.ExecuteNonQuery(query, args);
            _SqlCEContext.ExecuteNonQuery(query, args);
        }

        public T ExecuteScalar<T>(string query, Dictionary<string, object> args)
        {
            var t = _MSSQLContext.ExecuteScalar<T>(query, args);
            var t1 = _MSSQLContext.ExecuteScalar<T>(query, args);
            //for nonsense purposes return the first result
            return t;
        }

        #region hybrid methods
        
        
        public bool MoveDataFromSQLCEToMSSQL(object where, Needletail.DataAccess.Engines.FilterType filterType)
        {
            return MoveDataBetweenServers(_SqlCEContext, _MSSQLContext, where, filterType);
        }

        public bool MoveDataFromMSSQLToSQLCE(object where, Needletail.DataAccess.Engines.FilterType filterType)
        {
            return MoveDataBetweenServers(_MSSQLContext, _SqlCEContext, where, filterType);
        }


        private bool MoveDataBetweenServers(DBTableDataSourceBase<E, K> source, DBTableDataSourceBase<E, K> target, object where, Needletail.DataAccess.Engines.FilterType filterType)
        {
            //get the data to move
            var toMove = source.GetMany(where, filterType, null, null);
            foreach (var e in toMove)
            {
                //insert the data
                target.Insert(e);
            }
            //delete the data
            return source.Delete(where, filterType);
        }


        #endregion

        
    }
}
