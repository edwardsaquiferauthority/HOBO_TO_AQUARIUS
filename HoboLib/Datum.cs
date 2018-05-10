/****************************** Module Header ******************************\
Module Name:    Datum
Project:        HoboLib
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using System;

namespace HoboLib
{
    /// <summary>
    /// Class that defines an individual peice of data logged by the data logger
    /// </summary>
    [Serializable]
    public class Datum : ICloneable
    {
        #region Public Properties

        /// <summary>
        /// Description of the value
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Used to differentiate values in the same dataset that have the same description
        /// </summary>
        public string Differentiator { get; set; }

        /// <summary>
        /// Unit of the value
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// The double value
        /// </summary>
        public double Value { get; set; }

        #endregion Public Properties

        #region Internal Properties

        /// <summary>
        /// The ID of the logger
        /// </summary>
        internal string LoggerId { get; set; }

        /// <summary>
        /// The name of the logger
        /// </summary>
        internal string LoggerName { get; set; }

        /// <summary>
        /// The type of logger
        /// </summary>
        internal StationType StationType { get; set; }

        /// <summary>
        /// The timestamp of when the data was logged
        /// </summary>
        internal DateTime Timestamp { get; set; }

        #endregion Internal Properties

        #region Public Methods

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString()
        {
            return Description;
        }

        #endregion Public Methods
    }
}