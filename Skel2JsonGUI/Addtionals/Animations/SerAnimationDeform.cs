using System.ComponentModel;
using Newtonsoft.Json;
using SpineModelExtractor.SkelClasses;

namespace SpineModelExtractor.AdditionalClasses
{
    public class SerAnimationDeform : ContainName //slot name
    {
        public SerAnimDeformSlot[] Slots { get; set; }
    }

    public class SerAnimDeformSlot : ContainName //none name
    {
        public SerAnimDeformSlotFrame[] Frames { get; set; }
    }

    public class SerAnimDeformSlotFrame : SerTimeLineMember
    {
        [JsonProperty("offset")]
        [DefaultValue(0)]
        public int Offset { get; set; }

        [JsonProperty("vertices")]
        [DefaultValue(null)]
        public float[] Vertices { get; set; }

        [JsonProperty("curve")]
        [DefaultValue(null)]
        public float[] Curve { get; set; }

        [JsonIgnore]
        [DefaultValue(null)]
        public bool? IsStepped { get; set; }
    }
}
