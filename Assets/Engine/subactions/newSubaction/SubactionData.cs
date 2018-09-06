using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubactionData : ScriptableObject {
    public string SubactionName;
    public SerializableDictionary<string, SubactionVarData> arguments;
}
