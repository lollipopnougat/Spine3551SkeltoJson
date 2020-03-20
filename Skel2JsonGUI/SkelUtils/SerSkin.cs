using SpineModelExtractor.AdditionalClasses;

namespace SpineModelExtractor.SkelClasses
{
    //public class SerSkins : ContainName //skins
    //{
    //    public SerSkin[] Skins { get; set; }
    //}

    public class SerSkin : ContainName //default
    {
        public SerSkinSlot[] Slots { get; set; }
    }

    public class SerSkinSlot : ContainName //clipping
    {
        public SerAttachment[] Attachments { get; set; }
    }


    //public class SerSkinSlotAttachments : ContainName
    //{
    //    public SerAttachment[] Slots { get; set; }
    //}
}
