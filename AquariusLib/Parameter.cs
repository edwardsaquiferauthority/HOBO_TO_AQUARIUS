/****************************** Module Header ******************************\
Module Name:    Parameter [serializable]
Project:        AquariusLib
Summary:        Defines a parameter as defined in aquarius
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using Base;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;

namespace AquariusLib
{
    [Serializable]
    public class Parameter
    {
        #region Public Constructors

        // ReSharper disable once EmptyConstructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public Parameter()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// The parameter's display id
        /// </summary>
        public string DisplayId { get; set; }

        /// <summary>
        /// The parameter's interpolation code
        /// </summary>
        public string Interpolation { get; set; }

        /// <summary>
        /// The parameter's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The parameter's id
        /// </summary>
        public string ParameterId { get; set; }

        /// <summary>
        /// The parameter's unit
        /// </summary>
        public string Unit { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns the name of the parameter
        /// </summary>
        /// <returns>The name of the parameter</returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion Public Methods

        #region Internal Methods

        internal static List<Parameter> ParseParameters(string _params)
        {
            var result = new List<Parameter>();

            using (var parser = new TextFieldParser(Util.GetStreamFromString(_params)))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                while (!parser.EndOfData)
                {
                    var fields = parser.ReadFields();

                    var tmp = new Parameter();

                    if (fields != null)
                        foreach (var _ in fields)
                        {
                            tmp.ParameterId = fields[0];
                            tmp.DisplayId = fields[1];
                            tmp.Name = fields[2];
                            tmp.Interpolation = fields[3];
                            tmp.Unit = fields[4];
                        }

                    result.Add(tmp);
                }
            }

            result.RemoveAt(0);
            return result;
        }

        #endregion Internal Methods
    }
}