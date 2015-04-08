using System;
using System.Collections.Generic;
using Needletail.DataAccess.Engines;
using Needletail.DataAccess.Entities;
using System.Threading.Tasks;
namespace Needletail.DataAccess {
    public interface IDataSourceAsync<E, K>
     where E : class
    {
        #region Async Code

        Task<bool> DeleteAsync(object where, FilterType filterType);
        Task<bool> DeleteAsync(object where);
        Task<bool> DeleteEntityAsync(E item);

        Task<System.Collections.Generic.IEnumerable<E>> GetAllAsync();
        Task<System.Collections.Generic.IEnumerable<E>> GetAllAsync(object orderBy);
        Task<System.Collections.Generic.IEnumerable<E>> GetManyAsync(string select, string where, string orderBy);
        Task<System.Collections.Generic.IEnumerable<E>> GetManyAsync(object where);
        Task<System.Collections.Generic.IEnumerable<E>> GetManyAsync(object where, object orderBy);
        Task<System.Collections.Generic.IEnumerable<E>> GetManyAsync(object where, FilterType filterType, object orderBy, int? topN);
        Task<System.Collections.Generic.IEnumerable<E>> GetManyAsync(object where, object orderBy, FilterType filterType, int page, int pageSize);
        Task<System.Collections.Generic.IEnumerable<E>> GetManyAsync(string where, string orderBy, System.Collections.Generic.Dictionary<string, object> args, int page, int pageSize);
        Task<System.Collections.Generic.IEnumerable<E>> GetManyAsync(string where, string orderBy, System.Collections.Generic.Dictionary<string, object> args, int? topN);
        Task<System.Collections.Generic.IEnumerable<DynamicEntity>> JoinAsync(string selectColumns, string joinQuery, string whereQuery, string orderBy, System.Collections.Generic.Dictionary<string, object> args);
        Task<IEnumerable<T>> JoinGetTypedAsync<T>(string selectColumns, string joinQuery, string whereQuery, string orderBy, Dictionary<string, object> args);
        Task<E> GetSingleAsync(object where);
        Task<E> GetSingleAsync(object where, FilterType filterType);
        Task<E> GetSingleAsync(string where, System.Collections.Generic.Dictionary<string, object> args);
        Task<K> InsertAsync(E newItem);
        Task<bool> UpdateAsync(object item);
        Task<bool> UpdateWithWhereAsync(object values, object where, FilterType filterType);
        Task<bool> UpdateWithWhereAsync(object values, object where);
        Task ExecuteNonQueryAsync(string query, Dictionary<string, object> args);
        Task<T> ExecuteScalarAsync<T>(string query, Dictionary<string, object> args);

        #endregion

    }
}
