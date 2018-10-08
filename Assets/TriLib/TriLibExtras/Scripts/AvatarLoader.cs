using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TriLib.Extras
{
    /// <summary>
    /// Represents an avatar loader behaviour. The main class used to load avatars.
    /// </summary>
    public class AvatarLoader : MonoBehaviour
    {
        /// <summary>
        /// Current Avatar reference.
        /// </summary>
        public GameObject CurrentAvatar;

        /// <summary>
        /// AnimationController that will be assigned to the loaded object.
        /// </summary>
        public RuntimeAnimatorController RuntimeAnimatorController;

        /// <summary>
        /// Amount by which the arm's length is allowed to stretch when using IK.
        /// </summary>
        public float ArmStretch = 0.05f;

        /// <summary>
        /// Modification to the minimum distance between the feet of a humanoid model.
        /// </summary>
        public float FeetSpacing = 0f;

        /// <summary>
        /// True for any human that has a translation Degree of Freedom (DoF). It is set to false by default.
        /// </summary>
        public bool HasTranslationDof = false;

        /// <summary>
        /// Amount by which the leg's length is allowed to stretch when using IK.
        /// </summary>
        public float LegStretch = 0.5f;

        /// <summary>
        /// Defines how the lower arm's roll/twisting is distributed between the elbow and wrist joints.
        /// </summary>
        public float LowerArmTwist = 0.5f;

        /// <summary>
        /// Defines how the lower leg's roll/twisting is distributed between the knee and ankle.
        /// </summary>
        public float LowerLegTwist = 0.5f;

        /// <summary>
        /// Defines how the lower arm's roll/twisting is distributed between the shoulder and elbow joints.
        /// </summary>
        public float UpperArmTwist = 0.5f;

        /// <summary>
        /// Defines how the upper leg's roll/twisting is distributed between the thigh and knee joints.
        /// </summary>
        public float UpperLegTwist = 0.5f;

        /// <summary>
        /// Object loading scale.
        /// </summary>
        public float Scale = 0.01f;

        /// <summary>
        /// Offset applied to CapsuleCollider height.
        /// </summary>
        public float HeightOffset = 0.01f;

        /// <summary>
        /// Use this field to define custom Human Bone to Unity Bone relationship, you can follow the tempĺates bellow.
        /// </summary>
        public BoneRelationshipList CustomBoneNames;

        /// <summary>
        /// 3Ds Max Biped files to Unity human bone relationship.
        /// </summary>
        private static readonly BoneRelationshipList BipedBoneNames = new BoneRelationshipList {
            {"Head", "Head", false},
            {"Neck", "Neck", true},
            {"Chest", "Spine3", true},
            {"UpperChest", "Spine1", true},
            {"Spine", "Spine", false},
            {"Hips", "Bip01", false},

            {"LeftShoulder", "L Clavicle", true},
            {"LeftUpperArm", "L UpperArm", false},
            {"LeftLowerArm", "L Forearm", false},
            {"LeftHand", "L Hand", false},

            {"RightShoulder", "R Clavicle", true},
            {"RightUpperArm", "R UpperArm", false},
            {"RightLowerArm", "R Forearm", false},
            {"RightHand", "R Hand", false},

            {"LeftUpperLeg", "L Thigh", false},
            {"LeftLowerLeg", "L Calf", false},
            {"LeftFoot", "L Foot", false},
            {"LeftToes", "L Toe0", true},

            {"RightUpperLeg", "R Thigh", false},
            {"RightLowerLeg", "R Calf", false},
            {"RightFoot", "R Foot", false},
            {"RightToes", "R Toe0", true},

            {"Left Thumb Proximal", "L Finger0", true},
            {"Left Thumb Intermediate", "L Finger01", true},
            {"Left Thumb Distal", "L Finger02", true},

            {"Left Index Proximal", "L Finger1", true},
            {"Left Index Intermediate", "L Finger11", true},
            {"Left Index Distal", "L Finger12", true},

            {"Left Middle Proximal", "L Finger2", true},
            {"Left Middle Intermediate", "L Finger21", true},
            {"Left Middle Distal", "L Finger22", true},

            {"Left Ring Proximal", "L Finger3", true},
            {"Left Ring Intermediate", "L Finger31", true},
            {"Left Ring Distal", "L Finger32", true},

            {"Left Little Proximal", "L Finger4", true},
            {"Left Little Intermediate", "L Finger41", true},
            {"Left Little Distal", "L Finger42", true},

            {"Right Thumb Proximal", "R Finger0", true},
            {"Right Thumb Intermediate", "R Finger01", true},
            {"Right Thumb Distal", "R Finger02", true},

            {"Right Index Proximal", "R Finger1", true},
            {"Right Index Intermediate", "R Finger11", true},
            {"Right Index Distal", "R Finger12", true},

            {"Right Middle Proximal", "R Finger2", true},
            {"Right Middle Intermediate", "R Finger21", true},
            {"Right Middle Distal", "R Finger22", true},

            {"Right Ring Proximal", "R Finger3", true},
            {"Right Ring Intermediate", "R Finger31", true},
            {"Right Ring Distal", "R Finger32", true},

            {"Right Little Proximal", "R Finger4", true},
            {"Right Little Intermediate", "R Finger41", true},
            {"Right Little Distal", "R Finger42", true}
        };

        /// <summary>
        /// Mixamo files to Unity human bone relationship.
        /// </summary>
        private static readonly BoneRelationshipList MixamoBoneNames = new BoneRelationshipList {
            {"Head", "Head", false},
            {"Neck", "Neck", true},
            {"Chest", "Spine1", true},
            {"UpperChest", "Spine2", true},
            {"Spine", "Spine", false},
            {"Hips", "Hips", false},

            {"LeftShoulder", "LeftShoulder", true},
            {"LeftUpperArm", "LeftArm", false},
            {"LeftLowerArm", "LeftForeArm", false},
            {"LeftHand", "LeftHand", false},

            {"RightShoulder", "RightShoulder", true},
            {"RightUpperArm", "RightArm", false},
            {"RightLowerArm", "RightForeArm", false},
            {"RightHand", "RightHand", false},

            {"LeftUpperLeg", "LeftUpLeg", false},
            {"LeftLowerLeg", "LeftLeg", false},
            {"LeftFoot", "LeftFoot", false},
            {"LeftToes", "LeftToeBase", true},

            {"RightUpperLeg", "RightUpLeg", false},
            {"RightLowerLeg", "RightLeg", false},
            {"RightFoot", "RightFoot", false},
            {"RightToes", "RightToeBase", true},

            {"Left Thumb Proximal", "LeftHandThumb1", true},
            {"Left Thumb Intermediate", "LeftHandThumb2", true},
            {"Left Thumb Distal", "LeftHandThumb3", true},

            {"Left Index Proximal", "LeftHandIndex1", true},
            {"Left Index Intermediate", "LeftHandIndex2", true},
            {"Left Index Distal", "LeftHandIndex3", true},

            {"Left Middle Proximal", "LeftHandMiddle1", true},
            {"Left Middle Intermediate", "LeftHandMiddle2", true},
            {"Left Middle Distal", "LeftHandMiddle3", true},

            {"Left Ring Proximal", "LeftHandRing1", true},
            {"Left Ring Intermediate", "LeftHandRing2", true},
            {"Left Ring Distal", "LeftHandRing3", true},

            {"Left Little Proximal", "LeftHandPinky1", true},
            {"Left Little Intermediate", "LeftHandPinky2", true},
            {"Left Little Distal", "LeftHandPinky3", true},

            {"Right Thumb Proximal", "RightHandThumb1", true},
            {"Right Thumb Intermediate", "RightHandThumb2", true},
            {"Right Thumb Distal", "RightHandThumb3", true},

            {"Right Index Proximal", "RightHandIndex1", true},
            {"Right Index Intermediate", "RightHandIndex2", true},
            {"Right Index Distal", "RightHandIndex3", true},

            {"Right Middle Proximal", "RightHandMiddle1", true},
            {"Right Middle Intermediate", "RightHandMiddle2", true},
            {"Right Middle Distal", "RightHandMiddle3", true},

            {"Right Ring Proximal", "RightHandRing1", true},
            {"Right Ring Intermediate", "RightHandRing2", true},
            {"Right Ring Distal", "RightHandRing3", true},

            {"Right Little Proximal", "RightHandPinky1", true},
            {"Right Little Intermediate", "RightHandPinky2", true},
            {"Right Little Distal", "RightHandPinky3", true}
        };

        /// <summary>
        /// Sample loading options.
        /// </summary>
        private AssetLoaderOptions _loaderOptions;

        /// <summary>
        /// Setups the Avatar Loader.
        /// </summary>
        protected void Start()
        {
            _loaderOptions = AssetLoaderOptions.CreateInstance();
            _loaderOptions.UseLegacyAnimations = false;
            _loaderOptions.DontGenerateAvatar = true;
            _loaderOptions.AnimatorController = RuntimeAnimatorController;
        }

        /// <summary>
        /// Loads the avatar from specified filename.
        /// </summary>
        /// <param name="data">Avatar file data.</param>
        /// <param name="extension">File extension.</param>
        /// <param name="templateAvatar">Template <see cref="UnityEngine.GameObject"/>.</param>  
        /// <returns><c>true</c>, if avatar was loaded, <c>false</c> otherwise.</returns>
        public bool LoadAvatarFromMemory(byte[] data, string extension, GameObject templateAvatar)
        {
            GameObject loadedObject;
            if (CurrentAvatar != null)
            {
                Destroy(CurrentAvatar);
            }
            try
            {
                using (var assetLoader = new AssetLoader())
                {
                    loadedObject = assetLoader.LoadFromMemoryWithTextures(data, extension, _loaderOptions, templateAvatar);
                }
            }
#if TRILIB_OUTPUT_MESSAGES || ASSIMP_OUTPUT_MESSAGES
            catch (Exception exception)
            {
                Debug.LogError(exception.ToString());
                return false;
            }
#else
            catch
            {
                if (CurrentAvatar != null)
                {
                    Destroy(CurrentAvatar);
                }
                return false;
            }
#endif
            if (loadedObject != null)
            {
                if (templateAvatar != null)
                {
                    loadedObject.transform.parent = templateAvatar.transform;
                    CurrentAvatar = templateAvatar;
                }
                else
                {
                    CurrentAvatar = loadedObject;
                }
                CurrentAvatar.transform.localScale = Vector3.one * Scale;
                CurrentAvatar.tag = "Player";
                SetupCapsuleCollider();
                return BuildAvatar();
            }
            return false;
        }

        /// <summary>
        /// Loads the avatar from specified filename.
        /// </summary>
        /// <param name="filename">Avatar filename.</param>
        /// <param name="templateAvatar">Template <see cref="UnityEngine.GameObject"/>.</param>  
        /// <returns><c>true</c>, if avatar was loaded, <c>false</c> otherwise.</returns>
        public bool LoadAvatar(string filename, GameObject templateAvatar)
        {

            GameObject loadedObject;
            if (CurrentAvatar != null)
            {
                Destroy(CurrentAvatar);
            }
            try
            {
                using (var assetLoader = new AssetLoader())
                {
                    loadedObject = assetLoader.LoadFromFile(filename, _loaderOptions, templateAvatar);
                }
            }
#if TRILIB_OUTPUT_MESSAGES || ASSIMP_OUTPUT_MESSAGES
            catch (Exception exception)
            {
                Debug.LogError(exception.ToString());
                return false;
            }
#else
            catch
            {
                if (CurrentAvatar != null)
                {
                    Destroy(CurrentAvatar);
                }
                return false;
            }
#endif
            if (loadedObject != null)
            {
                if (templateAvatar != null)
                {
                    loadedObject.transform.parent = templateAvatar.transform;
                    CurrentAvatar = templateAvatar;
                }
                else
                {
                    CurrentAvatar = loadedObject;
                }
                CurrentAvatar.transform.localScale = Vector3.one * Scale;
                CurrentAvatar.tag = "Player";
                SetupCapsuleCollider();
                return BuildAvatar();
            }
            return false;
        }

        /// <summary>
        /// Builds the object avatar, based on pre-defined templates (Mixamo, Biped), or based on the <see cref="TriLib.Extras.AvatarLoader.CustomBoneNames"></see>, if it's not null or empty.
        /// </summary>
        /// <returns><c>true</c> if avatar was built, <c>false</c> otherwise.</returns>
        private bool BuildAvatar()
        {
            var animator = CurrentAvatar.GetComponent<Animator>();
            if (animator == null)
            {
#if TRILIB_OUTPUT_MESSAGES || ASSIMP_OUTPUT_MESSAGES
                Debug.LogError("No Animator Component found on current Avatar.");
#endif
                return false;
            }
            var skeletonBones = new List<SkeletonBone>();
            var humanBones = new List<HumanBone>();
            var boneTransforms = FindOutBoneTransforms(CurrentAvatar);
            if (boneTransforms.Count == 0)
            {
#if TRILIB_OUTPUT_MESSAGES || ASSIMP_OUTPUT_MESSAGES
                Debug.LogError("No suitable bones format found");
#endif
                return false;
            }
            foreach (var boneTransform in boneTransforms)
            {
                humanBones.Add(CreateHumanBone(boneTransform.Key, boneTransform.Value.name));
            }
            var transforms = CurrentAvatar.GetComponentsInChildren<Transform>();
            var rootTransform = transforms[1];
            skeletonBones.Add(CreateSkeletonBone(rootTransform));
            rootTransform.localEulerAngles = Vector3.zero;
            for (var i = 0; i < transforms.Length; i++)
            {
                var childTransform = transforms[i];
                var meshRenderers = childTransform.GetComponentsInChildren<MeshRenderer>();
                if (meshRenderers.Length > 0)
                {
                    continue;
                }
                var skinnedMeshRenderers = childTransform.GetComponentsInChildren<SkinnedMeshRenderer>();
                if (skinnedMeshRenderers.Length > 0)
                {
                    continue;
                }
                skeletonBones.Add(CreateSkeletonBone(childTransform));
            }
            var humanDescription = new HumanDescription();
            humanDescription.armStretch = ArmStretch;
            humanDescription.feetSpacing = FeetSpacing;
            humanDescription.hasTranslationDoF = HasTranslationDof;
            humanDescription.legStretch = LegStretch;
            humanDescription.lowerArmTwist = LowerArmTwist;
            humanDescription.lowerLegTwist = LowerLegTwist;
            humanDescription.upperArmTwist = UpperArmTwist;
            humanDescription.upperLegTwist = UpperLegTwist;
            humanDescription.skeleton = skeletonBones.ToArray();
            humanDescription.human = humanBones.ToArray();
            animator.avatar = AvatarBuilder.BuildHumanAvatar(CurrentAvatar, humanDescription);
            return true;

        }

        /// <summary>
        /// Figures out the bone hierarchy for Avatar building.
        /// </summary>
        /// <returns>The bone hierarchy.</returns>
        /// <param name="loadedObject">Previously loaded object.</param>
        private Dictionary<string, Transform> FindOutBoneTransforms(GameObject loadedObject)
        {
            var boneTransforms = new Dictionary<string, Transform>();
            var boneRelationshipLists = new List<BoneRelationshipList>();
            boneRelationshipLists.Add(BipedBoneNames);
            boneRelationshipLists.Add(MixamoBoneNames);
            if (CustomBoneNames != null)
            {
                boneRelationshipLists.Add(CustomBoneNames);
            }
            var lastBonesValid = false;
            foreach (var boneRelationshipList in boneRelationshipLists)
            {
                if (lastBonesValid)
                {
                    break;
                }
                lastBonesValid = true;
                foreach (var boneRelationship in boneRelationshipList)
                {
                    var boneTransform = loadedObject.transform.FindDeepChild(boneRelationship.BoneName, true);
                    if (boneTransform == null)
                    {
                        if (!boneRelationship.Optional)
                        {
                            boneTransforms.Clear();
                            lastBonesValid = false;
                            break;
                        }
                        continue;
                    }
                    boneTransforms.Add(boneRelationship.HumanBone, boneTransform);
                }
            }
            return boneTransforms;
        }

        /// <summary>
        /// Setups the avatar Capsule Collider to encapsulate the loaded object.
        /// </summary>
        private void SetupCapsuleCollider()
        {
            var capsuleCollider = CurrentAvatar.GetComponent<CapsuleCollider>();
            if (capsuleCollider == null)
            {
                return;
            }
            var bounds = CurrentAvatar.transform.EncapsulateBounds();
            var fraction = 1f / Scale;
            var boundExtentsX = bounds.extents.x * fraction;
            var boundExtentsY = bounds.extents.y * fraction;
            var boundExtentsZ = bounds.extents.z * fraction;
            capsuleCollider.height = (float)Math.Round(boundExtentsY * 2f, 1);
            capsuleCollider.radius = (float)Math.Round(Mathf.Sqrt(boundExtentsX * boundExtentsX + boundExtentsZ * boundExtentsZ) * 0.5f, 1);
            capsuleCollider.center = new Vector3(0f, (float)Math.Round(boundExtentsY, 1) + HeightOffset, 0f);
        }

        /// <summary>
        /// Builds a SkeletonBone.
        /// </summary>
        /// <returns>The built skeleton bone.</returns>
        /// <param name="boneTransform">The bone Transform to build the Skeleton from.</param>
        private static SkeletonBone CreateSkeletonBone(Transform boneTransform)
        {
            var skeletonBone = new SkeletonBone();
            skeletonBone.name = boneTransform.name;
            skeletonBone.position = boneTransform.localPosition;
            skeletonBone.rotation = boneTransform.localRotation;
            skeletonBone.scale = boneTransform.localScale;
            return skeletonBone;
        }

        /// <summary>
        /// Builds a Human Bone.
        /// </summary>
        /// <returns>The human bone.</returns>
        /// <param name="humanName">Human name.</param>
        /// <param name="boneName">Bone name.</param>
        private static HumanBone CreateHumanBone(string humanName, string boneName)
        {
            var humanBone = new HumanBone();
            humanBone.boneName = boneName;
            humanBone.humanName = humanName;
            humanBone.limit.useDefaultValues = true;
            return humanBone;
        }
    }

    /// <summary>
    /// Represents a human bone to Unity bone relationship.
    /// </summary>
    public class BoneRelationship
    {
        public string HumanBone; //Human Bone name.
        public string BoneName; //Unity Bone name.
        public bool Optional; //Is this bone optional?

        /// <summary>
        /// Initializes a new instance of the <see cref="TriLib.Extras.BoneRelationship"/> class.
        /// </summary>
        /// <param name="humanBone">Human bone.</param>
        /// <param name="boneName">Bone name.</param>
        /// <param name="optional">If set to <c>true</c> this bone relationship will be optional in the hierarchy.</param>
        public BoneRelationship(string humanBone, string boneName, bool optional)
        {
            HumanBone = humanBone;
            BoneName = boneName;
            Optional = optional;
        }
    }

    /// <summary>
    /// Represents a BoneRelationship list.
    /// </summary>
    [Serializable]
    public class BoneRelationshipList : IEnumerable<BoneRelationship>
    {
        private readonly List<BoneRelationship> _relationships; //Relationship list

        /// <summary>
        /// Initializes a new instance of the <see cref="TriLib.Extras.BoneRelationshipList"/> class.
        /// </summary>
        public BoneRelationshipList()
        {
            _relationships = new List<BoneRelationship>();
        }

        /// <summary>
        /// Adds a new BoneRelationship to this list.
        /// </summary>
        /// <param name="humanBone">Human bone.</param>
        /// <param name="boneName">Bone name.</param>
        /// <param name="optional">If set to <c>true</c> this bone relationship will be optional in the hierarchy.</param>
        public void Add(string humanBone, string boneName, bool optional)
        {
            _relationships.Add(new BoneRelationship(humanBone, boneName, optional));
        }

        /// <summary>
        /// Returns an enumerator that iterates through this collection.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<BoneRelationship> GetEnumerator()
        {
            return _relationships.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator for this collection.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
