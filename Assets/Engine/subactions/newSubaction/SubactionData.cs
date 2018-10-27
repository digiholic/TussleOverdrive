using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="SubactionData",menuName="Subactions")]
public class SubactionData : ScriptableObject {
    public string SubactionName;
    public SubactionType subType;
    public SubVarDict arguments;

    [TextArea]
    public string Description;

    public override string ToString()
    {
        return SubactionName;
    }

    [ExecuteInEditMode]
    void OnValidate()
    {
        SubactionDataDocumentationCreator.generateHtml();
    }
}

/// <summary>
/// This derivation is necessary for the Serializable Dictionary to drawn in inspector
/// </summary>
[System.Serializable]
public class SubVarDict : SerializableDictionary<string, SubactionVarData>{

    public List<SubactionVarData> GetItems()
    {
        //Since this.Values is a weird list type that I can't work with, we have to do this stupid thing
        List<SubactionVarData> retData = new List<SubactionVarData>();
        foreach(KeyValuePair<string,SubactionVarData> data in this)
        {
            retData.Add(data.Value);
        }
        return retData;
    }
}