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
        else if (varData == null)
        {
            Debug.LogWarning("Attempting to get FloatData from null VarData");
            return 0.0f;
        }
        else
            return (float)varData;
    }

    public int GetIntData()
    {
        if (varData is string)
            return int.Parse((string)varData);
        else if (varData == null)
        {
            Debug.LogWarning("Attempting to get IntData from null VarData");
            return 0;
        }
        else
            return (int)varData;
    }

    public bool GetBoolData()
    {
        if (varData is string)
            return bool.Parse((string)varData);
        else if (varData == null)
        {
            Debug.LogWarning("Attempting to get BoolData from null VarData");
            return false;
        }
        else
            return (bool)varData;
    }

    public string GetStringData()
    {
        if (varData == null)
        {
            Debug.LogWarning("Attempting to get StringData from null VarData");
            return "";
        }
        else
        {
            return varData.ToString();
        }
    }

    public void SetData(object data)
    {
        varData = data;
    }
}

