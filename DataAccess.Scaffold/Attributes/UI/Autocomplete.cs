using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class Autocomplete : NeedletailUIAttribute
    {

        public Autocomplete(string foreignKey, string referencedTable, string referencedField, string searchField, string displayField,string orderByField)
        {
            this.SearchField = searchField;
            this.ForeignKey = foreignKey;
            this.ReferencedTable = referencedTable;
            this.ReferencedField = referencedField;
            this.DisplayField = displayField;
            this.OrderByField = orderByField;
        }

        public string SearchField { get; private set; }
        public string ReferencedTable { get; private set; }
        public string ReferencedField { get; private set; }
        public string DisplayField { get; private set; }
        public string OrderByField { get; private set; }
    }
}
