using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionLoader : MonoBehaviour {

    public GameAction LoadAction(string _name)
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
            
            //Attacks
            case "NeutralAttack": return ScriptableObject.CreateInstance<BaseAttack>();
            case "ForwardAttack": return ScriptableObject.CreateInstance<BaseAttack>();
            case "ForwardSmash": return ScriptableObject.CreateInstance<BaseAttack>();
            case "UpAttack": return ScriptableObject.CreateInstance<BaseAttack>();
            case "UpSmash": return ScriptableObject.CreateInstance<BaseAttack>();
            case "DownAttack": return ScriptableObject.CreateInstance<BaseAttack>();
            case "DownSmash": return ScriptableObject.CreateInstance<BaseAttack>();
            case "NeutralAir": return ScriptableObject.CreateInstance<AirAttack>();
            case "ForwardAir": return ScriptableObject.CreateInstance<AirAttack>();
            case "BackAir": return ScriptableObject.CreateInstance<AirAttack>();
            case "UpAir": return ScriptableObject.CreateInstance<AirAttack>();
            case "DownAir": return ScriptableObject.CreateInstance<AirAttack>();
            default: return ScriptableObject.CreateInstance<GameAction>();
        }
    }
}
