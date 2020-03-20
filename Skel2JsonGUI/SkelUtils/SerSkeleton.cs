using Newtonsoft.Json;

namespace SpineModelExtractor.SkelClasses
{
    public class SerSkeleton
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("spine")]
        public string Spine { get; set; }

        [JsonProperty("width")]
        public float Width { get; set; }

        [JsonProperty("height")]
        public float Height { get; set; }

        [JsonProperty("images")]
        public string Images { get; set; }
    }
    
}
