using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    public class ForeignKeyAttribute : Attribute
    {

        public string ForeignKey { get; protected set; }
    }
}
