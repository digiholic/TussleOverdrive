using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class XMLLoader : MonoBehaviour {
    public string xml_file_path = "";
    
	// Use this for initialization
	void Start () {
        
        XmlDocument data_xml = new XmlDocument();
        data_xml.Load(xml_file_path);

        Debug.Log(data_xml.SelectSingleNode("//fighter/stats/weight").Name); //Cool!
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
