using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Needletail.Mvc.Communications
{

    /// <summary>
    /// Use this to request remote execution on the client(browser)
    /// </summary>
    public class RemoteExecution
    {

               
        /// <summary>
        /// Execute the given call on the client
        /// </summary>
        /// <param name="remoteCall">The details of the call</param>
        /// <param name="raiseEventOnError">if false, no event will be raised if there is a communications problem</param>
        public static void ExecuteOnClient(ClientCall remoteCall, bool raiseEventOnError = true)
        {
            if (remoteCall == null)
                throw new ArgumentNullException("remoteCall");
            if (!SseHelper.SendMessage(remoteCall,raiseEventOnError) && !raiseEventOnError)
            { 
                //inform the user who made the call
                RemoteExecutionController.RaiseConnectionLostEvent(remoteCall);
            }
        }

        /// <summary>
        /// Perform the given call on all the connected clients
        /// </summary>
        /// <param name="remoteCall">Details of the call</param>
        public static void BroadcastExecuteOnClient(ClientCall remoteCall)
        {
            if (remoteCall == null)
                throw new ArgumentNullException("remoteCall");
            
            //Send message to everyone
            SseHelper.BroadCastMessage(remoteCall);
        }
    }
}
