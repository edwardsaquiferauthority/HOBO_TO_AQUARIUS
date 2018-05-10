/****************************** Module Header ******************************\
Module Name:    HoboExport
Project:        HoboLib
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace HoboLib
{
    /// <summary>
    /// Class used to store all HOBO data exported from a web service query, this class is serializable
    /// </summary>
    [Serializable]
    public class HoboExport
    {
        #region Public Properties

        /// <summary>
        /// The data sets
        /// </summary>
        public List<LoggerDataSet> DataSets { get; set; }

        /// <summary>
        /// A guid to identify the export
        /// </summary>
        public Guid Guid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The timestamp of the export
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Creates an instance of a HoboExport from an xml file
        /// </summary>
        /// <param name="fileName">The name/path of the XML file on disk</param>
        /// <returns>A HoboExport object</returns>
        public static HoboExport FromFile(string fileName)
        {
            using (var stream = File.OpenRead(fileName))
            {
                var serializer = new XmlSerializer(typeof(HoboExport));
                return serializer.Deserialize(stream) as HoboExport;
            }
        }

        /// <summary>
        /// Creates an instance of a HoboExport from an xml string
        /// </summary>
        /// <param name="xml">The xml string</param>
        /// <returns>A HoboExport object</returns>
        public static HoboExport FromXml(string xml)
        {
            using (var stream = Util.GetStreamFromString(xml))
            {
                var serializer = new XmlSerializer(typeof(HoboExport));
                return serializer.Deserialize(stream) as HoboExport;
            }
        }

        /// <summary>
        /// Gets an XML serialization of this HOBO export
        /// </summary>
        /// <returns>XML serialization of the HOBO export</returns>
        public string GetXml()
        {
            var xsSubmit = new XmlSerializer(typeof(HoboExport));
            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, this);
                    return sww.ToString();
                }
            }
        }

        #endregion Public Methods
    }
}