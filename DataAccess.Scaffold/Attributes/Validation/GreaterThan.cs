using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class GreaterThan : NeedletailAttribute
    {
        public GreaterThan(int greaterThan)
            : base("Value must be Greater than" + greaterThan, Constants.ErrorClass)
        {
            this.Value = greaterThan;
        }

        public GreaterThan(int greaterThan, string errorMessage, string errorClass)
            : base(errorMessage, errorClass)
        {
            this.Value = greaterThan;
        }

        public int Value { get; private set; }

        public override string ValidatorName
        {
            get
            {
                return "greaterThan";
            }
        }

        private Dictionary<string, string> validatorDetails;
        public override Dictionary<string, string> ValidatorDetails
        {
            get
            {
                if (validatorDetails == null)
                    validatorDetails = new Dictionary<string, string>();
                validatorDetails.Add("Value", Value.ToString());
                return validatorDetails;
            }
        }
    }
}
