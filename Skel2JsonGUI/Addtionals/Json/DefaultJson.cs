using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SpineModelExtractor.AdditionalClasses.JsonDefaults
{
    public class DefaultJson
    {
        public static readonly JsonSerializerSettings DefaultSerializeSettings = new JsonSerializerSettings()
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter>() { new FloatConverter() }
        };

        public static readonly JsonSerializer DefaultSerializer = JsonSerializer.Create(DefaultSerializeSettings);
    }
}
