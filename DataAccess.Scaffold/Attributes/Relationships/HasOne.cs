using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    [System.AttributeUsage(AttributeTargets.Field | AttributeTargets.Property , AllowMultiple = true)]
    public class HasOne : Attribute
    {

        public HasOne( string referencedTable, string referencedField)
        {
            this.ReferencedTable = referencedTable;
            this.ReferencedField = referencedField;
        }

        public string ReferencedTable { get; private set; }
        public string ReferencedField { get; private set; }
    }
}
