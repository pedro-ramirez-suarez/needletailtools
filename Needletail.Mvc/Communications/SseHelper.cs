using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Concurrent;

namespace Needletail.Mvc.Communications
{
    internal class SseHelper
    {
        static Dictionary<string, StreamWriter> clientStreams = new Dictionary<string, StreamWriter>();


        /// <summary>
        /// Adds a stream so we can send messages later
        /// </summary>
        internal static string AddStream(string id, StreamWriter streamWriter)
        {
            //check if the id already exists
            if (clientStreams.ContainsKey(id))
                clientStreams[id] = streamWriter;
            else
                clientStreams.Add(id, streamWriter);
            return id;
        }

        internal static bool SendMessage(ClientCall remoteCall, bool throwException = true)
        {
            if (remoteCall == null)
                throw new ArgumentNullException("remoteCall");
            //check if the id exists
            if (!clientStreams.ContainsKey(remoteCall.ClientId))
            {
                if (throwException)
                    throw new Exception("ClientId does not exist");
                else
                    return false;
            }

            //get the stream
            var st = clientStreams[remoteCall.ClientId];
            try
            {
                st.WriteLine(string.Concat ("data:", remoteCall.ToString() , "\n"));
                st.Flush();
            }
            catch (Exception e)
            { 
                //its very likely that the client its has been disconnected if an exception occurs
                return false;
            }
            return true;
        }


        internal static void BroadCastMessage(ClientCall remoteCall)
        {
            if (remoteCall == null)
                throw new ArgumentNullException("remoteCall");
            foreach (var k in clientStreams.Keys)
            {
                if (k != remoteCall.CallerId)
                {
                    remoteCall.ClientId = k;
                    SendMessage(remoteCall,false);
                }
            }
        }
    }
}
