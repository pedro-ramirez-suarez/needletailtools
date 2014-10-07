using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LessThan : NeedletailAttribute
    {
        public LessThan(int lessThan)
            : base("Value must be less than" + lessThan, Constants.ErrorClass)
        {
            this.Value = lessThan;
        }

        public LessThan(int lessThan, string errorMessage, string errorClass)
            : base(errorMessage, errorClass)
        {
            this.Value = lessThan;
        }

        public int Value { get; private set; }

        public override string ValidatorName
        {
            get
            {
                return "lessThan";
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
