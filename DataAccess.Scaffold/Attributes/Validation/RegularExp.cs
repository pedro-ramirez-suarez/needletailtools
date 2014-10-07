using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RegularExp : NeedletailAttribute
    {

        public RegularExp(string regExp)
            : base("Wrong RegularExp format", Constants.ErrorClass)
        {
            this.RegularExpression = regExp;
        }


        public RegularExp(string regExp, string errorMessage, string errorClass)
            : base(errorMessage, errorClass)
        {
            this.RegularExpression = regExp;
        }

        public string RegularExpression
        {
            get;
            private set;
        }
        public override string ValidatorName
        {
            get
            {
                return "regexp";
            }
        }

        private Dictionary<string, string> validatorDetails;
        public override Dictionary<string, string> ValidatorDetails
        {
            get
            {
                if (validatorDetails == null)
                    validatorDetails = new Dictionary<string, string>();
                validatorDetails.Add("RegExp",RegularExpression);         
                return validatorDetails;
            }
        }
    }
}
