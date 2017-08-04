using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Needletail.DataAccess.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ReadonlyFieldAttribute : Attribute
    {
        /* 
         
         * This attribute is used to mark a field as readonly, it's not included on insert and update operations
         
         */
    }
}
