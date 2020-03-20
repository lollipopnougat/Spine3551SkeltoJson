using System.ComponentModel;
using Newtonsoft.Json;
using SpineModelExtractor.SkelClasses;

namespace SpineModelExtractor.AdditionalClasses
{


    public class SerAnimationBone : ContainName
    {
        public SerAnimBoneRotate Rotate { get; set; }

        public SerAnimBoneTranslate Translate { get; set; }

        public SerAnimBoneScale Scale { get; set; }

        public SerAnimBoneShear Shear { get; set; }
    }

    //name: rotate
    public class SerAnimBoneRotate : SerAnimBoneRotateFramesContainer
    {
        
    }

    //name: translate
    public class SerAnimBoneTranslate : SerAnimBoneFramesContainer
    {

    }

    //name: scale
    public class SerAnimBoneScale : SerAnimBoneFramesContainer
    {

    }

    //name: shear
    public class SerAnimBoneShear : SerAnimBoneFramesContainer
    {

    }

    public abstract class SerAnimBoneRotateFramesContainer
    {
        public SerAnimBoneRotateFrame[] Frames { get; set; }
    }

    public abstract class SerAnimBoneFramesContainer
    {
        public SerAnimBoneFrame[] Frames { get; set; }
    }

    //public class SerAnimBoneFrame : SerCurvedMember
    //{
    //
    //}
    public class SerAnimBoneRotateFrame : SerTimeLineMember
    {
        [JsonProperty("angle")]
        [DefaultValue(null)]
        public float Angle { get; set; }

        //[JsonProperty("curve")]
        //[DefaultValue(null)]
        [JsonIgnore]
        public float[] Curve { get; set; }


        [JsonProperty("curve")]
        public object CurveF
        {
            get
            {
                if (Curve != null)
                {
                    return Curve;
                }
                else if (IsStepped == true)
                {
                    return "stepped";
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonIgnore]
        [DefaultValue(null)]
        public bool? IsStepped { get; set; }
    }

    public class SerAnimBoneFrame : SerTimeLineMember
    {
        [JsonIgnore]
        [DefaultValue(null)]
        public float[] Curve { get; set; }

        [JsonProperty("curve")]
        public object CurveF
        {
            get
            {
                if (Curve != null)
                {
                    return Curve;
                }
                else if (IsStepped == true)
                {
                    return "stepped";
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonProperty("x")]
        [DefaultValue(null)]
        public float X { get; set; }

        [JsonProperty("y")]
        [DefaultValue(null)]
        public float Y { get; set; }

        [JsonIgnore]
        [DefaultValue(null)]
        public bool? IsStepped { get; set; }
    }
}
