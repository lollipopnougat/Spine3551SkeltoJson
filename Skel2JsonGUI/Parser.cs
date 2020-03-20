using System;
using System.Collections.Generic;
using System.Linq;
using Spine;
using SpineModelExtractor.AdditionalClasses;
using SpineModelExtractor.SkelClasses;

namespace SpineModelExtractor
{
    class Parser
    {
        private SkeletonData skeletonData;
        public Parser(SkeletonData skeletonData)
        {
            this.skeletonData = skeletonData;
        }

        public SerSkeleton GetSkeleton()
        {
            return new SerSkeleton
            {
                Hash = skeletonData.Hash,
                Height = skeletonData.Height,
                Width = skeletonData.Width,
                Spine = skeletonData.Version,
                Images = skeletonData.ImagesPath
            };
        }

        public SerBone[] GetBones()
        {
            var serBones = new SerBone[skeletonData.Bones.Items.Length];
            for (int i = 0; i < skeletonData.Bones.Items.Length; i++)
            {
                var bone = skeletonData.Bones.Items[i];
                serBones[i] = new SerBone()
                {
                    Color = null,
                    Name = bone.Name,
                    Parent = bone.Parent?.Name,
                    Rotation = bone.Rotation,
                    ScaleX = bone.ScaleX,
                    ScaleY = bone.ScaleY,
                    ShearX = bone.ShearX,
                    ShearY = bone.ShearY,
                    X = bone.X,
                    Y = bone.Y,
                    Transform = Char.ToLowerInvariant(bone.TransformMode.ToString()[0]) + bone.TransformMode.ToString().Substring(1),
                    Length = bone.Length
                };
            }

            return serBones;
        }

        public SerSlot[] GetSlots()
        {
            var serSlots = new SerSlot[skeletonData.Slots.Items.Length];
            for (int i = 0; i < skeletonData.Slots.Items.Length; i++)
            {
                var slot = skeletonData.Slots.Items[i];
                serSlots[i] = new SerSlot()
                {
                    Name = slot.Name,
                    Bone = slot.BoneData.Name,
                    Attachment = slot.AttachmentName,
                    Blend = slot.BlendMode.ToString().ToLower(),
                    Color = FloatColorToHexString(slot.R) +
                            FloatColorToHexString(slot.G) +
                            FloatColorToHexString(slot.B) +
                            FloatColorToHexString(slot.A),
                    Dark = FloatColorToHexString(slot.R2) +
                           FloatColorToHexString(slot.G2) +
                           FloatColorToHexString(slot.B2)
                };
            }

            return serSlots;
        }

        public static string FloatColorToHexString(float color)
        {
            return ((byte)(color * 255)).ToString("X2");
        }

        

        public SerAttachment[] GetAttachments(Dictionary<Skin.AttachmentKeyTuple, Attachment> attachments)
        {
            var serAttachments = new SerAttachment[skeletonData.Skins.Items[0].Attachments.Count];
            var attachmentKeys = attachments.Keys.ToArray();
            var attachmentValues = attachments.Values.ToArray();
            for (int i = 0; i < skeletonData.Skins.Items[0].Attachments.Count; i++)
            {
                var attachmentKey = attachmentKeys[i];
                var attachmentValue = attachmentValues[i];
                var serAttachment = new SerAttachment();

                //case ClippingAttachment
                if (typeof(ClippingAttachment) == attachmentValue.GetType())
                {
                    var clipping = attachmentValue as ClippingAttachment;
                    var serClipping = new SerClipping()
                    {
                        Color = "FFFFFFFF",  //not find
                        End = clipping.EndSlot.Name,
                        Name = clipping.Name,
                        Type = "clipping",
                        VertexCount = clipping.Vertices.Length / 2, //maybe?
                        Vertices = clipping.Vertices
                    };
                    serAttachment = serClipping;
                }
                //case MeshAttachment
                else if (typeof(MeshAttachment) == attachmentValue.GetType())
                {
                    var mesh = attachmentValue as MeshAttachment;
                    var serMesh = new SerMesh()
                    {
                        Color = Parser.FloatColorToHexString(mesh.R) +
                                Parser.FloatColorToHexString(mesh.G) +
                                Parser.FloatColorToHexString(mesh.B) +
                                Parser.FloatColorToHexString(mesh.A),
                        Edges = mesh.Edges,
                        Height = mesh.RegionOriginalHeight,
                        Width = mesh.RegionOriginalWidth,
                        Hull = mesh.HullLength / 2,
                        Name = mesh.Name,
                        Parent = mesh.ParentMesh?.Name,
                        Skin = null,
                        Triangles = mesh.Triangles,
                        Vertices = mesh.Vertices,///###!!!
                        Type = "mesh",
                        UVs = mesh.RegionUVs
                    };
                    if (mesh.ParentMesh != null)
                    {
                        serMesh.Deform = mesh.InheritDeform;
                    }
                    serAttachment = serMesh;
                }
                //case region
                else if (typeof(RegionAttachment) == attachmentValue.GetType())
                {
                    var region = attachmentValue as RegionAttachment;
                    var serRegion = new SerRegion()
                    {
                        Name = region.Name,
                        Color = null,
                        Height = region.Height,
                        Width = region.Width,
                        Rotation = region.Rotation,
                        ScaleX = region.ScaleX,
                        ScaleY = region.ScaleY,
                        X = region.X,
                        Y = region.Y,
                        Type = "region"
                    };
                    serAttachment = serRegion;
                }
                //case boundingbox
                else if (typeof(SerBoundingbox) == attachmentValue.GetType())
                {
                    throw new NotImplementedException();
                }
                //case linkedmash
                else if (typeof(SerLinkedMesh) == attachmentValue.GetType())
                {
                    throw new NotImplementedException();
                }
                //case path
                else if (typeof(SerPath) == attachmentValue.GetType())
                {
                    throw new NotImplementedException();
                }
                //case point
                else if (typeof(SerPoint) == attachmentValue.GetType())
                {
                    throw new NotImplementedException();
                }

                serAttachment.SlotIndex = attachmentKey.slotIndex;
                serAttachments[i] = serAttachment;
            }

            return serAttachments;
        }


        #region skins

        public SerSkin[] GetSkins()
        {
            var serSkins = new SerSkin[skeletonData.Skins.Count];
            for (int i = 0; i < skeletonData.Skins.Count; i++)
            {
                serSkins[i] = new SerSkin()
                {
                    Name = skeletonData.Skins.Items[i].Name,
                    Slots = GetSkinSlots(skeletonData.Skins.Items[i].Attachments)
                };
            }

            return serSkins;
        }

        //helper method for GetSkins
        private SerSkinSlot[] GetSkinSlots(Dictionary<Skin.AttachmentKeyTuple, Attachment> attachments)
        {
            var serAttachments = GetAttachments(attachments);

            var serSkinSlots = new SerSkinSlot[skeletonData.Slots.Count];
            for (int i = 0; i < skeletonData.Slots.Count; i++)
            {
                serSkinSlots[i] = new SerSkinSlot()
                {
                    Attachments = serAttachments.Where(p => p.SlotIndex == i).ToArray(),
                    Name = skeletonData.Slots.Items[i].Name
                };
            }

            return serSkinSlots.OrderBy(s=>s.Name).ToArray();
        }

        #endregion

        public SerIK[] GetIKs()
        {
            var iksList = new List<SerIK>();
            foreach (var spineIkConstraint in skeletonData.IkConstraints)
            {
                var serIk = new SerIK
                {
                    BendPositive = spineIkConstraint.BendDirection == 1,
                    Name = spineIkConstraint.Name,
                    Mix = spineIkConstraint.Mix,
                    Order = spineIkConstraint.Order,
                    Target = spineIkConstraint.Target.Name,
                    Bones = spineIkConstraint.Bones.Select(b => b.Name).ToArray()
                };
                iksList.Add(serIk);
            }

            return iksList.ToArray();
        }

        public SerEvent[] GetEvents()
        {
            var eventsDict = new List<SerEvent>();
            foreach (var spineEvent in skeletonData.Events)
            {
                var serEvent = new SerEvent
                {
                    Float = spineEvent.Float,
                    Int = spineEvent.Int,
                    String = spineEvent.String,
                    Name = spineEvent.Name
                };
                eventsDict.Add(serEvent);
            }

            return eventsDict.ToArray();
        }

        public SerAnimation[] GetAnimations()
        {
            var spineAnimations = skeletonData.Animations;
            var serAnimations = new List<SerAnimation>();
            foreach (var spineAnimation in spineAnimations)
            {
                //for slots *need optimization
                var slotsList = new List<SerAnimationSlot>();
                var slotAttachmentsDict = new Dictionary<int, List<SerAnimSlotAttachment>>();
                var slotColorDict = new Dictionary<int, List<SerAnimSlotColor>>();
                var slotTwoColorDict = new Dictionary<int, List<SerAnimSlotTwoColor>>();

                //for bones *need optimization
                var bonesList = new List<SerAnimationBone>();
                var boneRotateDict = new Dictionary<int, SerAnimBoneRotate>();
                var boneScaleDict = new Dictionary<int, SerAnimBoneScale>();
                var boneShearDict = new Dictionary<int, SerAnimBoneShear>();
                var boneTranslateDict = new Dictionary<int, SerAnimBoneTranslate>();

                //for deforms *need optimization
                var deformList = new List<SerAnimationDeform>();
                var deformsDict = new Dictionary<int, List<SerAnimDeformSlot>>();

                

                foreach (var animationTimeline in spineAnimation.Timelines)
                {
                    //slots
                    if (animationTimeline is AttachmentTimeline spineAttachment)
                    {
                        
                        var frames = new SerAnimSlotAttachmentFrame[spineAttachment.FrameCount];
                        for (var i = 0; i < spineAttachment.FrameCount; i++)
                        {
                            frames[i] = new SerAnimSlotAttachmentFrame()
                            {
                                Name = spineAttachment.AttachmentNames[i],
                                Time = spineAttachment.Frames[i]
                            };
                        }
                        var slotAttachment = new SerAnimSlotAttachment(){ Frames = frames };
                        AddOrInsert(slotAttachmentsDict, slotAttachment, spineAttachment.SlotIndex);
                    }
                    else if (animationTimeline is ColorTimeline spineColor)
                    {
                        var frames = new SerAnimSlotColorFrame[spineColor.FrameCount];
                        for (int i = 0; i < spineColor.FrameCount; i++)
                        {
                            frames[i] = new SerAnimSlotColorFrame()
                            {
                                Time = spineColor.Frames[i * spineColor.FrameCount + 0],
                                Color = FloatColorToHexString(spineColor.Frames[i * spineColor.FrameCount + 1]) +
                                        FloatColorToHexString(spineColor.Frames[i * spineColor.FrameCount + 2]) +
                                        FloatColorToHexString(spineColor.Frames[i * spineColor.FrameCount + 3]) +
                                        FloatColorToHexString(spineColor.Frames[i * spineColor.FrameCount + 4])
                            };
                        }
                        var slotColor = new SerAnimSlotColor(){ Frames = frames };
                        AddOrInsert(slotColorDict, slotColor, spineColor.SlotIndex);
                    }
                    else if (animationTimeline is TwoColorTimeline spineTwoColor)
                    {
                        throw new NotImplementedException();
                    }
                    //bones
                    else if (animationTimeline is RotateTimeline spineRotate)
                    {
                        //var boneRotate = 
                        var frames = new SerAnimBoneRotateFrame[spineRotate.FrameCount];
                        for (int i = 0; i < spineRotate.FrameCount; i++)
                        {
                            frames[i] = new SerAnimBoneRotateFrame()
                            {
                                Time = spineRotate.Frames[2 * i + 0],
                                Angle = spineRotate.Frames[2 * i + 1],
                                Curve = spineRotate.RawCurve.Where(s => s.Key == i)
                                .Select(s => s.Value)
                                .FirstOrDefault(),
                                IsStepped = spineRotate.IsStepped
                                    .Where(s => s.Key == i)
                                    .Select(s => s.Value)
                                    .FirstOrDefault()
                            };
                        }
                        boneRotateDict.Add(spineRotate.BoneIndex, new SerAnimBoneRotate() { Frames = frames });
                    }
                    else if (animationTimeline is ScaleTimeline spineScale)
                    {
                        var boneScale = GetAnimBoneFrames<SerAnimBoneScale>(spineScale);
                        boneScaleDict.Add(spineScale.BoneIndex, boneScale);
                    }
                    else if (animationTimeline is ShearTimeline spineShear)
                    {
                        var boneShear = GetAnimBoneFrames<SerAnimBoneShear>(spineShear);
                        boneShearDict.Add(spineShear.BoneIndex, boneShear);
                    }
                    else if (animationTimeline is TranslateTimeline spineTranslate)
                    {
                        var boneTranslate = GetAnimBoneFrames<SerAnimBoneTranslate>(spineTranslate);
                        boneTranslateDict.Add(spineTranslate.BoneIndex, boneTranslate);
                    }
                    //deforms
                    else if (animationTimeline is DeformTimeline spineDeform)
                    {
                        var frames = new SerAnimDeformSlotFrame[spineDeform.FrameCount];
                        for (int i = 0; i < spineDeform.FrameCount; i++)
                        {
                            frames[i] = new SerAnimDeformSlotFrame()
                            {
                                Time = spineDeform.Frames[i],
                                Offset = FindInDict(spineDeform.RawOffset, i),
                                Vertices = FindInDict(spineDeform.RawVertices, i),
                                Curve = FindInDict(spineDeform.RawCurve, i),
                                IsStepped = FindInDict(spineDeform.IsStepped, i)
                            };
                        }

                        if (!deformsDict.ContainsKey(spineDeform.SlotIndex))
                        {
                            var deformSlotDict = new List<SerAnimDeformSlot>
                            {
                                new SerAnimDeformSlot()
                                {
                                    Frames = frames,
                                    Name = spineDeform.Attachment.Name
                                }
                            };
                            deformsDict.Add(spineDeform.SlotIndex, deformSlotDict);
                            
                        }
                        else
                        {
                            deformsDict[spineDeform.SlotIndex].Add(new SerAnimDeformSlot()
                            {
                                Frames = frames,
                                Name = spineDeform.Attachment.Name
                            });
                            
                        }
                    }
                    
                }


                //build animation slots
                for (int i = 0; i < skeletonData.Slots.Items.Length; i++)
                {
                    var currentAnimationSlot = new SerAnimationSlot()
                    {
                        Attachments = GetArrElemsById(slotAttachmentsDict, i),
                        Colors = GetArrElemsById(slotColorDict, i),
                        TwoColors = GetArrElemsById(slotTwoColorDict, i),
                        Name = skeletonData.Slots.Items[i].Name
                    };
                    if (currentAnimationSlot.Attachments != null || currentAnimationSlot.Colors != null || currentAnimationSlot.TwoColors != null)
                    {
                        slotsList.Add(currentAnimationSlot);
                    }
                }

                //build animation bones
                for (int i = 0; i < skeletonData.Bones.Items.Length; i++)
                {
                    var currentAnimationBone = new SerAnimationBone()
                    {
                        Rotate = GetElemById(boneRotateDict, i),
                        Scale = GetElemById(boneScaleDict, i),
                        Shear = GetElemById(boneShearDict, i),
                        Translate = GetElemById(boneTranslateDict, i),
                        Name = skeletonData.Bones.Items[i].Name
                    };
                    if (currentAnimationBone.Rotate != null || currentAnimationBone.Scale != null || currentAnimationBone.Shear != null || currentAnimationBone.Translate != null)
                    {
                        bonesList.Add(currentAnimationBone);
                    }
                }

                //build animation deforms
                foreach (var rawDeform in deformsDict)
                {
                    deformList.Add(new SerAnimationDeform()
                    {
                        Name = skeletonData.Slots.Items[rawDeform.Key].Name,
                        Slots = rawDeform.Value.ToArray()
                    });
                }


                serAnimations.Add(new SerAnimation()
                {
                    Bones = bonesList.ToArray(),
                    Deforms = deformList.ToArray(),
                    Name = spineAnimation.Name,
                    Slots = slotsList.ToArray()
                });
            }


            return serAnimations.ToArray();
        }

        private static T GetElemById<T>(Dictionary<int, T> dictionary, int index)
        {
            return dictionary.Where(s => s.Key == index).Select(s => s.Value).FirstOrDefault();
        }

        private static T[] GetArrElemsById<T>(Dictionary<int, List<T>> dictionary, int index)
        {
            return dictionary.Where(s => s.Key == index).Select(s => s.Value).FirstOrDefault()?.ToArray();
        }

        private static T GetAnimBoneFrames<T>(TranslateTimeline spineTranslate) where  T: SerAnimBoneFramesContainer, new()
        {
            var frames = new SerAnimBoneFrame[spineTranslate.FrameCount];
            for (int i = 0; i < spineTranslate.FrameCount; i++)
            {
                frames[i] = new SerAnimBoneFrame()
                {
                    Time = spineTranslate.Frames[3 * i + 0],
                    X = spineTranslate.Frames[3 * i + 1],
                    Y = spineTranslate.Frames[3 * i + 2],
                    Curve = spineTranslate.RawCurve
                        .Where(s => s.Key == i)
                        .Select(s => s.Value)
                        .FirstOrDefault(),
                    IsStepped = spineTranslate.IsStepped
                        .Where(s => s.Key == i)
                        .Select(s => s.Value)
                        .FirstOrDefault()
                };
            }
            return new T() { Frames = frames };
        }

        private static T FindInDict<T>(IDictionary<int, T> dictionary, int index)
        {
            return dictionary
                .Where(s => s.Key == index)
                .Select(s => s.Value)
                .FirstOrDefault();
        }

        private static void AddOrInsert<T>(IDictionary<int, List<T>> dictionary, T value, int index)
        {
            if (!dictionary.ContainsKey(index))
            {
                var slotAttachmentsList = new List<T> { value };
                dictionary.Add(index, slotAttachmentsList);
            }
            else
            {
                dictionary[index].Add(value);
            }
        }
    }
}
