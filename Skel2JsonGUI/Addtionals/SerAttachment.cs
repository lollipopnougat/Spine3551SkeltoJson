using System.ComponentModel;
using Newtonsoft.Json;

namespace SpineModelExtractor.AdditionalClasses
{
    //default is "region"
    //"skinnedmesh" => "weightedmesh"
    //"weightedmesh" => "mesh"
    //"weightedlinkedmesh" => "linkedmesh"
    //"mesh" == "skinnedmesh"


    
    public class ContainName
    {
        [JsonIgnore]
        public string Name { get; set; }
    }
    public class SerAttachment : ContainName
    {
        [JsonIgnore]
        public int SlotIndex { get; set; }

        [JsonProperty(PropertyName = "type", Order = 1)]
        [DefaultValue("region")]
        public string Type { get; set; }
    }

    public class SerClipping: SerAttachment //clipping
    {
        [JsonProperty(PropertyName = "end")]
        public string End { get; set; }

        [JsonProperty(PropertyName = "vertexCount")]
        public int VertexCount { get; set; }

        [JsonProperty(PropertyName = "vertices")]
        public float[] Vertices { get; set; }

        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }
    }

    public class SerRegion : SerAttachment //region
    {

        [JsonProperty(PropertyName = "width", Order = 7)]
        //[DefaultValue(32.0)]
        public float Width { get; set; }    //def 32

        [JsonProperty(PropertyName = "height", Order = 8)]
        //[DefaultValue(32.0)]
        public float Height { get; set; }     //def 32

        [JsonProperty(PropertyName = "x", Order = 2)]
        [DefaultValue(0.0)]
        public float X { get; set; }      //def 0

        [JsonProperty(PropertyName = "y", Order = 3)]
        [DefaultValue(0.0)]
        public float Y { get; set; }      //def 0

        [JsonProperty(PropertyName = "scaleX", Order = 4)]
        [DefaultValue(1.0)]
        public float ScaleX { get; set; } //def 1

        [JsonProperty(PropertyName = "scaleY", Order = 5)]
        [DefaultValue(1.0)]
        public float ScaleY { get; set; } //def 1

        [JsonProperty(PropertyName = "rotation", Order = 6)]
        [DefaultValue(0.0)]
        public float Rotation { get; set; } //def 0

        [JsonProperty(PropertyName = "color", Order = 9)]
        [DefaultValue(null)]
        public string Color  { get; set; }  //def null (rrggbbaa hex)
    }

    public class SerBoundingbox
    {

    }

    public class SerMesh : SerAttachment    //mesh and linkedmash
    {
        [JsonProperty(PropertyName = "uvs", Order = 2)]
        public float[] UVs { get; set; }

        [JsonProperty(PropertyName = "triangles", Order = 3)]
        public int[] Triangles { get; set; }

        [JsonProperty(PropertyName = "vertices", Order = 4)]
        public float[] Vertices { get; set; }

        [JsonProperty(PropertyName = "hull", Order = 5)]
        public int Hull { get; set; }

        [JsonProperty(PropertyName = "edges", Order = 6)]
        public int[] Edges { get; set; }

        [JsonProperty(PropertyName = "width", Order = 7)]
        [DefaultValue(0.0)]
        public float Width { get; set; }

        [JsonProperty(PropertyName = "height", Order = 8)]
        [DefaultValue(0.0)]
        public float Height { get; set; }
        
        [JsonProperty(PropertyName = "parent", Order = 9)]
        [DefaultValue(null)]
        public string Parent { get; set; }

        [JsonProperty(PropertyName = "skin", Order = 10)]
        [DefaultValue(null)]
        public string Skin { get; set; }

        [JsonProperty(PropertyName = "deform", Order = 11)]
        [DefaultValue(true)]
        public bool Deform { get; set; }

        [JsonProperty(PropertyName = "color", Order = 12)]
        [DefaultValue(null)]
        public string Color { get; set; }  //def null (rrggbbaa hex)
    }
    public class SerLinkedMesh : SerAttachment
    {

    }
    public class SerPath : SerAttachment
    {

    }
    public class SerPoint : SerAttachment
    {

    }
    

    
}
