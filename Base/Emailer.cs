/****************************** Module Header ******************************\
Module Name:    Emailer [static]
Project:        Base
Summary:        A static utility class for sending our service related emails
                in a synchronous manner
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace Base
{
    public static class Emailer
    {
        #region Public Methods

        /// <summary>
        /// Sends an email to each listed recipient
        /// </summary>
        /// <param name="recipients">List of recipients</param>
        /// <param name="subject">Email subject</param>
        /// <param name="message">Email content</param>
        /// <param name="credentials">STMP credentials</param>
        public static void Send(List<string> recipients, string subject, string message, NetworkCredential credentials)
        {
            var mail = new MailMessage(@"HOBO_TO_AQUARIUS_SERVICE@edwardsaquifer.org", recipients[0]);

            for (var i = 1; i < recipients.Count; i++)
                mail.To.Add(recipients[i]);

            var client = new SmtpClient
            {
                Port = 25,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = @"edwardsaquifer-org.mail.protection.outlook.com",
                Credentials = credentials,
                EnableSsl = true,
            };

            mail.Subject = subject;
            mail.Body = message;
            client.Send(mail);
        }

        #endregion Public Methods
    }
}