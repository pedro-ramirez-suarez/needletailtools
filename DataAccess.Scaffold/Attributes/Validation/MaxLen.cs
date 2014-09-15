using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaxLen: NeedletailAttribute
    {
        public MaxLen(int maxLen)
            : base("Max Length is " + maxLen, Constants.ErrorClass)
        {
            this.Value= maxLen;
        }

        public MaxLen(int maxLen,string errorMessage, string errorClass)
            : base(errorMessage, errorClass)
        {
            this.Value = maxLen;
        }

        public int Value { get; private set; }

        public override string ValidatorName
        {
            get
            {
                return "max";
            }
        }
    }
}
