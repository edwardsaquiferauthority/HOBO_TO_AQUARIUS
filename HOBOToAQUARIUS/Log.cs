/****************************** Module Header ******************************\
Module Name:    static Log
Project:        Mothership
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using Base;
using System;
using System.Diagnostics;

namespace HOBOToAQUARIUS
{
    public static class Log
    {
        #region Private Fields

        private static readonly TraceSource _traceSource = new TraceSource("Mothership");

        #endregion Private Fields

        #region Public Methods

        public static void MakeEntry(string msg, EventId id, EventLogEntryType type = EventLogEntryType.Information)
        {
            switch (type)
            {
                case EventLogEntryType.Information:
                    _traceSource.TraceData(TraceEventType.Information, (int)id, $"{msg}\nEventCode: {id}");
                    break;

                case EventLogEntryType.Warning:
                    _traceSource.TraceData(TraceEventType.Warning, (int)id, $"{msg}\nEventCode: {id}");
                    break;

                case EventLogEntryType.Error:
                    MakeEntry(msg, new Exception(msg), id);
                    break;

                default:
                    _traceSource.TraceData(TraceEventType.Verbose, (int)id, $"{msg}\nEventCode: {id}");
                    break;
            }

            _traceSource.Flush();

            EventLogManager.Instance.MakeEntry(msg, id, type);
        }

        public static void MakeEntry(string msg, Exception ex, EventId id, EventLogEntryType type = EventLogEntryType.Error, bool rethrow = true)
        {
            _traceSource.TraceData(TraceEventType.Error, (int)id, $@"{msg}: {ex}\nEventCode: {id}");
            _traceSource.Flush();

            EventLogManager.Instance.MakeEntry(msg, ex, id, type, rethrow);
        }

        #endregion Public Methods
    }
}