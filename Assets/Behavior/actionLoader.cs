using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionLoader : MonoBehaviour {

    public Action LoadAction(string _name)
    {
        switch (_name)
        {
            case "NeutralAction": return ScriptableObject.CreateInstance<NeutralAction>();
            case "Fall": return ScriptableObject.CreateInstance<Fall>();
            case "Jump": return ScriptableObject.CreateInstance<Jump>();
            case "AirJump": return ScriptableObject.CreateInstance<AirJump>();
            case "Crouch": return ScriptableObject.CreateInstance<Crouch>();
            case "CrouchGetup": return ScriptableObject.CreateInstance<CrouchGetup>();
            case "Move": return ScriptableObject.CreateInstance<Move>();
            case "Stop": return ScriptableObject.CreateInstance<Stop>();
            case "Land": return ScriptableObject.CreateInstance<Land>();
            case "Dash": return ScriptableObject.CreateInstance<Dash>();
            default: return ScriptableObject.CreateInstance<Action>();
        }
    }
}
