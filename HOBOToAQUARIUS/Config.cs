/****************************** Module Header ******************************\
Module Name:    Config generic
Project:        Mothership
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using Base;
using System;
using System.Diagnostics;

namespace HOBOToAQUARIUS
{
    public abstract class Config
    {
        #region Protected Constructors

        // ReSharper disable once EmptyConstructor
        protected Config()
        {
        }

        #endregion Protected Constructors

        #region Public Properties

        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime LastRun { get; set; }

        public string Name { get; set; }

        public string Sender { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Initialize()
        {
            Log.MakeEntry($@"Automation Config {Id} was initialized.", EventId.AUTOMATION_EVENT);
        }

        public string RunNow()
        {
            Log.MakeEntry($@"Automation Config {Id} started.", EventId.AUTOMATION_EVENT);

            try
            {
                var sw = new Stopwatch();
                sw.Start();

                action(); //do the action

                LastRun = DateTime.UtcNow;

                sw.Stop();
                Log.MakeEntry($@"Automation Config {Id} completed. Runtime: {sw.ElapsedMilliseconds}ms",
                    EventId.AUTOMATION_EVENT);

                return $@"Automation Config {Id} : {Name} completed at {LastRun}, Runtime: {sw.ElapsedMilliseconds}ms.";
            }
            catch (Exception ex)
            {
                Log.MakeEntry($@"Automation Config {Id} error", ex, EventId.AUTOMATION_EVENT);
                return $@"Automation Config {Id} failed. See log for details. {ex}";
            }
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract void action();

        #endregion Protected Methods
    }
}