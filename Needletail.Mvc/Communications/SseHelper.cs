using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;

namespace Needletail.Mvc.Communications
{

    /// <summary>
    /// This is the one that does the final job of sending the commands to the client(s)
    /// </summary>
    internal class SseHelper
    {
        /// <summary>
        /// A reference to all the connected clients
        /// </summary>
        static Dictionary<string, StreamWriter> clientStreams = new Dictionary<string, StreamWriter>();

        /// <summary>
        /// This holds a list of connections that are ready to send another message
        /// </summary>
        internal static List<string> ConnectionsMade = new List<string>();

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

        /// <summary>
        /// Sends a message to the proper client
        /// </summary>
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

            //remove the client id from the internal list
            if (ConnectionsMade.Contains(remoteCall.ClientId))
                ConnectionsMade.Remove(remoteCall.ClientId);

            //get the stream
            var st = clientStreams[remoteCall.ClientId];
            lock (st)
            {
                try
                {
                    //always send an empty package first
                    string data = string.Concat("data:", "-1", "\n\n");
                    st.WriteLine(data);
                    st.Flush();
                    //then send the real message
                    data = string.Concat("data:", remoteCall.ToString(), "\n\n");
                    st.WriteLine(data);
                    st.Flush();
                    //always send an empty package at the end
                    data = string.Concat("data:", "-1", "\n\n");
                    st.WriteLine(data);
                    st.Flush();
                    ConnectionsMade.Add(remoteCall.ClientId);
                }
                catch (Exception e)
                {
                    //its very likely that the client its has been disconnected if an exception occurs
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Performs the call in all the connected clients
        /// </summary>
        /// <param name="remoteCall">details of the call</param>
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

        /// <summary>
        /// determines if the client is online
        /// </summary>
        /// <param name="clientId">id of the client to check</param>
        internal static bool ClientIsOnLine(string clientId)
        {
            return clientStreams.ContainsKey(clientId);
        }
    }
}
