using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Needletail.Mvc.Communications;
using System.Web.Http;
using System.Threading;

namespace Needletail.Mvc
{
    /// <summary>
    /// Use this to request remote calls on the client, you can use it in any controller
    /// </summary>
    public class TwoWayResult : ActionResult
    {

        internal ClientCall Call { get; private set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="call">The call to execute on the client</param>
        public TwoWayResult(ClientCall call)
        {
            this.Call = call;
        }

        /// <summary>
        /// The override does not send anything but makes a client call
        /// </summary>
        public override void ExecuteResult(ControllerContext context)
        {
            //make the client call
            if (string.IsNullOrEmpty(this.Call.ClientId))
                RemoteExecution.BroadcastExecuteOnClient(this.Call);
            else
            {
                RemoteExecution.ExecuteOnClient(this.Call, false);
                //wait until the call has been made so the connection is not trunckated
                int len = (int)(string.Concat("data:", this.Call.ToString(), "\n").Length / 10);
                while(true)
                {
                    if (SseHelper.ConnectionsMade.Contains(this.Call.ClientId))
                        break;
                    //wait a few miliseconds 
                    Thread.Sleep(len);
                } 

                //send just success
                var jsonp = new JavaScriptSerializer().Serialize(new { success = true });
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.Write(jsonp);
            }
            
            
        }
    }
}
