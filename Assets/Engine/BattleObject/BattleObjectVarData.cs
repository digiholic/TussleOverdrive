using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleObjectVarData
{
    public string varName;
    public object varData;

    public BattleObjectVarData(string name, object data)
    {
        varName = name;
        varData = data;
    }

    public object GetData()
    {
        return varData;
    }

    public float GetFloatData()
    {
        //Debug.Log("Getting Float Data: " + varName + " - " + varData);
        if (varData is string)
            return float.Parse((string)varData);
        else
            return (float)varData;
    }

    public int GetIntData()
    {
        if (varData is string)
            return int.Parse((string)varData);
        else
            return (int)varData;
    }

    public bool GetBoolData()
    {
        if (varData is string)
            return bool.Parse((string)varData);
        else
            return (bool)varData;
    }

    public string GetStringData()
    {
        return varData.ToString();
    }

    public void SetData(object data)
    {
        varData = data;
    }
}

