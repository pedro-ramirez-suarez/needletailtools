using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Needletail.DataAccess.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public class TableKeyAttribute: Attribute
    {

        public bool CanInsertKey { get; set; }
    }
}
