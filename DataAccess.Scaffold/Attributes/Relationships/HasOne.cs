using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class HasOne : NeedletailRelationAttribute
    {

        public HasOne(string localObject , string foreignKey, string referencedTable, string referencedField)
        {
            this.LocalObject = localObject;
            this.ForeignKey = foreignKey;
            this.ReferencedTable = referencedTable;
            this.ReferencedField = referencedField;
        }

        public string LocalObject { get; private set; }
        public string ReferencedTable { get; private set; }
        public string ReferencedField { get; private set; }
    }
}
