/****************************** Module Header ******************************\
Module Name:    Location [serializable]
Project:        AquariusLib
Summary:        Defines a location in aquarius
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using AquariusLib.AcquisitionService;
using System;

namespace AquariusLib
{
    [Serializable]
    public class Location
    {
        #region Public Constructors

        /// <summary>
        /// default constructor
        /// </summary>
        public Location()
        {
        }

        #endregion Public Constructors

        #region Internal Constructors

        internal Location(LocationDTO location)
        {
            Id = location.LocationId ?? long.MinValue;
            Name = location.LocationName;
            Identifier = location.Identifier;
            Type = LocationType.CustomLocationType(location.LocationTypeName);
            Latitude = location.Latitude ?? float.NaN;
            Longitude = location.Longitude ?? float.NaN;
            Elevation = location.Elevation ?? float.NaN;
            UtcOffset = location.UtcOffset;
            ElevationUnit = location.ElevationUnits;
            Path = location.LocationPath;
            //ExtendedAttributes = location.ExtendedAttributes;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// The elevation of the location
        /// </summary>
        public float Elevation { get; set; }

        /// <summary>
        /// The unit of elevation
        /// </summary>
        public string ElevationUnit { get; set; }

        /// <summary>
        /// The location ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The location's string identifier
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// The location's latitude
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// The location's longitude
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// The location's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The location's data path in aquarius
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The location's type
        /// </summary>
        public LocationType Type { get; set; }

        /// <summary>
        /// The location's UTC offset
        /// </summary>
        public float UtcOffset { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns the name of the station
        /// </summary>
        /// <returns>The name of the station</returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion Public Methods

        #region Internal Methods

        internal static LocationDTO LOCATION_TO_LOCATIONDTO(Location location)
        {
            var tmp = new LocationDTO
            {
                LocationId = location.Id,
                LocationName = location.Name,
                Identifier = location.Identifier,
                LocationTypeName = location.Type.ToString(),
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Elevation = location.Elevation,
                UtcOffset = location.UtcOffset,
                LocationPath = location.Path,
                ElevationUnits = location.ElevationUnit,
                //ExtendedAttributes = location.ExtendedAttributes,
            };

            return tmp;
        }

        #endregion Internal Methods

        //public Dictionary<string, object> ExtendedAttributes { get; set; }
    }
}