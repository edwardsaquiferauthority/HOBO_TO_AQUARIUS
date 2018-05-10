/****************************** Module Header ******************************\
Module Name:    static AuthInfo
Project:        Mothership
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using HoboLib;

namespace HOBOToAQUARIUS
{
    /// <summary>
    /// Summary description for AuthInfo
    /// </summary>
    public class AuthInfo
    {
        #region Public Properties

        public string AquariusPasswd { get; set; } = ;//apply authenication here
        public string AquariusUid { get; set; } = ;
        public Authentication HoboAuthentication { get; set; } = ;

        #endregion Public Properties
    }
}