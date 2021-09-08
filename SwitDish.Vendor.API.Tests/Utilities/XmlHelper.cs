using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SwitDish.Vendor.API.Tests.Utilities
{
    public static class XmlHelper
    {
        private static readonly XmlWriterSettings XmlWriterSettings =
            new XmlWriterSettings { OmitXmlDeclaration = true, Indent = true, NewLineOnAttributes = true };

        /// <summary>
        /// Format the specified XML for purposed of test result comparison
        /// </summary>
        /// <param name="xml">Unformatted xml string</param>
        /// <returns>Clean xml string</returns>
        public static string PrettyXml(string xml)
        {
            var stringBuilder = new StringBuilder();
            var element = XElement.Parse(xml);

            using (var xmlWriter = XmlWriter.Create(stringBuilder, XmlWriterSettings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// formats xml into a consistent readable format
        /// </summary>
        /// <param name="xml">The xml.</param>
        /// <returns>json string</returns>
        public static string XmlPrettyPrint(this string xml)
        {
            return PrettyXml(xml);
        }
    }
}
