using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    [System.AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class SelectFrom: Attribute
    {

        public SelectFrom(string referencedTable, string referencedField, string displayField)
        {
            this.ReferencedTable = referencedTable;
            this.ReferencedField = referencedField;
            this.DisplayField = displayField;
        }

        public string ReferencedTable { get; private set; }
        public string ReferencedField { get; private set; }
        public string DisplayField { get; private set; }
    }
}
