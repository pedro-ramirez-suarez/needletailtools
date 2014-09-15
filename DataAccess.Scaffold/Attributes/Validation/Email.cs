using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Email : NeedletailAttribute
    {

        public Email()
            : base("Wrong email format", Constants.ErrorClass)
        { 
        }


        public Email(string errorMessage, string errorClass)
            : base(errorMessage, errorClass)
        {
        }

        public override string ValidatorName
        {
            get
            {
                return "emailAddress";
            }
        }

    }
}
