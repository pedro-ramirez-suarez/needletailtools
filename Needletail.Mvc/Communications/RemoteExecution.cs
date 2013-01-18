using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Needletail.Mvc.Communications
{
    public class RemoteExecution
    {

               
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


        public static void BroadcastExecuteOnClient(ClientCall remoteCall)
        {
            if (remoteCall == null)
                throw new ArgumentNullException("remoteCall");
            
            //Send message to everyone
            SseHelper.BroadCastMessage(remoteCall);
        }
    }
}
