using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpineModelExtractor.AdditionalClasses;

namespace SpineModelExtractor.SkelClasses
{
    public class SerEvent : ContainName
    {
        [JsonProperty("int")]
        [DefaultValue(0)]
        public int Int { get; set; }

        [JsonProperty("float")]
        [DefaultValue(0.0)]
        public float Float { get; set; }

        [JsonProperty("string")]
        [DefaultValue("")]
        public string String { get; set; }
    }
}
