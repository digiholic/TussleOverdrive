using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class XMLLoader : MonoBehaviour {
    private XmlDocument data_xml;

    [HideInInspector]
    public Dictionary<string,DataNode> nodemap;

    void Start()
    {
        nodemap = new Dictionary<string, DataNode>();
    }

    public void LoadXML(string xml_file_path, bool overwrite = true)
    {
        if (overwrite)
            nodemap = new Dictionary<string, DataNode>();

        if (File.Exists(xml_file_path))
        {
            data_xml = new XmlDocument();
            data_xml.Load(xml_file_path);

            foreach (XmlNode node in data_xml)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    nodemap.Add(childNode.Name, new DataNode(childNode));
                }
            }
        }
        else
        {
            Debug.LogWarning("Could not find XML File "+xml_file_path);
        }
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