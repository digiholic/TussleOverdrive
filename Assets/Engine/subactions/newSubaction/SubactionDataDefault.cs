using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SubactionDataDefault", menuName = "Subactions")]
public class SubactionDataDefault : ScriptableObject
{
    public string SubactionName;
    public SubactionType subType;
    public SubVarDict arguments;
    public List<string> scriptArgNames;
    [TextArea]
    public string Description;

    /// <summary>
    /// We can't just create a subaction with our argument dict, since dicts are passed by reference.
    /// First we have to clone the dictionary so it doesn't modify our Default object.
    /// </summary>
    /// <returns></returns>
    public SubactionData CreateSubactionData()
    {
        SubVarDict copyArguments = new SubVarDict();
        foreach (KeyValuePair<string,SubactionVarData> dataPair in arguments){
            copyArguments[dataPair.Key] = dataPair.Value.Copy();
        }
        return new SubactionData(SubactionName, subType, copyArguments);
    }

    [ExecuteInEditMode]
    void OnValidate()
    {
        SubactionDataDocumentationCreator.generateHtml();
    }

    public static SubactionData GetByName(string name)
    {
        return Resources.Load<SubactionDataDefault>(name).CreateSubactionData();
    }
}

/// <summary>
/// This derivation is necessary for the Serializable Dictionary to drawn in inspector
/// </summary>
[System.Serializable]
public class SubVarDict : SerializableDictionary<string, SubactionVarData>
{

    public List<SubactionVarData> GetItems()
    {
        //Since this.Values is a weird list type that I can't work with, we have to do this stupid thing
        List<SubactionVarData> retData = new List<SubactionVarData>();
        foreach (KeyValuePair<string, SubactionVarData> data in this)
        {
            retData.Add(data.Value);
        }
        return retData;
    }
}