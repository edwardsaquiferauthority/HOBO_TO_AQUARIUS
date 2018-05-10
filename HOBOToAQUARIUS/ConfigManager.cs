using Base;
using System;
using System.Collections.Generic;
using System.IO;

namespace HOBOToAQUARIUS
{
    public class ConfigManager
    {
        #region Private Fields

        private readonly string configPath;
        private readonly List<HoboConfig> configs = new List<HoboConfig>();

        #endregion Private Fields

        #region Public Constructors

        public ConfigManager(string path, Settings settings)
        {
            configPath = path;

            try
            {
                if (!File.Exists(configPath))
                    Save();

                configs = XmlUtil<List<HoboConfig>>.Load(configPath);

                foreach (var config in configs)
                {
                    config.Initialize();
                    config.Settings = settings;
                }
            }
            catch (Exception ex)
            {
                Log.MakeEntry($@"There was an error reading the Config File: {configPath}", ex, EventId.CONFUIGURATION);
            }
        }

        #endregion Public Constructors

        #region Public Methods

        public List<string> RunConfigs()
        {
            var output = new List<string>();

            foreach (var cfg in configs)
                output.Add(cfg.RunNow());

            return output;
        }

        public void Save()
        {
            try
            {
                XmlUtil<List<HoboConfig>>.Save(configPath, configs);
                Log.MakeEntry(@"The Automation Config File was saved.", EventId.CONFUIGURATION);
            }
            catch (Exception ex)
            {
                Log.MakeEntry(@"There was an error saving the Automation Config File", ex, EventId.CONFUIGURATION);
            }
        }

        #endregion Public Methods
    }
}