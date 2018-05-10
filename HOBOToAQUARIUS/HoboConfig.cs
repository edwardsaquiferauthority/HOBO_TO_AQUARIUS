using AquariusLib;
using Base;
using HoboLib.DataContracts;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace HOBOToAQUARIUS
{
    public class HoboConfig : Config
    {
        #region Public Constructors

        // ReSharper disable once EmptyConstructor
        public HoboConfig()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        [XmlIgnore]
        public Settings Settings { get; set; }

        public SerializableDictionary<string, string> ValueMap { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static SerializableDictionary<string, string> DictionaryToSerializableDictionary(Dictionary<string, string> mapping)
        {
            var serMapping = new SerializableDictionary<string, string>();

            foreach (var map in mapping)
                serMapping.Add(map.Key, map.Value);

            return serMapping;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void action()
        {
            var cfr = new CustomFileRequest(Settings.AuthenticationInfo.HoboAuthentication, Name);
            var export = cfr.ToDataSet();

            var acl = new AquariusClient(Settings.AuthenticationInfo.AquariusUid, Settings.AuthenticationInfo.AquariusPasswd);
            var ts = acl.HoboToAquarius(export, ValueMap);

            acl.UploadTimeSeries(ts);
        }

        #endregion Protected Methods
    }
}