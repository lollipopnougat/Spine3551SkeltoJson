using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace SpineModelExtractor.AdditionalClasses.JsonDefaults
{
    public class FloatConverter : JsonConverter<float>
    {
        public override void WriteJson(JsonWriter writer, float value, JsonSerializer serializer)
        {
            //var roundValue = Math.Round(value, 5);
            //writer.WriteValue(roundValue.ToString());

            //var rawValue = $"\"{writer.Path}\": {value.ToString("0.####", CultureInfo.InvariantCulture)},";
            var rawValue = value.ToString("0.#####", CultureInfo.InvariantCulture);
            writer.WriteRawValue(rawValue);
            //writer.WriteRaw(rawValue);
        }

        public override float ReadJson(JsonReader reader, Type objectType, float existingValue, bool hasExistingValue, JsonSerializer serializer) => float.Parse((string)reader.Value);
    }
}
