using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class VarData
{
    public string name;
    public string value;
    public VarType type;

    public VarData(string name, string value, VarType type)
    {
        this.name = name;
        this.value = value;
        this.type = type;
    }

    public VarData Clone(){
        return new VarData(name,value,type);
    }
}

[Serializable]
public enum VarType
{
    FLOAT,
    INT,
    BOOL,
    STRING
}
