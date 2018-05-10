/****************************** Module Header ******************************\
Module Name:    TimeSeriesDataPoint [serializable]
Project:        AquariusLib
Summary:        Defines a datapoint within a timeseries
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using System;
using System.Globalization;

namespace AquariusLib
{
    [Serializable]
    public class TimeSeriesDataPoint
    {
        #region Public Constructors

        /// <summary>
        /// Constructs a datapoint from existing CSV data
        /// </summary>
        /// <param name="csv"></param>
        public TimeSeriesDataPoint(string csv)
        {
            var tmp = csv.Split(',');

            if (tmp[0] == string.Empty)
                return;

#pragma warning disable CS0642
            //Aquarius does not seem to always output a standard timestamp, so I've had to account for all the variations it has:
            if (tmp.Length > 0)
            {
                if (DateTime.TryParseExact(tmp[0], @"yyyy-M-dd h:m:s", CultureInfo.CurrentCulture, DateTimeStyles.None, out var timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-M-dd HH:m:s", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-M-dd HH:mm:s", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-M-dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-M-dd h:mm:s", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-M-dd h:m:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-M-dd h:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-MM-dd h:m:s", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-MM-dd HH:m:s", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-MM-dd HH:mm:s", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-MM-dd h:mm:s", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-MM-dd h:m:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-MM-dd h:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-M-d h:m:s", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-M-d HH:m:s", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-M-d HH:mm:s", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-M-d HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-M-d h:mm:s", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-M-d h:m:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-M-d h:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-MM-d h:m:s", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-MM-d HH:m:s", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-MM-d HH:mm:s", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-MM-d HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-MM-d h:mm:s", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-MM-d h:m:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
                else if (DateTime.TryParseExact(tmp[0], @"yyyy-MM-d h:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out timestamp))
                    ;
#pragma warning restore CS0642
                Timestamp = timestamp;
            }

            if (tmp.Length >= 1)
                Value = double.TryParse(tmp[1], out var val) ? val : double.NaN;

            if (tmp.Length >= 2)
                Flag = double.TryParse(tmp[2], out var val) ? val : double.NaN;

            if (tmp.Length >= 3)
                Grade = double.TryParse(tmp[3], out var val) ? val : double.NaN;

            if (tmp.Length >= 4)
                InterpolationCode = double.TryParse(tmp[4], out var val) ? val : double.NaN;

            if (tmp.Length >= 5)
                ApprovalCode = double.TryParse(tmp[5], out var val) ? val : double.NaN;

            if (tmp.Length == 6)
                Notes = tmp[6];
        }

        //Default constructor
        public TimeSeriesDataPoint()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// The records' approval code as shown in aquarius
        /// </summary>
        public double ApprovalCode { get; set; }

        /// <summary>
        /// The records' flag as shown in aquarius
        /// </summary>
        public double Flag { get; set; }

        /// <summary>
        /// The records' grade as shown in aquarius
        /// </summary>
        public double Grade { get; set; }

        /// <summary>
        /// The records' interpolation code as shown in aquarius
        /// </summary>
        public double InterpolationCode { get; set; }

        /// <summary>
        /// The records' notes as shown in aquarius
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// The records' timestamp as shown in aquarius
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The records's value
        /// </summary>
        public double Value { get; set; }

        #endregion Public Properties

        #region Internal Methods

        internal string ToCsv()
        {
            return $@"{Timestamp},{Value},{Flag},{Grade},{InterpolationCode},{ApprovalCode},{Notes}";
        }

        #endregion Internal Methods
    }
}