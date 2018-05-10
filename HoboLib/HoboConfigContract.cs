/****************************** Module Header ******************************\
Module Name:    HoboConfigContract
Project:        HoboLib
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HoboLib
{
    [DataContract]
    public class HoboConfigContract
    {
        #region Public Constructors

        // ReSharper disable once EmptyConstructor
        public HoboConfigContract()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        [DataMember]
        public string HoboExportName { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public DateTime LastRun { get; set; }

        [DataMember]
        public Dictionary<string, string> Mappings { get; set; }

        [DataMember]
        public DateTime NextRun { get; set; }

        [DataMember]
        public string Sender { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return HoboExportName;
        }

        #endregion Public Methods
    }
}