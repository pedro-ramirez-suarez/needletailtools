using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace Needletail.DataAccess.Engines {
    public class MySqlEngine : DBMSEngineBase, Needletail.DataAccess.Engines.IDBMSEngine
    {



        
        public override string GetQueryForPagination(string columns, string from, string where, string orderBy, int pageSize, int pageNumber, string key)
        {
            return string.Format("SELECT {0} FROM {1} {2} {3} LIMIT {4},{5}", 
                                columns,
                                from,
                                !string.IsNullOrWhiteSpace(where) ? string.Format(" WHERE {0}", where) : string.Empty,
                                !string.IsNullOrWhiteSpace(orderBy) ? string.Format(" ORDER BY {0}", orderBy) : string.Empty,
                                pageNumber * pageSize,
                                pageSize);
        }



        public override string GetOrderByQuery(string orderBy, SQLTokens.OrderBy direction) {
            return string.Format("{0} {1}", orderBy, direction == SQLTokens.OrderBy.DESC ? direction.ToString() : string.Empty);
        }


        public override string GetQueryTemplateForTop(string columns, string from, string where, string orderBy, int? topN)
        {
            return string.Format("SELECT {0} FROM {1} {2} {3} {4}",
                                             columns,
                                             from,
                                             !string.IsNullOrWhiteSpace(where) ? string.Format(" WHERE {0}", where) : string.Empty,
                                             !string.IsNullOrWhiteSpace(orderBy) ? string.Format(" ORDER BY {0}", orderBy) : string.Empty,
                                             topN.HasValue ? string.Format(" LIMIT {0}", topN.Value) : "");
        }

        
        
    }
}
