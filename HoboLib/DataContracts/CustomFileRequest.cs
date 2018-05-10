/****************************** Module Header ******************************\
Module Name:    CustomFileRequest
Project:        HoboLib
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using Base;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace HoboLib.DataContracts
{
    /// <inheritdoc />
    /// <summary>
    /// Class used to create a HOBO web service request that has CSV data export
    /// </summary>
    [DataContract]
    public sealed class CustomFileRequest : HoboDataContract
    {
        #region Public Constructors

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="query"></param>
        public CustomFileRequest(Authentication auth, string query) : base(auth)
        {
            Query = query;
        }

        #endregion Public Constructors

        #region Public Properties

        [DataMember(Name = "query")]
        public string Query { get; private set; }

        #endregion Public Properties

        #region Protected Properties

        protected override string addr => @"https://webservice.hobolink.com/restv2/data/custom/file";

        #endregion Protected Properties

        #region Public Methods

        /// <summary>
        /// Executes the query and converts the data to a C# data structure, LoggerDataSet
        /// </summary>
        /// <returns></returns>
        public HoboExport ToDataSet()
        {
            var dataSets = new List<LoggerDataSet>();
            var dataPoints = new List<Datum>();
            var templateDataPoints = new List<Datum>();
            var stream = Util.GetStreamFromString(ExecuteQuery());

            using (var parser = new TextFieldParser(stream))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                var fields = parser.ReadFields();

                templateDataPoints.AddRange(from field in (fields ?? throw new InvalidOperationException()).Where((t, i) => i != 0)
                                            select field.Split(',')
                    into meta
                                            let id = meta[2].Split('_')
                                            let idExtra = id[2].Split(' ')
                                            select new Datum()
                                            {
                                                Description = meta[0],
                                                Unit = meta[1],
                                                LoggerId = id[0].Replace(" ", string.Empty),
                                                LoggerName = id[1],
                                                StationType = (StationType)Enum.Parse(typeof(StationType), idExtra[0]),
                                                Differentiator = idExtra.Length == 2 ? $@" {idExtra[1]}" : string.Empty
                                            });

                while (!parser.EndOfData)
                {
                    fields = parser.ReadFields();

                    if (fields != null && (fields[0] == @"Date" || fields[0] == @"No data found"))
                        break;

                    var currentRecord = DateTime.MinValue;

                    if (fields == null) continue;
                    for (var i = 0; i < fields.Length; i++)
                    {
                        var field = fields[i];

                        if (i == 0)
                        {
                            currentRecord = DateTime.ParseExact(field, "MM/dd/yy HH:mm:ss", null);
                            continue;
                        }

                        if (!(templateDataPoints[i - 1].Clone() is Datum dp)) continue;
                        dp.Timestamp = currentRecord;

                        dp.Value = field != string.Empty ? double.Parse(field) : double.NaN;

                        dataPoints.Add(dp);
                    }
                }
            }

            foreach (var set in dataPoints.GroupBy(x => x.LoggerId))
            {
                var tmp = new LoggerDataSet
                {
                    LoggerId = set.Key,
                    LoggerName = set.First().LoggerName,
                    StationType = set.First().StationType
                };

                foreach (var rec in set.ToList().GroupBy(x => x.Timestamp).ToList())
                {
                    var record = new Record
                    {
                        TimeStamp = rec.First().Timestamp,
                        Values = rec.ToList()
                    };
                    tmp.Records.Add(record);
                }

                dataSets.Add(tmp);
            }

            return new HoboExport() { DataSets = dataSets };
        }

        #endregion Public Methods
    }
}