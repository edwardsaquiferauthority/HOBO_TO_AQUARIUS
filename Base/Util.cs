/****************************** Module Header ******************************\
Module Name:    Util [static]
Project:        Base
Summary:        A utility class for misc. utility functions
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Base
{
    /// <summary>
    /// Static class to house utility functions
    /// </summary>
    public static class Util
    {
        #region Private Fields

        private static readonly Random _random = new Random();

        #endregion Private Fields

        #region Public Methods

        public static Color GetRandomColor()
        {
            return Color.FromArgb((byte)_random.Next(150, 255), (byte)_random.Next(150, 255), (byte)_random.Next(150, 255));
        }

        /// <summary>
        /// Generates a stream containing string data
        /// </summary>
        /// <param name="val">The text to put in the stream</param>
        /// <returns>A Stream object</returns>
        public static Stream GetStreamFromString(string val)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(val);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Pulls the word of the date from merrian webster (used for fun on extra long loading screens)
        /// </summary>
        /// <returns></returns>
        public static string GetTheWordOfTheDay()
        {
            try
            {
                var x = new WebClient();
                var source = x.DownloadString(@"https://www.merriam-webster.com/word-of-the-day");
                return Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
            }
            catch { return string.Empty; }
        }

        #endregion Public Methods
    }
}