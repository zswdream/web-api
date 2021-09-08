using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SwitDish.Vendor.API.Tests.Utilities
{
    public static class Json
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            Converters = new List<JsonConverter> { new DecimalJsonConverter() }
        };

        /// <summary>
        /// formats json into a consistent readable format
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns>json string</returns>
        public static string JsonPrettyPrint(this string json)
        {
            return PrettyPrint(json);
        }

        /// <summary>
        /// formats json into a consistent readable format
        /// </summary>
        /// <param name="json">The json.</param>
        /// <param name="jsonSerializerSettings">Custom serializer settings.</param>
        /// <returns>json string</returns>
        public static string JsonPrettyPrint(this string json, JsonSerializerSettings jsonSerializerSettings)
        {
            return PrettyPrint(json, jsonSerializerSettings);
        }

        /// <summary>
        /// formats object into json in a consistent readable format
        /// </summary>
        /// <param name="value">The value to serialise.</param>
        /// <returns>json string</returns>
        public static string JsonPrettyPrint(this object value)
        {
            return PrettyPrint(value);
        }

        /// <summary>
        /// formats object into json in a consistent readable format
        /// </summary>
        /// <param name="value">The value to serialise.</param>
        /// <param name="jsonSerializerSettings">Custom serializer settings.</param>
        /// <returns>json string</returns>
        public static string JsonPrettyPrint(this object value, JsonSerializerSettings jsonSerializerSettings)
        {
            return PrettyPrint(value, jsonSerializerSettings);
        }

        /// <summary>
        /// formats json into a consistent readable format
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns>json string</returns>
        public static string PrettyPrint(string json)
        {
            return JToken.Parse(json).ToString(Formatting.Indented);
        }

        /// <summary>
        /// formats object into json in a consistent readable format
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="jsonSerializerSettings">Custom Json serializer settings.</param>
        /// <returns>
        /// json string
        /// </returns>
        public static string PrettyPrint(object value, JsonSerializerSettings jsonSerializerSettings)
        {
            // Configure the JSON formatter to always show null values - it's for the tests after all
            // and to always emit dates in UTC format
            string json = JsonConvert.SerializeObject(value, Formatting.Indented, jsonSerializerSettings);
            return json;
        }

        /// <summary>
        /// formats object into json in a consistent readable format
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// json string
        /// </returns>
        public static string PrettyPrint(object value)
        {
            // Configure the JSON formatter to always show null values - it's for the tests after all
            // and to always emit dates in UTC format
            string json = PrettyPrint(value, JsonSerializerSettings);
            return json;
        }

        /// <summary>
        /// Reads a file of json and formats it into a consistent readable format
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>json string</returns>
        public static string ReadJsonPrettyPrint(string path)
        {
            return PrettyPrint(File.ReadAllText(path));
        }

        /// <summary>
        /// Deserialises the specified json into a strong type, following the JsonSerializerSettings.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>an object of the requested type</returns>
        public static T Deserialise<T>(string value)
        {
            T fresh = JsonConvert.DeserializeObject<T>(value, JsonSerializerSettings);
            return fresh;
        }
    }
}
