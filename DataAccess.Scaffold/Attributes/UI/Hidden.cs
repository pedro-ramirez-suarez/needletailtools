using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes.UI
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Hidden : NeedletailAttribute
    {
        public Hidden()
            : base("", Constants.ErrorClass)
        { 
        }


        public Hidden(string errorMessage, string errorClass)
            : base(errorMessage, errorClass)
        {
        }

        public override string ValidatorName
        {
            get
            {
                return "hidden";
            }
        }

        public override Dictionary<string, string> ValidatorDetails
        {
            get { return new Dictionary<string, string>(); }
        }
    }
}
