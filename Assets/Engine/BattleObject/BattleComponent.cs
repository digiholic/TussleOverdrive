using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(BattleObject))]
public class BattleComponent : MonoBehaviour {
    public BattleObject battleObject;
    public string BattleComponentType;

	// Use this for initialization
	void Awake() {
        battleObject = GetComponent<BattleObject>();
	}

    public string ToJson(bool prettyPrint = false)
    {
        PrintDebug(this, 3, JsonUtility.ToJson(this, true));
        return JsonUtility.ToJson(this, prettyPrint);
    }

    public void PrintDebug(object callingObject, int debugLevel, string message)
    {
        getBattleObject().PrintDebug(callingObject, debugLevel, message);
    }

    public virtual void ManualUpdate()
    {

    }

    public void SetVar(string var_name, object obj)
    {
        getBattleObject().SetVar(var_name, obj);
    }

    public bool HasVar(string var_name)
    {
        return getBattleObject().HasVar(var_name);
    }

    /// <summary>
    /// Gets the variable with the given name from the variables list
    /// </summary>
    /// <param name="var_name">The name of the variable to pull</param>
    /// <returns>The variable from the dict as an object</returns>
    public object GetVar(string var_name)
    {
        return getBattleObject().GetVarData(var_name);
    }

    public int GetIntVar(string var_name)
    {
        return getBattleObject().GetIntVar(var_name);
    }

    public float GetFloatVar(string var_name)
    {
        return getBattleObject().GetFloatVar(var_name);
    }

    public bool GetBoolVar(string var_name)
    {
        return getBattleObject().GetBoolVar(var_name);
    }

    public string GetStringVar(string var_name)
    {
        return getBattleObject().GetStringVar(var_name);
    }

    protected BattleObject getBattleObject()
    {
        if (battleObject == null)
        {
            battleObject = GetComponent<BattleObject>();
        }
        return battleObject;
    }

}
