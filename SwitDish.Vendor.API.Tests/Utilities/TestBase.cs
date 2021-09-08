using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SwitDish.Vendor.API.Tests.Utilities
{
    public class TestBase
    {
        protected ILogger MockLogger { get; private set; }

        /// <summary>
        /// Wire up RhinoMocks logger so that we can see what's going on in there.
        /// Clear the DataArrangerBase LogErrorForLater mechanism
        /// </summary>
        [SetUp]
        public virtual void TestsBase_SetUp()
        {
            // setup logger
        }

        /// <summary>
        /// Report (and fail the test for) any errors that couldn't be thrown at the time, e.g. from a dispose block
        /// </summary>
        [TearDown]
        public virtual void TestsBase_TearDown()
        {
        }

        /// <summary>
        /// Gets the test's expected result as json
        /// </summary>
        /// <returns>
        /// a cleaned json fragment.
        /// </returns>
        /// <remarks>
        /// reads a json file with the name of the NUnit test and pretty-prints it to ensure consistency.
        /// </remarks>
        protected static string GetTestJsonFile()
        {
            string callingAssemblyName = Assembly.GetCallingAssembly().GetName().Name;
            string testClassName = TestContext.CurrentContext.Test.ClassName;
            testClassName = testClassName.Replace(callingAssemblyName, string.Empty);
            string[] classNameParts = testClassName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            // Here we only add the first occurrence of the class name to get the folder path.
            string testFolderPath = string.Empty;
            string previousItem = null;
            foreach (string classNamePart in classNameParts)
            {
                // If first item or it doesn't match the previous item.
                if (previousItem == null || !classNamePart.Equals(previousItem, StringComparison.InvariantCultureIgnoreCase))
                {
                    testFolderPath = Path.Combine(testFolderPath, classNamePart);
                }

                previousItem = classNamePart;
            }

            return GetPathJsonFile($@"\{testFolderPath}\{TestContext.CurrentContext.Test.Name}.json");
        }

        /// <summary>
        /// Gets the test's expected result as json
        /// </summary>
        /// <returns>
        /// a cleaned json fragment.
        /// </returns>
        /// <remarks>
        /// reads a json file with the name of the NUnit test and pretty-prints it to ensure consistency.
        /// </remarks>
        protected static string GetTestJsonFile(string fileName)
        {
            string callingAssemblyName = Assembly.GetCallingAssembly().GetName().Name;
            string testClassName = TestContext.CurrentContext.Test.ClassName;
            testClassName = testClassName.Replace(callingAssemblyName, string.Empty);
            string[] classNameParts = testClassName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            // Here we only add the first occurrence of the class name to get the folder path.
            string testFolderPath = string.Empty;
            string previousItem = null;
            foreach (string classNamePart in classNameParts)
            {
                // If first item or it doesn't match the previous item.
                if (previousItem == null || !classNamePart.Equals(previousItem, StringComparison.InvariantCultureIgnoreCase))
                {
                    testFolderPath = Path.Combine(testFolderPath, classNamePart);
                }

                previousItem = classNamePart;
            }

            return GetPathJsonFile($@"\{testFolderPath}\{fileName}.json");
        }

        /// <summary>
        /// Gets the test's expected result as json
        /// </summary>
        /// <returns>
        /// a cleaned json fragment.
        /// </returns>
        /// <param name="textFilePath">The file path to the JSON file</param>
        /// <remarks>
        /// reads a json file with the name of the NUnit test and pretty-prints it to ensure consistency.
        /// </remarks>
        protected static string GetPathTextFile(string textFilePath)
        {
            // Strip any TestCase parameters from the JSON file path.
            int openingParenthesisIndex = textFilePath.IndexOf('(');
            if (openingParenthesisIndex > -1)
            {
                int closingParenthesisIndex = textFilePath.LastIndexOf(')');
                if (closingParenthesisIndex > -1)
                {
                    textFilePath = textFilePath.Substring(0, openingParenthesisIndex) + textFilePath.Substring(closingParenthesisIndex + 1);
                }
            }

            // Resolve full file path.
            textFilePath = FileSystem.GetFilePath(textFilePath);
            string rawText = File.ReadAllText(textFilePath);
            return rawText;
        }

        /// <summary>
        /// Convert a String to a Stream
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static Stream ConvertStringToStream(string value)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(value);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        protected static string BlankDateField(string incomingJson, string dateField)
        {
            string pattern = @"(""" + dateField + @"""\: ?""\d+\-\d+\-\d+\w\d+\:\d+\:\d+(\.\d+)?Z"")";
            return Regex.Replace(incomingJson, pattern, @"""" + dateField + @""": ""####-##-##T##:##:##.###Z""");
        }

        /// <summary>
        /// JSON serializes .130 as .13Z
        /// .NET only allows a fixed number of placeholders in the format string eg .fffZ so would serialize as .130Z
        /// For that reason we don't serialize the milliseconds into our expected JSON and remove them in the actual JSON.
        /// </summary>
        /// <param name="incomingJson"></param>
        /// <returns>string representation of a datetime with no timezone or milliseconds</returns>
        protected static string RemoveMillisecondsAndTimezone(string incomingJson)
        {
            return Regex.Replace(incomingJson, @"(\d\d:\d\d:\d\d)(\.\d+)?Z", @"$1");
        }

        /// <summary>
        /// round down a datetime so it doesn't contain any fractional seconds and won't run the risk of being rounded up
        /// </summary>
        /// <param name="incoming"></param>
        /// <remarks>from a suggestion in <see href="https://stackoverflow.com/a/18796370"/></remarks>
        /// <returns>datetime with no milliseconds</returns>
        /// TODO: should be static as it doesn't use any member variables
        protected static DateTime RemoveMilliseconds(DateTime incoming)
        {
            var interval = new TimeSpan(0, 0, 0, 1);
            return incoming.AddTicks(-(incoming.Ticks % interval.Ticks));
        }

        /// <summary>
        /// Gets the test's expected result as xml
        /// </summary>
        /// <returns>
        /// a cleaned xml fragment.
        /// </returns>
        /// <remarks>
        /// reads a xml file with the name of the NUnit test and pretty-prints it to ensure consistency.
        /// </remarks>
        protected static string GetTestXmlFile()
        {
            string callingAssemblyName = Assembly.GetCallingAssembly().GetName().Name;
            string testClassName = TestContext.CurrentContext.Test.ClassName;
            testClassName = testClassName.Replace(callingAssemblyName, string.Empty);
            string[] classNameParts = testClassName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            // Here we only add the first occurrence of the class name to get the folder path.
            string testFolderPath = string.Empty;
            string previousItem = null;
            foreach (string classNamePart in classNameParts)
            {
                // If first item or it doesn't match the previous item.
                if (previousItem == null || !classNamePart.Equals(previousItem, StringComparison.InvariantCultureIgnoreCase))
                {
                    testFolderPath = Path.Combine(testFolderPath, classNamePart);
                }

                previousItem = classNamePart;
            }

            return GetTestXmlFile($@"\{testFolderPath}\{TestContext.CurrentContext.Test.Name}.xml");
        }

        /// <summary>
        /// Gets the test's expected result as Xml
        /// </summary>
        /// <returns>
        /// a cleaned xml fragment.
        /// </returns>
        /// <param name="xmlFilePath">The file path to the xml file</param>
        /// <remarks>
        /// reads a xml file with the name of the NUnit test and pretty-prints it to ensure consistency.
        /// </remarks>
        protected static string GetTestXmlFile(string xmlFilePath)
        {
            // Strip any TestCase parameters from the XML file path.
            int openingParenthesisIndex = xmlFilePath.IndexOf('(');
            if (openingParenthesisIndex > -1)
            {
                int closingParenthesisIndex = xmlFilePath.LastIndexOf(')');
                if (closingParenthesisIndex > -1)
                {
                    xmlFilePath = xmlFilePath.Substring(0, openingParenthesisIndex) + xmlFilePath.Substring(closingParenthesisIndex + 1);
                }
            }

            // Resolve full file path.
            xmlFilePath = FileSystem.GetFilePath(xmlFilePath);
            string rawXml = File.ReadAllText(xmlFilePath);

            return rawXml.XmlPrettyPrint();
        }

        /// <summary>
        /// Gets the test's expected result as json
        /// </summary>
        /// <returns>
        /// a cleaned json fragment.
        /// </returns>
        /// <param name="jsonFilePath">The file path to the JSON file</param>
        /// <remarks>
        /// reads a json file with the name of the NUnit test and pretty-prints it to ensure consistency.
        /// </remarks>
        private static string GetPathJsonFile(string jsonFilePath)
        {
            string rawJson = GetPathTextFile(jsonFilePath);
            return Json.PrettyPrint(rawJson);
        }
    }
}
