using System.Collections.Generic;
using System.Net;

namespace HOBOToAQUARIUS
{
    public class Settings
    {
        #region Public Properties

        public AuthInfo AuthenticationInfo { get; set; } = new AuthInfo();
        public List<string> EmailRecipients { get; set; } = new List<string>();
        public NetworkCredential SmtpCredentials { get; set; } = new NetworkCredential(@"rcooper", @"Tr@pMRch451)Lat#(Stob*L", @"EAANET");

        #endregion Public Properties
    }
}