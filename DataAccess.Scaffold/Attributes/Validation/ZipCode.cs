using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ZipCode : NeedletailAttribute
    {

        public ZipCode()
            : base("Wrong ZipCode format", Constants.ErrorClass)
        {
        }


        public ZipCode(string errorMessage, string errorClass)
            : base(errorMessage, errorClass)
        {
        }

        public override string ValidatorName
        {
            get
            {
                return "zipCode";
            }
        }
    }
}
