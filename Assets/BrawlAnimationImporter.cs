using UnityEngine;
using System;
using System.Collections.Generic;
using IniParser;
using UnityEditor;
using IniParser.Model;
using System.IO;

public class BrawlAnimationImporter : MonoBehaviour
{

    [UnityEditor.MenuItem("Brawl Animations/Import")]
    public static void ImportAniamtions()
    {
        int framerate = 60;
        int framesInClip = 0;
        string line;
        //You can put whatever file name your heart desires here
        string filePath = @"C:\Users\digiholic\Desktop\BrawlAnim\";

        string[] subdirectoryEntries = Directory.GetDirectories(filePath);
        for (int subdirectoryIndex = 0; subdirectoryIndex < subdirectoryEntries.Length; subdirectoryIndex++)
        {
            string subdirectory = subdirectoryEntries[subdirectoryIndex];
            string[] animFiles = Directory.GetFiles(subdirectory);
            string[] subdirectoryElements = subdirectory.Split('\\');
            System.IO.Directory.CreateDirectory(Application.dataPath + "/BrawlAnimations/" + subdirectoryElements[subdirectoryElements.Length - 1]);
            
            for (int fileIndex = 0; fileIndex < animFiles.Length; fileIndex++)
            {
                string[] fileString = animFiles[fileIndex].Split('\\');
                string folderName = fileString[fileString.Length - 2];
                string clipName = fileString[fileString.Length - 1];

                List<BoneAnimation> BoneAnimationList = new List<BoneAnimation>();
                System.IO.StreamReader file = new System.IO.StreamReader(animFiles[fileIndex]);

                while ((line = file.ReadLine()) != null)
                {

                    if (line.StartsWith("endTime"))
                    {
                        string[] endLine = line.Split(null);
                        string endFrame = endLine[1];
                        endFrame = endFrame.Remove(endFrame.Length - 1);
                        framesInClip = Convert.ToInt32(endFrame);
                    }
                    else if (line.StartsWith("anim") && (!line.StartsWith("animVersion") && !line.StartsWith("animData")))
                    {
                        BoneAnimation boneAnimation = new BoneAnimation();
                        string[] animationHeader = line.Split(null);
                        string animationType = animationHeader[1];
                        switch (animationType)
                        {
                            case "translate.translateX":
                                boneAnimation.transformType = "localPosition.x";
                                break;
                            case "translate.translateY":
                                boneAnimation.transformType = "localPosition.y";
                                break;
                            case "translate.translateZ":
                                boneAnimation.transformType = "localPosition.z";
                                break;
                            case "rotate.rotateX":
                                boneAnimation.transformType = "localEuler.x";
                                break;
                            case "rotate.rotateY":
                                boneAnimation.transformType = "localEuler.y";
                                break;
                            case "rotate.rotateZ":
                                boneAnimation.transformType = "localEuler.z";
                                break;
                            case "scale.scaleX":
                                boneAnimation.transformType = "localScale.x";
                                break;
                            case "scale.scaleY":
                                boneAnimation.transformType = "localScale.y";
                                break;
                            case "scale.scaleZ":
                                boneAnimation.transformType = "localScale.z";
                                break;
                        }

                        boneAnimation.boneName = animationHeader[3];
                        //Debug.Log("Bone Name: " + boneAnimation.boneName);

                        line = file.ReadLine();
                        line = file.ReadLine();
                        line = file.ReadLine();
                        line = file.ReadLine();
                        line = file.ReadLine();
                        line = file.ReadLine();
                        line = file.ReadLine();
                        line = file.ReadLine();

                        line = line.Trim();

                        while (line.Length > 0 && char.IsDigit(line[0]))
                        {
                            string[] oneKeyFrame = line.Split(null);
                            //time is a frame here
                            float frame = Convert.ToSingle(oneKeyFrame[0]);
                            frame = frame - 1;
                            float value = Convert.ToSingle(oneKeyFrame[1]);
                            //time is now the frame/ framerate 
                            //which makes it the frame in terms of seconds
                            float time = frame / framerate;

                            if (boneAnimation.transformType == "localPosition.x") { value = -value; }

                            Keyframe keyFrame = new Keyframe(time, value);

                            boneAnimation.keyFrames.Add(keyFrame);
                            boneAnimation.keyFrameFrames.Add(Convert.ToInt32(frame));
                            line = file.ReadLine();
                            line = line.Trim();
                        }
                        BoneAnimationList.Add(boneAnimation);
                    }
                }
                file.Close();

                AnimationClip clip = new AnimationClip();

                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile(filePath + "BoneDefinitions.txt");
                Dictionary<string, RotationCurveHolder> rotationDictionary = new Dictionary<string, RotationCurveHolder>();

                for (int i = 0; i < BoneAnimationList.Count; i++)
                {

                    BoneAnimation boneAnimation = BoneAnimationList[i];
                    string tempParentName = data["Parents"][boneAnimation.boneName];
                    string pathBuilder = boneAnimation.boneName;

                    while (tempParentName != "NONE")
                    {
                        pathBuilder = tempParentName + "/" + pathBuilder;
                        tempParentName = data["Parents"][tempParentName];
                    }

                    boneAnimation.bonePath = pathBuilder;

                    AnimationCurve curve = new AnimationCurve();

                    Keyframe[] keyFrames = boneAnimation.keyFrames.ToArray();
                    curve.keys = keyFrames;

                    if (boneAnimation.transformType == "localEuler.x" || (boneAnimation.transformType == "localEuler.y" || boneAnimation.transformType == "localEuler.z"))
                    {

                        float[] angleValues = new float[framesInClip];
                        for (float frame = 0; frame < framesInClip; frame++)
                        {
                            float time = (frame / framerate);

                            angleValues[Convert.ToInt32(frame)] = curve.Evaluate(time);
                        }

                        if (boneAnimation.transformType == "localEuler.x")
                        {
                            RotationCurveHolder rotationHolder = new RotationCurveHolder();
                            rotationHolder.angleValuesX = angleValues;
                            rotationDictionary.Add(boneAnimation.boneName, rotationHolder);
                        }
                        else if (boneAnimation.transformType == "localEuler.y")
                        {
                            RotationCurveHolder rotationHolder;
                            rotationDictionary.TryGetValue(boneAnimation.boneName, out rotationHolder);
                            rotationHolder.angleValuesY = angleValues;
                        }
                        else if (boneAnimation.transformType == "localEuler.z")
                        {
                            RotationCurveHolder rotationHolder;
                            rotationDictionary.TryGetValue(boneAnimation.boneName, out rotationHolder);
                            rotationHolder.angleValuesZ = angleValues;

                            Keyframe[] xCurveKeyframes = new Keyframe[framesInClip];
                            Keyframe[] yCurveKeyframes = new Keyframe[framesInClip];
                            Keyframe[] zCurveKeyframes = new Keyframe[framesInClip];

                            for (float floatFrame = 0; floatFrame < framesInClip; floatFrame++)
                            {
                                int frame = Convert.ToInt32(floatFrame);
                                float xRotation = rotationHolder.angleValuesX[frame];
                                float yRotation = rotationHolder.angleValuesY[frame];
                                float zRotation = rotationHolder.angleValuesZ[frame];
                                Vector3 rotations = new Vector3(xRotation, yRotation, zRotation);
                                Quaternion qq = MayaRotationToUnity(rotations);

                                xCurveKeyframes[frame] = new Keyframe(floatFrame / framerate, qq.eulerAngles.x, Mathf.Infinity, Mathf.Infinity);
                                yCurveKeyframes[frame] = new Keyframe(floatFrame / framerate, qq.eulerAngles.y, Mathf.Infinity, Mathf.Infinity);
                                zCurveKeyframes[frame] = new Keyframe(floatFrame / framerate, qq.eulerAngles.z, Mathf.Infinity, Mathf.Infinity);

                            }
                            AnimationCurve xCurve = new AnimationCurve();
                            AnimationCurve yCurve = new AnimationCurve();
                            AnimationCurve zCurve = new AnimationCurve();
                            xCurve.keys = xCurveKeyframes;
                            yCurve.keys = yCurveKeyframes;
                            zCurve.keys = zCurveKeyframes;
                            clip.SetCurve(boneAnimation.bonePath, typeof(Transform), "localEuler.x", xCurve);
                            clip.SetCurve(boneAnimation.bonePath, typeof(Transform), "localEuler.y", yCurve);
                            clip.SetCurve(boneAnimation.bonePath, typeof(Transform), "localEuler.z", zCurve);
                        }
                    }
                    else
                    {
                        clip.SetCurve(boneAnimation.bonePath, typeof(Transform), boneAnimation.transformType, curve);
                    }
                }
                //You can change this to whatever you want it to be here
                AssetDatabase.CreateAsset(clip, "Assets/BrawlAnimations/" + folderName + "/" + clipName);
            }
        }
    }

    private static Quaternion MayaRotationToUnity(Vector3 rotation)
    {
        Vector3 flippedRotation = new Vector3(rotation.x, -rotation.y, -rotation.z);
        Quaternion qx = Quaternion.AngleAxis(flippedRotation.x, Vector3.right);
        Quaternion qy = Quaternion.AngleAxis(flippedRotation.y, Vector3.up);
        Quaternion qz = Quaternion.AngleAxis(flippedRotation.z, Vector3.forward);
        Quaternion qq = qz * qy * qx;
        return qq;
    }

    public class BoneAnimation
    {
        public string boneName;
        public string transformType;
        public List<Keyframe> keyFrames = new List<Keyframe>();
        public List<int> keyFrameFrames = new List<int>();
        public string parentName;
        public string bonePath;

    }
    public class RotationCurveHolder
    {
        public float[] angleValuesX { get; set; }
        public float[] angleValuesY { get; set; }
        public float[] angleValuesZ { get; set; }
        public int[] keyFrameFramesX { get; set; }
        public int[] keyFrameFramesY { get; set; }
        public int[] keyFrameFramesZ { get; set; }
    }
}
