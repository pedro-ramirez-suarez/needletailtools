using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Required : NeedletailAttribute
    {
        public Required()
            : base("This field is required" , Constants.ErrorClass)
        {
            this.IsRequired= true;
        }

        public Required(bool required,string errorMessage, string errorClass)
            : base(errorMessage, errorClass)
        {
            this.IsRequired = required;
        }
        public bool IsRequired { get; private set; }

        public override string ValidatorName
        {
            get
            {
                return "notEmpty";
            }
        }
    }
}
