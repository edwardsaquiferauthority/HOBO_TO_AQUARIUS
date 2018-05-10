/****************************** Module Header ******************************\
Module Name:    AquariusClient
Project:        AquariusLib
Summary:        Provides client access to both the AUQARIUS acuisition API
                and the public API
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using AquariusLib.AcquisitionService;
using AquariusLib.PublishService;
using Base;
using HoboLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AquariusLib
{
    public class AquariusClient
    {
        #region Private Fields

        private readonly AQAcquisitionServiceClient acuisitionClient;
        private readonly string passwd;
        private readonly AquariusPublishServiceClient publishClient;
        private readonly string uid;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="uid">The user ID for aquarius login</param>
        /// <param name="passwd">The user password for aquaiurs login</param>
        public AquariusClient(string uid, string passwd)
        {
            this.uid = uid;
            this.passwd = passwd;

            acuisitionClient = new AQAcquisitionServiceClient("WSHttpBinding_IAQAcquisitionService");
            publishClient = new AquariusPublishServiceClient("BasicHttpBinding_IAquariusPublishService");
        }

        #endregion Public Constructors

        #region Private Properties

        private string acuisitionToken => acuisitionClient.GetAuthToken(uid, passwd);
        private string publishToken => publishClient.GetAuthToken(uid, passwd);

        #endregion Private Properties

        #region Public Methods

        /// <summary>
        /// Creates a new location in the AQUARIUS database
        /// </summary>
        /// <param name="location">The location to be created</param>
        public void CreateLocation(Location location)
        {
            KeepAlive();
            var loc = Location.LOCATION_TO_LOCATIONDTO(location);
            acuisitionClient.CreateLocation(acuisitionToken, loc);
        }
        
        /// <summary>
        /// Gets a location by its ID from aquarius
        /// </summary>
        /// <param name="id">The locations ID</param>
        /// <returns>The requested location</returns>
        public Location GetLocationById(long id)
        {
            KeepAlive();
            return new Location(acuisitionClient.GetLocation(acuisitionToken, id));
        }

        /// <summary>
        /// Gets all the locations of a specified type
        /// </summary>
        /// <param name="type">The location type</param>
        /// <returns>A list of locations by type</returns>
        public List<Location> GetLocationByType(LocationType type)
        {
            KeepAlive();
            return acuisitionClient.GetAllLocations(acuisitionToken).Where(x => x.LocationTypeName == type.Name).Select(loc => new Location(loc)).ToList();
        }

        /// <summary>
        /// Gets all locations in aquarius
        /// </summary>
        /// <returns>Returns a list of all locations in aquarius</returns>
        public List<Location> GetLocations()
        {
            KeepAlive();
            var tmp = new List<Location>();

            var locs = acuisitionClient.GetAllLocations(acuisitionToken);

            foreach (var loc in locs)
                tmp.Add(new Location(loc));

            return tmp;
        }

        /// <summary>
        /// Gets all locations in aquarius asynchronously
        /// </summary>
        /// <returns>List of all locations in aquarius</returns>
        public async Task<List<Location>> GetLocationsAsync()
        {
            KeepAlive();
            var tmp = new List<Location>();

            var locs = await acuisitionClient.GetAllLocationsAsync(acuisitionToken);

            foreach (var loc in locs.GetAllLocationsResult)
                tmp.Add(new Location(loc));

            return tmp;
        }

        /// <summary>
        /// Gets a list of all paramerters being used by aquarius
        /// </summary>
        /// <returns>List of aquarius parameters</returns>
        public List<Parameter> GetParameters()
        {
            KeepAlive();
            return Parameter.ParseParameters(publishClient.GetParameterList(publishToken));
        }

        /// <summary>
        /// Gets timeseries data from a specified timeseries and date range
        /// </summary>
        /// <param name="ts">The timeseries to access</param>
        /// <param name="from">The from date</param>
        /// <param name="to">The to date</param>
        /// <returns>The CSV output from aquarius</returns>
        public async Task<string> GetTimeSeriesData(TimeSeries ts, DateTime from, DateTime to)
        {
            KeepAlive();
            var tmp = await acuisitionClient.GetTimeSeriesAsync(acuisitionToken, ts.Id, @"Public", from, to, DateTime.MinValue);
            return tmp.GetTimeSeriesResult;
        }

        /// <summary>
        /// Gets the timeseries data from aquarius synchonously
        /// </summary>
        /// <param name="ts">The timeseries to access</param>
        /// <param name="from">The from date</param>
        /// <param name="to">The to date</param>
        /// <returns>The CVS output from aquarius</returns>
        public string GetTimeSeriesDataSync(TimeSeries ts, DateTime from, DateTime to)
        {
            KeepAlive();
            string tmp;

            try
            {
                tmp = acuisitionClient.GetTimeSeries(acuisitionToken, ts.Id, @"Public", from, to, DateTime.MinValue);
            }
            catch (Exception)
            {
                var output = string.Empty;
                tmp = publishClient.GetTimeSeriesRawData(acuisitionToken, ts.Id.ToString(), @"Public", from.ToString(CultureInfo.CurrentCulture), to.ToString(CultureInfo.CurrentCulture), string.Empty, string.Empty, true, false);

                var lines = Regex.Split(tmp, "\r\n|\r|\n");

                for (var i = 5; i < lines.Length; i++)
                {
                    var data = lines[i].Split(',');

                    if (!(data.Length >= 6))
                        continue;

                    output += $"{data[1].Replace('T', ' ').Remove(19)},{data[2]},,,{data[3]},{data[4]},\n";
                }

                return output;
            }

            return tmp;
        }

        /// <summary>
        /// Gets a list of all timeseries in a particular location
        /// </summary>
        /// <param name="id">The location id</param>
        /// <returns>A list of available timeseries</returns>
        public List<TimeSeries> GetTimeSeriesList(long id)
        {
            KeepAlive();
            var tmp = new List<TimeSeries>();
            var timeSeries = acuisitionClient.GetTimeSeriesListForLocation(acuisitionToken, id);

            foreach (var ts in timeSeries)
                tmp.Add(new TimeSeries(ts, this));

            return tmp;
        }

        /// <summary>
        /// Gets a list of all timeseries in a particular location asynchonously
        /// </summary>
        /// <param name="id">The location id</param>
        /// <returns>A list of available timeseries</returns>
        public async Task<List<TimeSeries>> GetTimeSeriesListAsync(long id)
        {
            KeepAlive();
            var tmp = new List<TimeSeries>();
            var timeSeries = await acuisitionClient.GetTimeSeriesListForLocationAsync(acuisitionToken, id);

            foreach (var ts in timeSeries.GetTimeSeriesListForLocationResult)
                tmp.Add(new TimeSeries(ts, this));

            return tmp;
        }

        /// <summary>
        /// Generates an aquarius accepted formate from HOBOLink export data
        /// </summary>
        /// <param name="export">The export from HOBOlink</param>
        /// <param name="mapping">The data mapping (what data from hobo maps to what data in aquarius)</param>
        /// <returns>A list of uploadable timeseries data for aquarius</returns>
        public List<TimeSeriesBuilder> HoboToAquarius(HoboExport export, Dictionary<string, string> mapping)
        {
            KeepAlive();
            var tBuilder = new List<TimeSeriesBuilder>();

            foreach (var dataSet in export.DataSets)
            {
                var loggerId = acuisitionClient.GetLocationId(acuisitionToken, dataSet.LoggerId + Enum.GetName(typeof(StationType), dataSet.StationType));
                var timeSeries = acuisitionClient.GetTimeSeriesListForLocation(acuisitionToken, loggerId);

                foreach (var series in timeSeries)
                    foreach (var record in dataSet.Records)
                        foreach (var val in record.Values)
                            try
                            {
                                string label;

                                try
                                {
                                    label = mapping[$@"{val.Description}{val.Differentiator}"];
                                }
                                catch (Exception)
                                {
                                    label = mapping[$@"{val.Description}"];
                                }

                                if (label != series.Label) continue;
                                if (tBuilder.All(x => x.Name != label))
                                    tBuilder.Add(new TimeSeriesBuilder(series));

                                tBuilder.Single(x => x.Name == label).AppendValue(record.TimeStamp, val.Value);
                            }
                            catch (Exception ex)
                            {
                                EventLogManager.Instance.MakeEntry($@"{val.Description}{val.Differentiator} was not parsed", ex, EventId.AUTOMATION_EVENT, System.Diagnostics.EventLogEntryType.Warning);
                            }
            }

            return tBuilder;
        }

        /// <summary>
        /// Uploads timeseries data to aquarius
        /// </summary>
        /// <param name="ts">The timeseries to upload</param>
        public void UploadTimeSeries(TimeSeriesBuilder ts)
        {
            KeepAlive();
            try
            {
                acuisitionClient.AppendTimeSeriesFromBytes(acuisitionToken, ts.TimeSeriesDescription.AqDataID, Encoding.UTF8.GetBytes(ts.Csv), @"service_acc", @"Automatic Service Upload");
            }
            catch (Exception ex)
            {
                EventLogManager.Instance.MakeEntry($@"There was an error appending {ts.Name} to Aquarius({ts.TimeSeriesDescription.AqDataID})", ex, EventId.AQUARIUS_UPLOAD_EVENT);
            }
        }

        /// <summary>
        /// Uploads multiple timeseries to aquarius
        /// </summary>
        /// <param name="ts">A list of timeseries to upload</param>
        public void UploadTimeSeries(List<TimeSeriesBuilder> ts)
        {
            KeepAlive();
            foreach (var set in ts)
                UploadTimeSeries(set);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void KeepAlive()
        {
            acuisitionClient.KeepConnectionAlive(acuisitionToken);
        }

        #endregion Internal Methods
    }
}