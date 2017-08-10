using System;
using System.Data.Common;
namespace Needletail.DataAccess.Engines {
    public interface IDBMSEngine {

        void PrepareEngine(string connectionString, DbConnection connection);
        
        string GetQueryForPagination(string columns, string from, string where, string orderBy, int pageSize, int pageNumber, string key);

        string GetOrderByQuery(string orderBy, SQLTokens.OrderBy direction);

        string GetQueryTemplateForTop(string columns,string from, string where, string orderBy,int? topN);

        void ConfigureParameterForValue(DbParameter param, object value, byte precision = 10, byte scale = 2);

        bool NeedLockOnConnection { get; }
    }
}
