using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="SubactionData",menuName="Subactions")]
public class SubactionData : ScriptableObject {
    public string SubactionName;
    public SubVarDict arguments;

    public override string ToString()
    {
        return SubactionName;
    }
}

/// <summary>
/// This derivation is necessary for the Serializable Dictionary to drawn in inspector
/// </summary>
[System.Serializable]
public class SubVarDict : SerializableDictionary<string, SubactionVarData>{ }