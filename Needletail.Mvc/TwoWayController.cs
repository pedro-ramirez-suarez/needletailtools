using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Net;
using System.Collections.Concurrent;
using Needletail.Mvc.Communications;
using System.Web.Http;
using System.Web;

namespace Needletail.Mvc
{
    public abstract class RemoteExecutionController : ApiController
    {

        /// <summary>
        /// Raises when a new connection is detected, use this to assign an ID for the new connection
        /// if the event is not handled or null or a duplicated value is returned, the library will assign 
        /// an id
        /// </summary>
        /// <returns>A disired id for the new connection</returns>
        public delegate string IncommingConnectionAssigningIdDelegate();
        public event IncommingConnectionAssigningIdDelegate IncommingConnectionAssigningId;


        /// <summary>
        /// Raises after the two-way stream is ready to be used, this returns the ID of the new connection
        /// If an ID was provided, it will try to use it, otherwise it will create a new one
        /// </summary>
        /// <returns>The final ID for the new connection</returns>
        public delegate void IncommingConnectionIdAssignedDelegate(string newId);
        public event IncommingConnectionIdAssignedDelegate IncommingConnectionIdAssigned;


        /// <summary>
        /// Fires when a connection is lost, this is only detected when a message is sent to a connection that does not exists anymore
        /// </summary>
        /// <returns>The Id of the broken connection</returns>
        public delegate void ConnectionLostDelegate(ClientCall call);
        public static event ConnectionLostDelegate ConnectionLost;      


        //This will be the url where the clients will get poll for messages
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            var response = request.CreateResponse();
            response.Content = new PushStreamContent(OnStreamAvailable, "text/event-stream");
            string newId;
            if (IncommingConnectionAssigningId != null)
                newId = IncommingConnectionAssigningId();
            else
                newId = null;
            if(string.IsNullOrWhiteSpace(newId))
                newId = Guid.NewGuid().ToString();
            response.Content.Headers.ContentLanguage.Add(string.Format("id-{0}",newId)); //This is to store the userid, the we will use a cookie
            CookieHeaderValue cookie = new CookieHeaderValue ("clientid","sample");
            IEnumerable<CookieHeaderValue> cookies = new List<CookieHeaderValue>();
            response.Headers.AddCookies(cookies);
            return response;
        }



        /// <summary>
        /// Stores the stream
        /// </summary>
        private void OnStreamAvailable(Stream stream, HttpContent headers, TransportContext context)
        {
            StreamWriter streamWriter = new StreamWriter(stream);
            var id = headers.Headers.ContentLanguage.FirstOrDefault(l=> l.StartsWith("id-"));
            if(id!= null)
            {
                id = id.Replace("id-", "");
                string newId;
                //we send an ID as a language
                newId = SseHelper.AddStream(id, streamWriter);
                if (IncommingConnectionIdAssigned != null)
                    IncommingConnectionIdAssigned(newId);
            }
        }

        internal static void RaiseConnectionLostEvent(ClientCall call)
        {
            if (ConnectionLost != null)
                ConnectionLost(call);
        }
    }
}
