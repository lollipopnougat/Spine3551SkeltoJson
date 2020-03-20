using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SpineModelExtractor.SkelClasses
{
    public class SerIK
    {
        [JsonProperty("order")]
        [DefaultValue(0)]
        public int Order { get; set; }

        [JsonProperty("target")]
        [DefaultValue(null)]
        public string Target { get; set; }

        [JsonProperty("name")]
        [DefaultValue(null)]
        public string Name { get; set; }

        [JsonProperty("bendPositive")]
        [DefaultValue(true)]
        public bool BendPositive { get; set; }

        [JsonProperty("mix")]
        [DefaultValue(1)]
        public float Mix { get; set; }

        [JsonProperty("bones")]
        [DefaultValue(null)]
        public string[] Bones { get; set; }
    }
}
