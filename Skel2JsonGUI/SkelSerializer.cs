using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spine;
using SpineModelExtractor.AdditionalClasses;
using SpineModelExtractor.AdditionalClasses.JsonDefaults;
using SpineModelExtractor.SkelClasses;

namespace SpineModelExtractor
{
    class SkelSerializer
    {
        //private static readonly JsonSerializer DefaultSerializer = JsonSerializer.Create(new JsonSerializerSettings() {DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore });
        
        //not used code
        public static JToken Serialize(object serObject, string propName)
        {
            JToken root = null;
            var jArrayContainer = new JObject();
            //if array
            if (serObject is ContainName[] serNamedArray)
            {
                foreach (var namedObject in serNamedArray)
                {
                    var node = Serialize(namedObject, "none");
                    jArrayContainer.Add(node);
                }

                root = new JProperty(propName, jArrayContainer);
            }
            //single object
            else if (serObject is ContainName serNamedObject)
            {
                var fields = serObject.GetType().GetProperties();
                foreach (var fieldInfo in fields)
                {
                    //if ignored attr then skip this field
                    if (fieldInfo.GetCustomAttributes(true).OfType<JsonIgnoreAttribute>().Any())
                        continue;
                    var jToken = Serialize(fieldInfo.GetValue(serObject), fieldInfo.Name);
                    jArrayContainer.Add(jToken);
                }
                root = new JProperty(serNamedObject.Name, jArrayContainer);
            }
            //alg for non Named members
            else
            {
                try
                {
                    root = new JProperty(propName, JToken.FromObject(serObject));
                }
                catch
                {
                    root = new JProperty(propName, serObject);
                }
            }

            return root;
        }


        public static JToken SerializeModel(SkeletonData skeletonData)
        {
            var parser = new Parser(skeletonData);
            var rootDict  = new Dictionary<string, JToken>();
            rootDict.Add("skeleton", SerializeSkeleton(parser.GetSkeleton()));
            rootDict.Add("bones", SerializeBones(parser.GetBones()));
            rootDict.Add("slots", SerializeSlots(parser.GetSlots()));
            var iks = parser.GetIKs();
            if (iks.Length != 0)
            {
                rootDict.Add("ik", SerializeIKs(iks));
            }
            rootDict.Add("skins", SerializeSkins(parser.GetSkins()));
            var events = parser.GetEvents();
            if (events.Length != 0)
            {
                rootDict.Add("events", SerializeEvents(events));
            }
            rootDict.Add("animations", SerializeAnimations(parser.GetAnimations()));
            

            

            return JObject.FromObject(rootDict, DefaultJson.DefaultSerializer);
        }

        public static JToken SerializeIKs(SerIK[] serIks)
        {
            var rootArr = new JArray();
            foreach (var serIk in serIks)
            {
                rootArr.Add(JObject.FromObject(serIk));
            }

            return rootArr;
        }

        public static JToken SerializeEvents(SerEvent[] serEvents)
        {
            var rootDict = new Dictionary<string, JToken>();
            foreach (var serEvent in serEvents)
            {
                rootDict.Add(serEvent.Name, JObject.FromObject(serEvent, DefaultJson.DefaultSerializer));
            }

            return JObject.FromObject(rootDict, DefaultJson.DefaultSerializer);
        }

        public static JObject SerializeSkeleton(SerSkeleton serSkeleton)
        {
            var root = JObject.FromObject(serSkeleton);
            return root;
        }

        public static JArray SerializeSlots(SerSlot[] serSlots)
        {
            var root = new JArray();
            foreach (var serSlot in serSlots)
            {
                root.Add(JObject.FromObject(serSlot, DefaultJson.DefaultSerializer));
            }
            return root;
        }

        public static JToken SerializeBones(SerBone[] serBones)
        {
            var root = new JArray();
            foreach (var serBone in serBones)
            {
                root.Add(JObject.FromObject(serBone, DefaultJson.DefaultSerializer));
            }
            return root;
        }

        
        public static JObject SerializeSkins(SerSkin[] serSkins)
        {
            var root = new JObject();
            foreach (var serSkin in serSkins)
            {
                root.Add(new JProperty(serSkin.Name, SerializeSkinSlots(serSkin.Slots)));
            }
            
            return root;
        }

        public static JObject SerializeAnimations(SerAnimation[] serAnimations)
        {
            var root = new JObject();
            foreach (var serAnimation in serAnimations)
            {
                root.Add(new JProperty(serAnimation.Name, SerializeAnimation(serAnimation)));
            }

            return root;
        }

        public static JToken SerializeAnimation(SerAnimation serAnimation)
        {
            //var root = new JObject();
            var mainDict = new Dictionary<string, JToken>
            {
                {"bones", SerializeAnimationBones(serAnimation.Bones)},
                {"deform", SerializeAnimationDeforms(serAnimation.Deforms)},
                {"slots", SerializeAnimationSlots(serAnimation.Slots)}
            };
            return JObject.FromObject(mainDict, DefaultJson.DefaultSerializer);
        }

        public static JToken SerializeAnimationSlots(SerAnimationSlot[] serAnimationSlots)
        {
            var rootDict = new Dictionary<string, JToken>();

            foreach (var slot in serAnimationSlots)
            {
                var props = new Dictionary<string, JToken>();
                if (slot.Attachments != null)
                {
                    props.Add("attachment", JArray.FromObject(slot.Attachments[0].Frames, DefaultJson.DefaultSerializer));
                }
                if (slot.Colors != null)
                {
                    props.Add("color", JArray.FromObject(slot.Colors[0].Frames, DefaultJson.DefaultSerializer));
                }
                if (slot.TwoColors != null)
                {
                    props.Add("twocolor", JArray.FromObject(slot.TwoColors, DefaultJson.DefaultSerializer));
                }

                rootDict.Add(slot.Name, JObject.FromObject(props, DefaultJson.DefaultSerializer));
            }

            Console.WriteLine(JsonConvert.SerializeObject(rootDict, Formatting.Indented, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore }));

            return JObject.FromObject(rootDict);
        }

        public static JToken SerializeAnimationDeforms(SerAnimationDeform[] serAnimationDeforms)
        {
            var rootDict = new Dictionary<string, JObject>();
            var deforms = new Dictionary<string, JObject>();
            foreach (var serAnimationDeform in serAnimationDeforms)
            {
                var slots = new Dictionary<string, JArray>();
                foreach (var slot in serAnimationDeform.Slots)
                {
                    slots.Add(slot.Name, JArray.FromObject(slot.Frames, DefaultJson.DefaultSerializer));
                }
                deforms.Add(serAnimationDeform.Name, JObject.FromObject(slots, DefaultJson.DefaultSerializer));
            }
            rootDict.Add("default", JObject.FromObject(deforms, DefaultJson.DefaultSerializer));

            //Console.WriteLine(JsonConvert.SerializeObject(rootDict, Formatting.Indented, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore }));

            return JObject.FromObject(rootDict, DefaultJson.DefaultSerializer);
        }

        public static JToken SerializeAnimationBones(SerAnimationBone[] serAnimationBones)
        {
            //var root = new JObject();
            var rootDict = new Dictionary<string, JObject>();
            foreach (var serAnimationBone in serAnimationBones)
            {
                var props = new Dictionary<string, JArray>();
                if (serAnimationBone.Rotate != null)
                {
                    props.Add("rotate", JArray.FromObject(serAnimationBone.Rotate.Frames, DefaultJson.DefaultSerializer));
                }
                if (serAnimationBone.Scale != null)
                {
                    props.Add("scale", JArray.FromObject(serAnimationBone.Scale.Frames, DefaultJson.DefaultSerializer));
                }
                if (serAnimationBone.Shear != null)
                {
                    props.Add("shear", JArray.FromObject(serAnimationBone.Shear.Frames, DefaultJson.DefaultSerializer));
                }
                if (serAnimationBone.Translate != null)
                {
                    props.Add("translate", JArray.FromObject(serAnimationBone.Translate.Frames, DefaultJson.DefaultSerializer));
                }
                rootDict.Add(serAnimationBone.Name, JObject.FromObject(props));
            }

            //Console.WriteLine(JsonConvert.SerializeObject(rootDict, Formatting.Indented, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore}));

            return JObject.FromObject(rootDict, DefaultJson.DefaultSerializer);
        }
        #region SerializeSkin private sub functions

        private static JToken SerializeSkinSlots(SerSkinSlot[] slots)
        {
            var root = new JObject();
            foreach (var serSkinSlot in slots)
            {
                root.Add(new JProperty(serSkinSlot.Name, SerializeSkinSlotsAttachments(serSkinSlot.Attachments)));
            }

            return root;
        }

        private static JToken SerializeSkinSlotsAttachments(SerAttachment[] attachments)
        {
            var root = new JObject();
            foreach (var attachment in attachments)
            {
                root.Add(new JProperty(attachment.Name, JObject.FromObject(attachment, DefaultJson.DefaultSerializer)));
            }

            return root;
        }

        #endregion
    }
}
