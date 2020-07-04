using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VarDataRow : MonoBehaviour
{
    [SerializeField] private SubactionVarData varData;

    [SerializeField] private int varType;
    [SerializeField] private int varSource;
    [SerializeField] private string varValue;

    public int VarType { get => varType; set
        {
            varType = value;
            varData.type = (SubactionVarType)value;
        }
    }

    public string VarName { get => varData.name; }

    public int VarSource { get => varSource; set
        {
            varSource = value;
            varData.source = (SubactionSource)value;
        }
    }
    public string VarValue { get => varValue; set
        {
            varValue = value;
            varData.data = value;
        }
    }

    public void setVarData(SubactionVarData data)
    {
        varData = data;
        varType = (int) data.type;
        varSource = (int)data.source;
        varValue = data.data;
    }
}
