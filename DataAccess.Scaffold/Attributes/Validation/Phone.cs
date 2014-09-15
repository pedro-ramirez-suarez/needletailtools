using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Phone : NeedletailAttribute
    {

        public Phone()
            : base("Wrong Phone format", Constants.ErrorClass)
        {
        }


        public Phone(string errorMessage, string errorClass)
            : base(errorMessage, errorClass)
        {
        }

        public override string ValidatorName
        {
            get
            {
                return "phone";
            }
        }
    }
}
