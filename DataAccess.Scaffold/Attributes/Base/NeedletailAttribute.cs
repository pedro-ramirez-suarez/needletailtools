using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Scaffold.Attributes
{
    public abstract class NeedletailAttribute : Attribute
    {

        public abstract string ValidatorName { get; }
        
        public NeedletailAttribute(string errorMessage, string errorClass)
        {
            this.ErrorClass = errorClass;
            this.ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; private set; }
        public string ErrorClass { get; private set; }

        public  abstract Dictionary<string, string> ValidatorDetails { get; }

        
    }
}
