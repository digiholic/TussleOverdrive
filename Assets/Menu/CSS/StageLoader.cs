using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StageLoader : MonoBehaviour {
    public StagePortraitRig portraitRig;

    private DirectoryInfo stagesDirectory = new DirectoryInfo("Assets/Resources/Stages");
    //private float lastYPos = -0f;

    // Use this for initialization
    void Start()
    {
        LoadFighterList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadFighterList()
    {
        DirectoryInfo[] individualStages = stagesDirectory.GetDirectories();
        foreach (DirectoryInfo stageDir in individualStages)
        {
            string combinedPath = Path.Combine(stageDir.FullName, "stage_info.json");
            if (File.Exists(combinedPath))
            {
                string json = File.ReadAllText(combinedPath);
                StageInfo info = JsonUtility.FromJson<StageInfo>(json);
                info.LoadDirectory(stageDir.Name);
                portraitRig.AddPanel(info);
            }
        }
    }
}
