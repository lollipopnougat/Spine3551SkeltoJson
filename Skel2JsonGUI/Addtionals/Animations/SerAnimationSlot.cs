using Newtonsoft.Json;
using SpineModelExtractor.SkelClasses;

namespace SpineModelExtractor.AdditionalClasses
{
    //public class SerAnimationSlotsContainer : ContainName
    //{
    //    public SerAnimationSlot[] SerAnimationSlots { get; set; }
    //}

    public class SerAnimationSlot : ContainName
    {
        public SerAnimSlotAttachment[] Attachments { get; set; }

        public SerAnimSlotColor[] Colors { get; set; }

        public SerAnimSlotTwoColor[] TwoColors { get; set; }
    }

    
    //name: attachment
    public class SerAnimSlotAttachment
    {
        public SerAnimSlotAttachmentFrame[] Frames { get; set; }
    }
    public class SerAnimSlotAttachmentFrame : SerTimeLineMember
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }



    //name: color
    public class SerAnimSlotColor
    {
        public SerAnimSlotColorFrame[] Frames { get; set; }
    }
    public class SerAnimSlotColorFrame : SerTimeLineMember
    {
        //RRGGBBAA
        [JsonProperty("color")]
        public string Color { get; set; }
    }

    //warning: Class not tested
    //name: twoColor 
    public class SerAnimSlotTwoColor : SerTimeLineMember
    {
        //RRGGBBAA
        [JsonProperty("light")]
        public string Light { get; set; }

        //RRGGBB
        [JsonProperty("dark")]
        public string Dark { get; set; }
    }

    
}
