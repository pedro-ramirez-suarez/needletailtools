using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.SqlServer.Types;

namespace Needletail.DataAccess.Engines {
    public class SqlClientEngine : DBMSEngineBase, Needletail.DataAccess.Engines.IDBMSEngine
    {




        public override string GetQueryForPagination(string columns, string from, string where, string orderBy, int pageSize, int pageNumber, string key)
        {
            return string.Format(
                "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY {0}) AS Row,{1} FROM {2} {3}) AS PAGED WHERE Row BETWEEN {5} AND {6} {4}",
                                key,
                                columns,
                                from,
                                !string.IsNullOrWhiteSpace(where) ? string.Format(" WHERE {0}", where) : string.Empty,
                                !string.IsNullOrWhiteSpace(orderBy) ? string.Format(" ORDER BY {0}", orderBy) : string.Empty, 
                                pageNumber * pageSize ,
                                (pageNumber*pageSize) + pageSize);
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
                                             !string.IsNullOrWhiteSpace(where) ? string.Format(" WHERE {0}",where): string.Empty,
                                             !string.IsNullOrWhiteSpace(orderBy) ? string.Format(" ORDER BY {0}", orderBy) : string.Empty);            
        }


        public override void ConfigureParameterForValue(DbParameter param, object value)
        {
            param.DbType = Converters.GetDBTypeFor(param.Value);
            if (value == null || value == DBNull.Value)
                return;
            if (param.DbType == System.Data.DbType.Decimal)
            { 
                //set precision 
                (param as SqlParameter).Precision = 10; // this has to be configured manually
                (param as SqlParameter).Scale = 2; // this has to be configured manually
            }
            else if (value.GetType() == typeof(SqlGeography))
            {
                (param as SqlParameter).SqlDbType = System.Data.SqlDbType.Udt;
                (param as SqlParameter).UdtTypeName = "GEOGRAPHY";
            }
            else if (value.GetType() == typeof(SqlGeometry))
            {
                (param as SqlParameter).SqlDbType = System.Data.SqlDbType.Udt;
                (param as SqlParameter).UdtTypeName = "GEOMETRY";
            }
        }
        
    }
}
