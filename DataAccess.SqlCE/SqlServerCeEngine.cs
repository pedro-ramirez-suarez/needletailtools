using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using System.Data.Common;
using System.Data;
using Microsoft.SqlServer.Types;

namespace Needletail.DataAccess.Engines {
    public class SqlServerCeEngine : DBMSEngineBase, Needletail.DataAccess.Engines.IDBMSEngine 
    {


        public override void PrepareEngine(string connectionString, DbConnection connection)
        {
            try {
                connection.Open();
            }
            catch {
                SqlCeEngine e = new SqlCeEngine(connectionString);
                //e.Upgrade();
            }
            finally
            {
                connection.Close();
            }
        }



        public override string GetQueryForPagination(string columns, string from, string where, string orderBy, int pageSize, int pageNumber, string key)
        {
            return string.Format("SELECT {0} FROM {1} {2} {3} OFFSET {4} ROWS FETCH NEXT {5} ROWS ONLY", 
                                columns,
                                from,
                                !string.IsNullOrWhiteSpace(where) ? string.Format(" WHERE {0}",where): string.Empty,
                                !string.IsNullOrWhiteSpace(orderBy) ? string.Format(" ORDER BY {0}", orderBy) : string.Empty,
                                pageNumber * pageSize,
                                pageSize);
        }


        public override string GetOrderByQuery(string orderBy, SQLTokens.OrderBy direction)
        {
            return string.Format("{0} {1}", orderBy, direction.ToString());
        }


        public override string GetQueryTemplateForTop(string columns, string from, string where, string orderBy, int? topN)
        {
            return string.Format("SELECT {0} {1} FROM {2} {3} {4}",
                                             topN.HasValue ? string.Format(" TOP {0}", topN.Value) : "",
                                             columns,
                                             from,
                                             !string.IsNullOrWhiteSpace(where) ? string.Format(" WHERE {0}", where) : string.Empty,
                                             !string.IsNullOrWhiteSpace(orderBy) ? string.Format(" ORDER BY {0}", orderBy) : string.Empty);            
        }

        public override void ConfigureParameterForValue(DbParameter param, object value, byte precision = 10, byte scale = 2)
        {
            param.DbType = Converters.GetDBTypeFor(param.Value);
            if (value == null || value == DBNull.Value)
                return;
            if (value.GetType() == typeof(string) && value.ToString().Length > 4000)
                (param as SqlCeParameter).SqlDbType = SqlDbType.NText;
            else if (value.GetType() == typeof(SqlGeography))
            {
                (param as SqlCeParameter).SqlDbType = System.Data.SqlDbType.Udt;
            }
            else if (value.GetType() == typeof(SqlGeometry))
            {
                (param as SqlCeParameter).SqlDbType = System.Data.SqlDbType.Udt;
            }
            
        }


        public override bool NeedLockOnConnection
        {
            get
            {
                return true;
            }
        }
    }
}
