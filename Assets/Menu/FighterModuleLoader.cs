using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FighterModuleLoader : MonoBehaviour {
    public PortraitRig portraitRig;

    private DirectoryInfo fightersDirectory = new DirectoryInfo("Assets/Resources/Fighters");
    private float lastYPos = -0f;

	// Use this for initialization
	void Start () {
        LoadFighterList();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadFighterList()
    {
        DirectoryInfo[] individualFighters = fightersDirectory.GetDirectories();
        foreach (DirectoryInfo fighterDir in individualFighters)
        {
            string combinedPath = Path.Combine(fighterDir.FullName, "fighter_info.json");
            if (File.Exists(combinedPath))
            {
                string json = File.ReadAllText(combinedPath);
                FighterInfo info = JsonUtility.FromJson<FighterInfo>(json);
                info.LoadDirectory(fighterDir.Name);
                portraitRig.AddPanel(info);
            }
        }
    }
}
