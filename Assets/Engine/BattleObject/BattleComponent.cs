using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleComponent : MonoBehaviour {
    public BattleObject battleObject;

	// Use this for initialization
	void Awake () {
        battleObject = GetComponent<BattleObject>();
	}

    public virtual void ManualUpdate()
    {

    }

    public void SetVar(string var_name, object obj)
    {
        battleObject.SetVar(var_name, obj);
    }

    public bool HasVar(string var_name)
    {
        return battleObject.HasVar(var_name);
    }

    /// <summary>
    /// Gets the variable with the given name from the variables list
    /// </summary>
    /// <param name="var_name">The name of the variable to pull</param>
    /// <returns>The variable from the dict as an object</returns>
    public object GetVar(string var_name)
    {
        return battleObject.GetVar(var_name);
    }

    public int GetIntVar(string var_name)
    {
        return battleObject.GetIntVar(var_name);
    }

    public float GetFloatVar(string var_name)
    {
        return battleObject.GetFloatVar(var_name);
    }

    public bool GetBoolVar(string var_name)
    {
        return battleObject.GetBoolVar(var_name);
    }

    public string GetStringVar(string var_name)
    {
        return battleObject.GetStringVar(var_name);
    }
}
