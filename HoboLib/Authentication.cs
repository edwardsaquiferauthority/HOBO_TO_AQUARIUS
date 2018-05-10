/****************************** Module Header ******************************\
Module Name:    Authentication
Project:        HoboLib
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

/****************************** WARNING ************************************\
THE SECURITY OF ANY PROJECT WITHIN THIS SOLUTION IS NIL. THESE TOOLS ARE
ONLY INTENDED FOR INTERNAL USE BY EAA, BUT CAN BE ADAPTED FOR ANYONE ELSE
\***************************************************************************/

using System.Runtime.Serialization;

namespace HoboLib
{
    /// <summary>
    /// Class to define the authentication required for HOBO access and web requests
    /// </summary>
    [DataContract]
    public class Authentication
    {
        #region Public Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">API access token</param>
        /// <param name="user">Username</param>
        /// <param name="passwd">Password</param>
        public Authentication(string token, string user, string passwd)
        {
            Token = token;
            UserId = user;
            Password = passwd;
        }

        public Authentication()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        [DataMember(Name = "password")]
        public string Password { get; set; }

        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "user")]
        public string UserId { get; set; }

        #endregion Public Properties
    }
}