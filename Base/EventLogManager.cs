/****************************** Module Header ******************************\
Module Name:    EventLogManager [singleton]
Project:        Base
Summary:        A logging class for managing file and windows event logs
                in a synchronous manner
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

#define USE_EVENT_LOG

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Timers;

namespace Base
{
    public enum EventId
    {
        SERVICE_STATUS = 100,
        CONFUIGURATION = 120,
        AUTOMATION_EVENT = 110,
        AQUARIUS_UPLOAD_EVENT = 111,
        HOBO_WEB_REQUEST = 113,
    }

    public class EventLogManager
    {
        private const string SOURCE = @"HOBO_TO_AQUARIUS_SERVICE";
        private const string LOG = @"HOBO_TO_AQUARIUS_SERVICE";
        private const string QUICK_LOG = "quick_log.txt";

        private static readonly Lazy<EventLogManager> _lazy =
        new Lazy<EventLogManager>(() => new EventLogManager());

        public static EventLogManager Instance => _lazy.Value;

        public string QuickLogLocation { get; set; }

        public string QuickLogPath => QuickLogLocation + QUICK_LOG;

        private readonly List<LogEntry> entries = new List<LogEntry>();

        private EventLogManager()
        {
#if USE_EVENT_LOG
            if (!EventLog.SourceExists(SOURCE))
                EventLog.CreateEventSource(SOURCE, LOG);
#endif

            var writeLog = new Timer();
            writeLog.Elapsed += (sender, args) => write();
            writeLog.Interval = 1000;
            writeLog.Start();
        }

        public void MakeEntry(string msg, EventId id, EventLogEntryType type = EventLogEntryType.Information)
        {
#if USE_EVENT_LOG
            EventLog.WriteEntry(SOURCE, msg, type, (int)id);
#endif
            entries.Add(new LogEntry()
            {
                Msg = msg,
                Id = id,
                Type = type
            });
        }

        public void MakeEntry(string msg, Exception ex, EventId id, EventLogEntryType type = EventLogEntryType.Error, bool rethrow = true)
        {
#if USE_EVENT_LOG
            EventLog.WriteEntry(SOURCE, $@"{msg}: {ex.Message}, Trace: {ex.StackTrace}", type, (int)id);
#endif
            entries.Add(new LogEntry()
            {
                Msg = msg,
                Id = id,
                Type = type,
                Exception = ex,
                ReThrow = rethrow
            });
        }

        private void write()
        {
            while (entries.Count > 0)
            {
                var entry = entries[0];

                if (entry.Exception == null)
                {
                    try
                    {
                        using (var stream = File.AppendText(QuickLogPath))
                        {
                            stream.WriteLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss}:{entry.Msg} : {entry.Type} : {entry.Id}");
                            stream.Flush();
                            stream.Close();
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
                else
                {
                    try
                    {
                        using (var stream = File.AppendText(QuickLogPath))
                        {
                            stream.WriteLine(
                                $"{DateTime.Now:dd-MM-yyyy HH:mm:ss}:{entry.Msg} : {entry.Exception} : {entry.Type} : {entry.Id}\n");
                            stream.Flush();
                            stream.Close();
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    if (entry.ReThrow)
                        throw entry.Exception; //rethrow exception to that is appears in the local WCF service log
                }

                entries.RemoveAt(0);
            }
        }
    }
}