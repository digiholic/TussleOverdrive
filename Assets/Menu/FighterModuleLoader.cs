using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
                GameObject fighterName = Instantiate(fighterButtonPrefab) as GameObject;
                fighterName.GetComponent<OnClickOpenFighter>().xml_data = combinedPath;
                RectTransform fighterNameTransform = fighterName.GetComponent<RectTransform>();
                //Place it properly
                fighterNameTransform.SetParent(gameObject.transform,false);
                fighterNameTransform.Translate(new Vector3(0.0f, lastYPos, 0.0f));
                //Set its name
                fighterName.GetComponent<Text>().text = fighterDir.Name;
            }
        }
    }
}
