using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Length : NeedletailAttribute
    {
        public Length(int length):
            base("Length should be " + length, Constants.ErrorClass)
        {
            this.Value = length;
        }

        public Length(int length, string errorMessage, string errorClass) :
            base (errorMessage, errorClass)
        {
            this.Value= length;
        }

        public int Value { get; private set; }

        public override string ValidatorName
        {
            get
            {
                return "stringLength";
            }
        }

        private Dictionary<string, string> validatorDetails;
        public override Dictionary<string, string> ValidatorDetails
        {
            get
            {
                if (validatorDetails == null)
                    validatorDetails = new Dictionary<string, string>();
                validatorDetails.Add("Len", Value.ToString());
                return validatorDetails;
            }
        }
    }
}
