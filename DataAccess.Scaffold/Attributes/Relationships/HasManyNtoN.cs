using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class HasManyNtoN : NeedletailRelationAttribute
    {


        public HasManyNtoN(string localList, string foreignKey, string relationTable, string relationTableForeignKey, string relationTableReferencedKey, string referencedTable, string referencedKey)
        {
            this.LocalList = localList;
            this.ForeignKey = foreignKey;
            this.RelationTable = relationTable;
            this.RelationTableForeignKey = relationTableForeignKey;
            this.RelationTableReferencedKey = relationTableReferencedKey;
            this.ReferencedTable = referencedTable;
            this.ReferencedKey = referencedKey;
        }

        public string LocalList { get; private set; }

        public string RelationTable { get; private set; }

        public string RelationTableForeignKey { get; private set; }
        public string RelationTableReferencedKey { get; private set; }

        public string ReferencedTable { get; private set; }
        public string ReferencedKey { get; private set; }
    }
}
