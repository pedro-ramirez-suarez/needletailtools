using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Between : NeedletailAttribute
    {

        public Between(int maxValue, int minValue) :
            base(string.Format("Value must be between {0} and {1}" , minValue, maxValue), Constants.ErrorClass)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }

        public Between(int maxValue, int minValue, string errorMessage, string errorClass) :
            base(errorMessage, errorClass)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }

        public int MaxValue { get; private set; }
        public int MinValue { get; private set; }

        public override string ValidatorName
        {
            get
            {
                return "between";
            }
        }
    }
}
