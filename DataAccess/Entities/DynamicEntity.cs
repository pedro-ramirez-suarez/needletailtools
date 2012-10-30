using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace Needletail.DataAccess.Entities
{
    public class DynamicEntity : DynamicObject
    {


        public DynamicEntity(List<string> fields)
        {
            Fields = fields;
            Values = new object[fields.Count];
        }

        public object this[string field]
        {
            get 
            {
                try
                {
                    return Values[Fields.IndexOf(field)];
                }
                catch
                {
                    return null;
                }
            }
            set 
            {
                try
                {
                    Values[Fields.IndexOf(field)] = value;
                }
                catch
                {
                    
                }
            }
        }


        private List<string> Fields;
        public object[] Values { get; set; }


        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            //if the property does not exist, we throw an exception
            try
            {
                result = Values[Fields.IndexOf(binder.Name)];
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }


        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            try
            {
                Values[Fields.IndexOf(binder.Name)] = value;
                return true;
            }
            catch {
                return false;
            }
        }
    }
}
