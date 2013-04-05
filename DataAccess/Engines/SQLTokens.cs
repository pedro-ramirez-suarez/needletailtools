using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Needletail.DataAccess.Engines {
    public class SQLTokens {

        public enum OrderBy { ASC = 1, DESC = 2 };
        private static Random rnd = new Random((int)DateTime.Now.Ticks);

        public static class ComparisonStrings {
            public static Dictionary<string, string> Comparisons = new Dictionary<string, string>();
            static ComparisonStrings() {
                Comparisons.Add("_EndsWith", EndsWith);
                Comparisons.Add("_StartsWith", StartsWith);
                Comparisons.Add("_MoreThan", MoreThan);
                Comparisons.Add("_LessThan", LessThan);
                Comparisons.Add("_Like", Like);
                Comparisons.Add("_Not", Not);
                Comparisons.Add("_In", In);
                Comparisons.Add("And_",And);
                Comparisons.Add("Or_",Or);
                
            }
            internal const string EndsWith = " {0} LIKE '%' + {1} ";
            internal const string StartsWith = " {0} LIKE {1}+ '%' ";
            internal const string MoreThan = " {0} > {1} ";
            internal const string LessThan = " {0} < {1} ";
            internal const string Like = " {0} LIKE '%' + {1} + '%' ";
            internal const string Not = " {0} <> {1} ";
            internal const string Equal = "{0} = {1} ";
            internal const string In = " {0} IN ({1}) ";
            internal const string And = "And";
            internal const string Or = "Or";
        }


        public static string BuildCompare(ref string column,out string parameterName,ref string separator) {
            //check if its 
            string comparison = string.Empty;
            foreach (var k in ComparisonStrings.Comparisons.Keys) {
                if (column.StartsWith(k))
                {
                    separator = ComparisonStrings.Comparisons[k];
                    column = column.Replace(k, "");
                } 
                if (column.EndsWith(k)) {
                    comparison = ComparisonStrings.Comparisons[k];
                    column = column.Replace(k,"");
                }
                
            }
            if (string.IsNullOrEmpty(comparison)) {
                comparison = ComparisonStrings.Equal;
            }
            parameterName = string.Format("@{0}_{1}", column,rnd.Next());
            return string.Format(comparison, column, parameterName);
        }
    }
}
