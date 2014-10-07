using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MinLen : NeedletailAttribute
    {
        public MinLen(int minLen)
            : base("Max Length is " + minLen, Constants.ErrorClass)
        {
            this.Value= minLen;
        }

        public MinLen(int minLen,string errorMessage, string errorClass)
            : base(errorMessage, errorClass)
        {
            this.Value = minLen;
        }

        public int Value { get; private set; }

        public override string ValidatorName
        {
            get
            {
                return "min";
            }
        }

        private Dictionary<string, string> validatorDetails;
        public override Dictionary<string, string> ValidatorDetails
        {
            get
            {
                if (validatorDetails == null)
                    validatorDetails = new Dictionary<string, string>();
                validatorDetails.Add("Min", Value.ToString());
                return validatorDetails;
            }
        }
    }
}
