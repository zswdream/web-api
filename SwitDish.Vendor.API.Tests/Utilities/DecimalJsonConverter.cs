using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.Vendor.API.Tests.Utilities
{
    public class DecimalJsonConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(decimal)) || (objectType == typeof(decimal?));
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        /// <exception cref="JsonSerializationException">
        /// Unexpected token type: " +
        /// token.Type
        /// </exception>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);

            if ((token.Type == JTokenType.Float) || (token.Type == JTokenType.Integer))
            {
                return token.ToObject<decimal>();
            }

            if (token.Type == JTokenType.String)
            {
                if (decimal.TryParse(
                    token.ToString(),
                    System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out decimal d))
                {
                    return d;
                }

                return null;
            }

            if ((token.Type == JTokenType.Null) && (objectType == typeof(decimal?)))
            {
                return null;
            }

            throw new JsonSerializationException(
                "Unexpected token type: " +
                token.Type);
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var d = default(decimal?);
            if (value != null)
            {
                d = value as decimal?;
                if (d.HasValue)
                {
                    // If value was a decimal?, then this is possible
                    d = new decimal(decimal.ToDouble(d.Value)); // The ToDouble-conversion removes all unnecessary precision
                }
            }

            JToken.FromObject(d).WriteTo(writer);
        }
    }
}
