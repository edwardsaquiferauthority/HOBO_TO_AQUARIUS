/****************************** Module Header ******************************\
Module Name:    abstract HoboDataContract
Project:        HoboLib
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using Base;
using HoboLib.DataContracts;
using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace HoboLib
{
    /// <summary>
    /// Abstract class for all HOBO data contracts
    /// </summary>
    [DataContract]
    [KnownType(typeof(CustomFileRequest))]
    public abstract class HoboDataContract
    {
        #region Protected Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="auth">The authentication for the session</param>
        protected HoboDataContract(Authentication auth)
        {
            Authentication = auth;
        }

        #endregion Protected Constructors

        #region Public Delegates

        /// <summary>
        /// Event handler for HoboDataContract queries
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args</param>
        public delegate void QueryCompleteHandler(object sender, QueryResponseArgs e);

        #endregion Public Delegates

        #region Public Events

        /// <summary>
        /// Fires when the query has completed and returned its output
        /// </summary>
        public event QueryCompleteHandler QueryComplete;

        #endregion Public Events

        #region Internal Properties

        [DataMember(Name = "authentication")]
        internal Authentication Authentication { get; set; }

        #endregion Internal Properties

        #region Protected Properties

        protected abstract string addr { get; }

        #endregion Protected Properties

        #region Public Methods

        /// <summary>
        /// Executes the query synchronously
        /// </summary>
        /// <returns>The response from HOBO</returns>
        public string ExecuteQuery()
        {
            var response = getWebRequest().GetResponse();
            var data = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()).ReadToEnd();
            response.Close();
            QueryComplete?.Invoke(this, new QueryResponseArgs(data));
            return data;
        }

        /// <summary>
        /// Executes the query asynchronously
        /// </summary>
        /// <returns>The response from HOBO</returns>
        public async Task<string> ExecuteQueryAsync()
        {
            var response = await getWebRequest().GetResponseAsync();
            response.Close();
            var data = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()).ReadToEnd();
            QueryComplete?.Invoke(this, new QueryResponseArgs(data));
            return data;
        }

        #endregion Public Methods

        #region Private Methods

        private byte[] getBytes()
        {
            var ms = new MemoryStream();
            var ser = new DataContractJsonSerializer(typeof(HoboDataContract));
            ser.WriteObject(ms, this);
            var bytes = ms.ToArray();
            ms.Close();
            return bytes;
        }

        private WebRequest getWebRequest()
        {
            try
            {
                var bytes = getBytes();
                var request = WebRequest.Create(addr);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = bytes.Length;

                var postStream = request.GetRequestStream();
                postStream.Write(bytes, 0, bytes.Length);
                postStream.Close();
                EventLogManager.Instance.MakeEntry(@"HOBO web request completed", EventId.HOBO_WEB_REQUEST);
                return request;
            }
            catch (Exception ex)
            {
                EventLogManager.Instance.MakeEntry(@"HOBO web request failed", ex, EventId.HOBO_WEB_REQUEST);
                return null;
            }
        }

        #endregion Private Methods

        #region Public Classes

        /// <summary>
        /// Class for defining the event arguments for a query response
        /// </summary>
        public class QueryResponseArgs : EventArgs
        {
            #region Public Constructors

            /// <summary>
            /// Default constructor
            /// </summary>
            /// <param name="response">The response from HOBO</param>
            public QueryResponseArgs(string response)
            {
                Response = response;
            }

            #endregion Public Constructors

            #region Public Properties

            /// <summary>
            /// The response from HOBO
            /// </summary>
            public string Response { get; }

            #endregion Public Properties
        }

        #endregion Public Classes
    }
}