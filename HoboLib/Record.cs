/****************************** Module Header ******************************\
Module Name:    Record
Project:        HoboLib
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using System;
using System.Collections.Generic;

namespace HoboLib
{
    /// <summary>
    /// A set of values taken by the data logger
    /// </summary>
    [Serializable]
    public class Record
    {
        #region Public Properties

        public Guid Guid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The timestamp of the logging
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// The values logged by the data logger
        /// </summary>
        public List<Datum> Values { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $@"{TimeStamp.ToLongDateString()} - {TimeStamp.ToLongTimeString()}";
        }

        #endregion Public Methods
    }
}