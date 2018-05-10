using Base;
using System;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace HOBOToAQUARIUS
{
    public partial class HobotoAquarius : ServiceBase
    {
        #region Private Fields

        private readonly Settings settings;
        private readonly System.Timers.Timer timer;

        #endregion Private Fields

        #region Public Constructors

        public HobotoAquarius()
        {
            InitializeComponent();
            timer = new System.Timers.Timer();

            settings = XmlUtil<Settings>.Load(@"C:\HOBO_TO_AQUARIUS\settings.xml");
        }

        #endregion Public Constructors

        #region Public Methods

        public void Start()
        {
            OnStart(null);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnStart(string[] args)
        {
            Log.MakeEntry(@"Stopped", EventId.SERVICE_STATUS);

            timer.AutoReset = true;
            timer.Interval = (DateTime.Today.AddDays(1).AddHours(1) - DateTime.Now).TotalMilliseconds;
            timer.Start();

            timer.Elapsed += Timer_Elapsed;

            new Thread(() => { Timer_Elapsed(this, null); }).Start();//run on start
        }

        protected override void OnStop()
        {
            Log.MakeEntry(@"Stopped", EventId.SERVICE_STATUS);
        }

        #endregion Protected Methods

        #region Private Methods

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Emailer.Send(settings.EmailRecipients, @"HOBO To AQUARIUS", @"Database sync in progress. " + DateTime.UtcNow, settings.SmtpCredentials);

            var cfgs = new ConfigManager(@"C:\HOBO_TO_AQUARIUS\hobo_configs.xml", settings);

            var output = cfgs.RunConfigs();

            cfgs.Save();

            timer.Interval = (DateTime.Today.AddDays(1).AddHours(1) - DateTime.Now).TotalMilliseconds;
            EventLog.WriteEntry("Next run set for " + DateTime.Today.AddDays(1).AddHours(1) + "");

            var sb = new StringBuilder();

            sb.AppendLine(@"Database sync finished. " + DateTime.UtcNow);
            sb.AppendLine(@"Next Run: " + DateTime.Today.AddDays(1).AddHours(1));

            foreach (var line in output)
                sb.AppendLine(line);

            Emailer.Send(settings.EmailRecipients, @"HOBO To AQUARIUS", sb.ToString(), settings.SmtpCredentials);
        }

        #endregion Private Methods
    }
}