using System.ComponentModel;
using Newtonsoft.Json;

namespace SpineModelExtractor.SkelClasses
{
    //class SerBones
    //{
    //    [JsonProperty("bones")]
    //    public SerBone[] Bones { get; set; }
    //}
    public class SerBone
    {
        [JsonProperty(PropertyName = "name", Order = 1)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "parent", Order = 2)]
        public string Parent { get; set; }

        [JsonProperty(PropertyName = "y", Order = 5)]
        [DefaultValue(0.0)]
        public float Y { get; set; }

        [JsonProperty(PropertyName = "x", Order = 6)]
        [DefaultValue((float)0.0)]
        public float X { get; set; }

        [JsonProperty(PropertyName = "color", Order = 20)] //not used
        [DefaultValue(null)]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "scaleX", Order = 9)]
        [DefaultValue(1.0)]
        public float ScaleX { get; set; }

        [JsonProperty(PropertyName = "scaleY", Order = 10)]
        [DefaultValue(1.0)]
        public float ScaleY { get; set; }

        [JsonProperty(PropertyName = "shearX", Order = 7)]
        [DefaultValue(0.0)]
        public float ShearX { get; set; }

        [JsonProperty(PropertyName = "shearY", Order = 8)]
        [DefaultValue(0.0)]
        public float ShearY { get; set; }

        [JsonProperty(PropertyName = "rotation", Order = 4)]
        [DefaultValue(0.0)]
        public float Rotation { get; set; }

        [JsonProperty(PropertyName = "transform", Order = 11)]
        [DefaultValue("Normal")]
        public string Transform { get; set; }

        [JsonProperty(PropertyName = "length", Order = 3)]
        [DefaultValue(0.0)]
        public float Length { get; set; }
    }
}
