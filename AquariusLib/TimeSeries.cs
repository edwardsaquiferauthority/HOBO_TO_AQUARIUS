/****************************** Module Header ******************************\
Module Name:    TimeSeries [serializable]
Project:        AquariusLib
Summary:        Defines a timeseries as used by aquarius
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using AquariusLib.AcquisitionService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AquariusLib
{
    [Serializable]
    public class TimeSeries
    {
        #region Private Fields

        private readonly AquariusClient client;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// The default constructor
        /// </summary>
        public TimeSeries()
        {
        }

        #endregion Public Constructors

        #region Internal Constructors

        internal TimeSeries(TimeSeriesDescription descript, AquariusClient client)
        {
            this.client = client;
            Id = descript.AqDataID;
            Type = descript.Aqtimeseriestype;
            LastRecord = descript.EndTime;
            Units = descript.Units;
            NumberOfSamples = descript.TotalSamples;
            Identifier = descript.Identifier;
            LastModified = descript.LastModified;
            Name = descript.Label;
            ValueType = descript.ParameterType;
            ValueName = descript.ParameterName;
            Builder = new TimeSeriesBuilder(descript);
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// The timeseries' builder if needed
        /// </summary>
        [XmlIgnore]
        public TimeSeriesBuilder Builder { get; }

        /// <summary>
        /// The datapoints for this timeseries
        /// </summary>
        public List<TimeSeriesDataPoint> DataPoints { get; set; } = new List<TimeSeriesDataPoint>();

        /// <summary>
        /// The timeseries' id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The timeseries' string identifier
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// The last date this timeseries was changed
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// The last record available on this timeseries
        /// </summary>
        public DateTime LastRecord { get; set; }

        /// <summary>
        /// The timeseries' name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The number of samples available in this timeseries
        /// </summary>
        public long NumberOfSamples { get; set; }

        /// <summary>
        /// The type of timeseries
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The unit used for this timeseries' data
        /// </summary>
        public string Units { get; set; }

        /// <summary>
        /// The timeseries' value's name
        /// </summary>
        public string ValueName { get; set; }

        /// <summary>
        /// The timeseries' data value types
        /// </summary>
        public string ValueType { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Get an ordered list of data from this timeseries by day
        /// </summary>
        /// <returns></returns>
        public List<Tuple<DateTime, List<TimeSeriesDataPoint>>> GetByDay()
        {
            var ts0 = DateTime.MinValue;
            var hourly = new List<TimeSeriesDataPoint>();
            var output = new List<Tuple<DateTime, List<TimeSeriesDataPoint>>>();
            foreach (var dataPoint in DataPoints)
            {
                if (ts0 == DateTime.MinValue)
                    ts0 = dataPoint.Timestamp;

                if (dataPoint.Timestamp.Ticks <= ts0.AddDays(1).Ticks)
                {
                    hourly.Add(dataPoint);
                }
                else
                {
                    output.Add(new Tuple<DateTime, List<TimeSeriesDataPoint>>(dataPoint.Timestamp, hourly));
                    hourly = new List<TimeSeriesDataPoint>();
                    ts0 = dataPoint.Timestamp;
                    hourly.Add(dataPoint);
                }
            }

            return output;
        }

        /// <summary>
        ///  Get an ordered list of data from this timeseries by hour
        /// </summary>
        /// <returns></returns>
        public List<Tuple<DateTime, List<TimeSeriesDataPoint>>> GetByHour()
        {
            var ts0 = DateTime.MinValue;
            var hourly = new List<TimeSeriesDataPoint>();
            var output = new List<Tuple<DateTime, List<TimeSeriesDataPoint>>>();
            foreach (var dataPoint in DataPoints)
            {
                if (ts0 == DateTime.MinValue)
                    ts0 = dataPoint.Timestamp;

                if (dataPoint.Timestamp.Ticks < ts0.AddHours(1).Ticks)
                {
                    hourly.Add(dataPoint);
                }
                else
                {
                    output.Add(new Tuple<DateTime, List<TimeSeriesDataPoint>>(dataPoint.Timestamp, hourly));
                    hourly = new List<TimeSeriesDataPoint>();
                    ts0 = dataPoint.Timestamp;
                    hourly.Add(dataPoint);
                }
            }

            return output;
        }

        /// <summary>
        ///  Get an ordered list of data from this timeseries by month
        /// </summary>
        /// <returns></returns>
        public List<Tuple<DateTime, List<TimeSeriesDataPoint>>> GetByMonth()
        {
            var ts0 = DateTime.MinValue;
            var hourly = new List<TimeSeriesDataPoint>();
            var output = new List<Tuple<DateTime, List<TimeSeriesDataPoint>>>();
            var check = false;
            var month = DateTime.MinValue;

            foreach (var dataPoint in DataPoints)
            {
                month = dataPoint.Timestamp;
                if (ts0 == DateTime.MinValue)
                    ts0 = dataPoint.Timestamp;

                if (dataPoint.Timestamp.Ticks <= ts0.AddMonths(1).Ticks)
                {
                    hourly.Add(dataPoint);
                }
                else
                {
                    check = true;
                    output.Add(new Tuple<DateTime, List<TimeSeriesDataPoint>>(dataPoint.Timestamp, hourly));
                    hourly = new List<TimeSeriesDataPoint>();
                    ts0 = dataPoint.Timestamp;
                    hourly.Add(dataPoint);
                }
            }

            if (!check)
                output.Add(new Tuple<DateTime, List<TimeSeriesDataPoint>>(month, hourly));

            return output;
        }

        /// <summary>
        /// Gets a list of timeseries datapoints on the specified date range
        /// </summary>
        /// <param name="from">The from date</param>
        /// <param name="to">The to date</param>
        /// <returns></returns>
        public List<TimeSeriesDataPoint> GetData(DateTime from, DateTime to)
        {
            try
            {
                client.KeepAlive();
                //var tmp = AsyncHelpers.RunSync(() => client.GetTimeSeriesData(this, from, to));
                var tmp = client.GetTimeSeriesDataSync(this, from, to);

                foreach (var line in Regex.Split(tmp, "\r\n|\r|\n"))
                    DataPoints.Add(new TimeSeriesDataPoint(line));

                DataPoints.RemoveAll(x => x.Timestamp == DateTime.MinValue);

                return DataPoints;
            }
            catch (Exception) { return null; }
        }

        /// <summary>
        /// Gets a list of timeseries datapoints on the specified date range asynchronously
        /// </summary>
        /// <param name="from">The from date</param>
        /// <param name="to">The to date</param>
        /// <returns></returns>
        public async Task<List<TimeSeriesDataPoint>> GetDataAsync(DateTime from, DateTime to)
        {
            client.KeepAlive();
            var tmp = await client.GetTimeSeriesData(this, from, to);

            foreach (var line in Regex.Split(tmp, "\r\n|\r|\n"))
                DataPoints.Add(new TimeSeriesDataPoint(line));

            DataPoints.RemoveAll(x => x.Timestamp == DateTime.MinValue);

            return DataPoints;
        }

        /// <summary>
        /// Gets the Aquarius formatted CSV output from the timeseries
        /// </summary>
        /// <returns></returns>
        public string ToCsv()
        {
            var output = new StringBuilder();
            output.AppendLine($@"Timestamp,{ValueName}({Units}),Flag,Grade,Interpolation Code,Approval Code,Notes");

            foreach (var dp in DataPoints)
                output.AppendLine(dp.ToCsv());

            return output.ToString();
        }

        /// <summary>
        /// Returns the name of this timeseries
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion Public Methods
    }
}