/****************************** Module Header ******************************\
Module Name:    LocationType [serializable]
Project:        AquariusLib
Summary:        Defines the location types available in aquarius
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using System;
using System.Collections.Generic;

namespace AquariusLib
{
    [Serializable]
#pragma warning disable 660,661//can add equity members if needed
    public class LocationType
#pragma warning restore 660,661
    {

        #region Public Fields

        //Edit these as needed, if anything in aquarius changes, so must these
        public static LocationType Dam = new LocationType(@"Recharge Dams");
        public static LocationType Enviornmental = new LocationType(@"Environmental");
        public static LocationType RainGauge = new LocationType(@"Rain Gauges");
        public static LocationType Spring = new LocationType(@"Springs");
        public static LocationType Stream = new LocationType(@"Streams");
        public static LocationType WeatherStation = new LocationType(@"Weather Station");
        public static LocationType Well = new LocationType(@"Wells");

        public static List<LocationType> Types = new List<LocationType>()
        {
            Dam,
            Enviornmental,
            RainGauge,
            Spring,
            Stream,
            WeatherStation,
            Well
        };

        #endregion Public Fields

        #region Public Constructors

        public LocationType()
        {
        }

        #endregion Public Constructors

        #region Private Constructors

        private LocationType(string name)
        {
            Name = name;
        }

        #endregion Private Constructors

        #region Public Properties

        public string Name { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static bool operator !=(LocationType a, LocationType b)
        {
            return a?.Name != b?.Name;
        }

        public static bool operator ==(LocationType a, LocationType b)
        {
            return a?.Name == b?.Name;
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion Public Methods

        #region Internal Methods

        internal static LocationType CustomLocationType(string locationTypeName)
        {
            return new LocationType(locationTypeName);
        }

        #endregion Internal Methods
    }
}