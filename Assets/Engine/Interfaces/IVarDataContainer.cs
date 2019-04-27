using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVarDataContainer
{
    //Initialize the variable if it's not set yet, then return it
    BattleObjectVarData GetVar(string var_name);
    void SetVar(string var_name, object obj);
    bool HasVar(string var_name);
    object GetVarData(string var_name);
}
