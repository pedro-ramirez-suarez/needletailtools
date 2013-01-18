using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Web.Script.Serialization;

namespace Needletail.Mvc
{
    public class ClientCall : DynamicObject
    {
        /// <summary>
        /// The ID of the client where the call will be executed, if this is empty a bradcast call will be executed
        /// </summary>
        public string ClientId { get; set; }
        public string CallerId { get; set; }
        private string Method { get; set; }
        private object[] Parameters { get; set; }
        private ClientCall Child { get; set; }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            //we are not invoking anything here
            result = null;
            //set the name of the method to invoke and the parameters
            Method = binder.Name; //this is to allow the user to use namespaced calls
            Parameters = args;

            //always return true
            return true;
            
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            //if clientId or callerid is invoked set the values
            if (binder.Name == "ClientId")
                result = this.ClientId;
            else if (binder.Name == "CallerId")
                result = this.CallerId;
            else
            {
                //we are not invoking, we try to use a namespace
                dynamic res = new ClientCall { };
                Child = res;
                result = res;
                //set the name of the method to invoke
                Method = binder.Name;
            }
            //always return true
            return true;
        }


        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            //if clientId or callerid is invoked set the values
            if (binder.Name == "ClientId")
                this.ClientId = value.ToString();
            else if (binder.Name == "CallerId")
                this.CallerId= value.ToString();
            else
                return false;
            //return true
            return true;
        }


        private object[] GetParameters()
        {
            if (this.Child == null)
                return this.Parameters;
            else
                return Child.Parameters;
        }

        public override string ToString()
        {
            string method = this.Child == null ?  this.Method : string.Concat(this.Method, ".", Child.Method);
            var cmd = new { command = method, parameters = GetParameters() };
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(cmd);
        }
        
    }
}

