/****************************** Module Header ******************************\
Module Name:    LogEntry
Project:        Base
Summary:        Defines a log entry as used by the EventLogManager
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using System;
using System.Diagnostics;

namespace Base
{
    public class LogEntry
    {
        #region Public Properties

        /// <summary>
        /// The exception message if any
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// The event ID
        /// </summary>
        public EventId Id { get; set; }

        /// <summary>
        /// The log message
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// Indicated whether or not to rethrow the exception after logging
        /// </summary>
        public bool ReThrow { get; set; }

        /// <summary>
        /// The event log type
        /// </summary>
        public EventLogEntryType Type { get; set; }

        #endregion Public Properties
    }
}