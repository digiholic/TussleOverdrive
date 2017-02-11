using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class FighterModuleLoader : MonoBehaviour {
    public GameObject fighterButtonPrefab;

    private DirectoryInfo fightersDirectory = new DirectoryInfo("Assets/Fighters");
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
            string combinedPath = Path.Combine(fighterDir.FullName, "fighter.xml");
            if (File.Exists(combinedPath))
            {
                Debug.Log(combinedPath);
                GameObject fighterName = Instantiate(fighterButtonPrefab) as GameObject;
                RectTransform fighterNameTransform = fighterName.GetComponent<RectTransform>();
                fighterNameTransform.SetParent(gameObject.transform,false);
                fighterNameTransform.Translate(new Vector3(0.0f, lastYPos, 0.0f));
                fighterName.GetComponent<Text>().text = fighterDir.Name;
            }
        }
    }
}
