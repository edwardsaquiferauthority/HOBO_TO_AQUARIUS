/****************************** Module Header ******************************\
Module Name:    AquariusConfigContract
Project:        AquariusLib
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AquariusLib
{
    [Obsolete(@"Was only use for future planning to automate aquarius reporting")]
    [DataContract]
    public class AquariusConfigContract
    {
        #region Public Properties

        [DataMember]
        public bool Daily { get; set; }

        [DataMember]
        public DateTime FromTime { get; set; }

        [DataMember]
        public bool Hourly { get; set; }

        [DataMember]
        public Guid Id { get; set; } = Guid.NewGuid();

        [DataMember]
        public DateTime LastRun { get; set; }

        [DataMember]
        public List<Location> Locations { get; set; }

        [DataMember]
        public LocationType LocationsType { get; set; }

        [DataMember]
        public bool Monthly { get; set; }

        [DataMember]
        public DateTime NextRun { get; set; }

        [DataMember]
        public List<Parameter> Parameters { get; set; }

        [DataMember]
        public List<string> Paths { get; set; }

        [DataMember]
        public string ReportName { get; set; }

        [DataMember]
        public string Sender { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

        [DataMember]
        public DateTime ToTime { get; set; }

        [DataMember]
        public bool Weekly { get; set; }

        [DataMember]
        public bool Yearly { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return ReportName;
        }

        #endregion Public Methods
    }
}