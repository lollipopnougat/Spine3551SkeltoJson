using System.ComponentModel;
using Newtonsoft.Json;
using SpineModelExtractor.AdditionalClasses;

namespace SpineModelExtractor.SkelClasses
{
    class SerAnimation : ContainName
    {
        //slot
        public SerAnimationSlot[] Slots { get; set; }

        //bone
        public SerAnimationBone[] Bones { get; set; }

        //deform
        public SerAnimationDeform[] Deforms { get; set; }
        
        ////////////NOT IMPLEMENTED///////////////
        
        //ik
        //public SerAnimationIK[] IKs { get; set; } 

        //transform
        //public SerAnimationTransform[] Transforms { get; set; } 

        //path
        //public SerAnimationPath[] Paths { get; set; } 

        //draw
        //public SerAnimationDraw[] Draws { get; set; } 

        //event
        //public SerAnimationEvent[] Events { get; set; } 

    }


    public class SerTimeLineMember
    {
        [JsonProperty("time")]
        [DefaultValue(null)]
        public float Time { get; set; }
    }
}
