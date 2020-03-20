using System.ComponentModel;
using Newtonsoft.Json;

namespace SpineModelExtractor.SkelClasses
{
    //class SerSlots
    //{
    //    [JsonProperty("slots")]
    //    public SerSlot[] Slots;
    //}

    public class SerSlot
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("bone")]
        public string Bone { get; set; }

        [JsonProperty("attachment")]
        public string Attachment { get; set; }

        [JsonProperty("color")]
        [DefaultValue("FFFFFFFF")]
        public string Color { get; set; }

        [JsonProperty("dark")]
        [DefaultValue("000000")]
        public string Dark { get; set; }

        [JsonProperty("blend")]
        [DefaultValue("normal")]
        public string Blend { get; set; }
    }
}
