/****************************** Module Header ******************************\
Module Name:    XmlUtil [static]
Project:        Base
Summary:        A utility class for serializing and deserializing C# classes
                into XML
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using System.IO;
using System.Xml.Serialization;

namespace Base
{
    /// <summary>
    /// Utility class for serializing XML file into classes
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public static class XmlUtil<T>
    {
        #region Public Methods

        /// <summary>
        /// Loads the XML file and serializes it
        /// </summary>
        /// <param name="path">the path of the XML file</param>
        /// <returns>Serialized class</returns>
        public static T Load(string path)
        {
            T instance;
            using (TextReader reader = new StreamReader(path))
            {
                var xml = new XmlSerializer(typeof(T));
                instance = (T)xml.Deserialize(reader);
            }
            return instance;
        }

        /// <summary>
        /// Saves the class to file
        /// </summary>
        /// <param name="path">path of the XML file</param>
        /// <param name="obj">object to serialize</param>
        public static void Save(string path, object obj)
        {
            using (TextWriter writer = new StreamWriter(path))
            {
                var xml = new XmlSerializer(typeof(T));
                xml.Serialize(writer, obj);
            }
        }

        #endregion Public Methods
    }
}