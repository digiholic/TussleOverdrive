using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class VarData : ScriptableObject
{
    public string name;
    public string value;
    public VarType type;
}

public enum VarType
{
    FLOAT,
    INT,
    BOOL,
    STRING
}
