using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubactionData {
    public string SubactionName;
    public SubactionType subType;
    public SubVarDict arguments;

    public SubactionData(string name, SubactionType subT, SubVarDict args)
    {
        SubactionName = name;
        subType = subT;
        arguments = args;
    }

    public void SetArgument(string key, object value)
    {
        if (arguments.ContainsKey(key))
        {
            arguments[key].data = value.ToString();
        }
    }
}
