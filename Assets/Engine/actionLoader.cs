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
            case "NeutralAttack": return ScriptableObject.CreateInstance<NeutralAttack>();
            case "ForwardAttack": return ScriptableObject.CreateInstance<Ftilt>();
            case "UpAttack": return ScriptableObject.CreateInstance<Utilt>();
            case "DownAttack": return ScriptableObject.CreateInstance<Dtilt>();
            case "NeutralAir": return ScriptableObject.CreateInstance<Nair>();
            case "ForwardAir": return ScriptableObject.CreateInstance<Fair>();
            case "BackAir": return ScriptableObject.CreateInstance<Bair>();
            case "UpAir": return ScriptableObject.CreateInstance<Uair>();
            case "DownAir": return ScriptableObject.CreateInstance<Dair>();
            default: return ScriptableObject.CreateInstance<Action>();
        }
    }
}
