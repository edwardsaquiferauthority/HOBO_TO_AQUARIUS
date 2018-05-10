/****************************** Module Header ******************************\
Module Name:    LoggerDataSet
Project:        HoboLib
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using System;
using System.Collections.Generic;

namespace HoboLib
{
    /// <summary>
    /// Class that defines an entire set of data from a data logger
    /// </summary>
    [Serializable]
    public class LoggerDataSet
    {
        #region Public Properties

        public Guid Guid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The ID of the logger
        /// </summary>
        public string LoggerId { get; set; }

        /// <summary>
        /// The name of the logger
        /// </summary>
        public string LoggerName { get; set; }

        /// <summary>
        /// The records returned from the data logger
        /// </summary>
        public List<Record> Records { get; set; } = new List<Record>();

        /// <summary>
        /// The type of logger
        /// </summary>
        public StationType StationType { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return LoggerId;
        }

        #endregion Public Methods
    }
}