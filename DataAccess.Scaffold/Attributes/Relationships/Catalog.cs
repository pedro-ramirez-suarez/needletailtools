using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Catalog : Attribute
    {


        public Catalog(string tableName)
        {
            this.TableName = tableName;
        }
        public string TableName { get; private set; }

    }
}
