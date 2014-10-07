using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class SelectFrom : NeedletailUIAttribute
    {

        public SelectFrom(string localList, string foreignKey,string referencedTable, string referencedField, string displayField)
        {
            this.LocalList = localList;
            this.ForeignKey = foreignKey;
            this.ReferencedTable = referencedTable;
            this.ReferencedField = referencedField;
            this.DisplayField = displayField;
        }

        public string LocalList { get; private set; }
        public string ReferencedTable { get; private set; }
        public string ReferencedField { get; private set; }
        public string DisplayField { get; private set; }
    }
}
