using System;
using System.Collections.Generic;
using Needletail.DataAccess.Engines;
using Needletail.DataAccess.Entities;
using System.Threading.Tasks;
namespace Needletail.DataAccess {
    public interface IDataSource<E, K>
     where E : class
    {
        
        bool Delete(object where, FilterType filterType);
        bool Delete(object where);
        bool DeleteEntity(E item);

        System.Collections.Generic.IEnumerable<E> GetAll();
        System.Collections.Generic.IEnumerable<E> GetAll(object orderBy);
        System.Collections.Generic.IEnumerable<E> GetMany(string select, string where,string orderBy);
        System.Collections.Generic.IEnumerable<E> GetMany(object where);
        System.Collections.Generic.IEnumerable<E> GetMany(object where,object orderBy);
        System.Collections.Generic.IEnumerable<E> GetMany(object where, FilterType filterType, object orderBy, int? topN);
        System.Collections.Generic.IEnumerable<E> GetMany(object where, object orderBy, FilterType filterType, int page, int pageSize);
        System.Collections.Generic.IEnumerable<E> GetMany(string where, string orderBy, System.Collections.Generic.Dictionary<string, object> args, int page, int pageSize);
        System.Collections.Generic.IEnumerable<E> GetMany(string where, string orderBy, System.Collections.Generic.Dictionary<string, object> args, int? topN);
        System.Collections.Generic.IEnumerable<DynamicEntity> Join(string selectColumns, string joinQuery, string whereQuery, string orderBy, System.Collections.Generic.Dictionary<string, object> args);
        IEnumerable<T> JoinGetTyped<T>(string selectColumns, string joinQuery, string whereQuery, string orderBy, Dictionary<string, object> args);
        E GetSingle(object where);
        E GetSingle(object where, FilterType filterType);
        E GetSingle(string where, System.Collections.Generic.Dictionary<string, object> args);
        K Insert(E newItem);
        bool Update(object item);
        bool UpdateWithWhere(object values, object where, FilterType filterType);
        bool UpdateWithWhere(object values, object where);        
        void ExecuteNonQuery(string query, Dictionary<string, object> args);
        T ExecuteScalar<T>(string query, Dictionary<string, object> args);

        System.Collections.Generic.IEnumerable<T> ExecuteStoredProcedureReturnRows<T>(string name, object parameters);
        void ExecuteStoredProcedure(string name, object parameters);

    }
}
