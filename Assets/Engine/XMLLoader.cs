using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class XMLLoader : MonoBehaviour {
    private XmlDocument data_xml;

    public string resource_path;

    void Awake()
    {
        TextAsset xml_asset = Resources.Load<TextAsset>(resource_path + "fighter");
        
        if (xml_asset != null)
            LoadXML(xml_asset.text);
        else
            throw new System.Exception("Illegal Fighter at " + resource_path);
    }

    public void LoadXML(string xml_string)
    {
        data_xml = new XmlDocument();
        data_xml.LoadXml(xml_string); 
    }

    public DataNode SelectSingleNode(string nodePath)
    {
        if (data_xml != null)
        {
            XmlNode node = data_xml.SelectSingleNode(nodePath);
            if (node != null)
                return new DataNode(node);
            else
                return null;
        }
        else
        {
            Debug.LogWarning("No XML document loaded when querying " + nodePath);
            return null;
        }
    }

    public T LoadResource<T>(string resourcePath, bool relative = true) where T : Object
    {
        if (relative)
            resourcePath = resource_path + resourcePath;
        T res_asset = Resources.Load<T>(resourcePath);
        if (res_asset == null)
        {
            Debug.LogError("Could not load resource " + resourcePath + ". Make sure not to include file extensions.");
            return null;
        }
        return res_asset;
    }

    void ChangeXML(string newXml)
    {
        resource_path = newXml;
        TextAsset xml_asset = Resources.Load<TextAsset>(resource_path + "fighter");

        if (xml_asset != null)
            LoadXML(xml_asset.text);
        else
            throw new System.Exception("Illegal Fighter at " + resource_path);
    }
}

public class DataNode
{
    private XmlNode node;
    public string Name;

    public DataNode(XmlNode _node)
    {
        node = _node;
        Name = node.Name;
    }

    public string GetString()
    {
        return node.InnerText;
    }

    public int GetInt()
    {
        return int.Parse(node.InnerText);
    }

    public float GetFloat()
    {

        return float.Parse(node.InnerText);
    }

    public bool GetBool()
    {
        return bool.Parse(node.InnerText);
    }
}