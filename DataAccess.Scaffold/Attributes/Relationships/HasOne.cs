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

        public HasOne(string foreignKey, string referencedTable, string referencedKey)
        {
            this.ForeignKey = foreignKey;
            this.ReferencedTable = referencedTable;
            this.ReferencedKey = referencedKey;
        }

        public string ForeignKey { get; private set; }
        public string ReferencedTable { get; private set; }
        public string ReferencedKey { get; private set; }
    }
}
