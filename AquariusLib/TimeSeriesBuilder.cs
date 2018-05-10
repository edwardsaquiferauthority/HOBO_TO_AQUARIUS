/****************************** Module Header ******************************\
Module Name:    TimeSeriesBuilder
Project:        AquariusLib
Summary:        Utility class for building aquarius accepted CSV 
                timeseries data
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

//#define NAN_FILTER

using AquariusLib.AcquisitionService;
using System;
using System.Text;

namespace AquariusLib
{
    public class TimeSeriesBuilder
    {
        public string Name => TimeSeriesDescription.Label;

        internal TimeSeriesDescription TimeSeriesDescription { get; }

        private readonly StringBuilder csv = new StringBuilder();

        /// <summary>
        /// Gets the aquarius formatted CSV data for this timeseries
        /// </summary>
        public string Csv => csv.ToString();

        internal TimeSeriesBuilder(TimeSeriesDescription timeSeriesDescription)
        {
            TimeSeriesDescription = timeSeriesDescription;
        }

        /// <summary>
        /// App3ends a value to this timeseries' CSV data
        /// </summary>
        /// <param name="timestamp">The timestamp of the data</param>
        /// <param name="value">The value to be appended</param>
        public void AppendValue(DateTime timestamp, double value)
        {
#if NAN_FILTER
            if (value == double.NaN) //Filter out NaN?
                return;
#endif
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            csv.AppendLine(value != double.NaN//not too concerned
                ? $@"{timestamp:yyyy-MM-dd hh:mm:ss}, {value},,,,,"
                : $@"{timestamp:yyyy-MM-dd hh:mm:ss},,,,,,");
        }
    }
}