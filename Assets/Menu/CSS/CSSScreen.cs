using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSSScreen : MonoBehaviour {
    public CSSPanel panelPrefab;

	// Use this for initialization
	void Start () {
        DirectoryInfo topdir = new DirectoryInfo("Assets/Resources/Fighters");
        DirectoryInfo[] fighterdirs = topdir.GetDirectories();

        int count = 0;
        foreach (DirectoryInfo fighterdir in fighterdirs)
        {
            if (fighterdir.GetFiles("fighter.xml").Length > 0)
            {
                Debug.Log("Fighters/" + fighterdir.Name + "/");
                CSSPanel new_panel = Instantiate<CSSPanel>(panelPrefab,transform,false);
                Vector3 pos = new_panel.transform.localPosition;
                pos.x += 128 * count;
                new_panel.transform.localPosition = pos;
                new_panel.SendMessage("ChangeXML", "Fighters/" + fighterdir.Name + "/");
                count++;
            }
                
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
