using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class ActionHandler : BattleComponent {
    private GameAction _current_action;
    public GameAction CurrentAction { get { return _current_action; } }

    public ActionFile actions_file = new ActionFile();

    private FighterInfo fighter_info;

    // Use this for initialization
    void Start () {
        fighter_info = GetComponent<FighterInfoLoader>().GetFighterInfo();
        actions_file = fighter_info.action_file;
        _current_action = new NeutralAction();
        _current_action.SetDynamicAction(actions_file.Get("NeutralAction"));
        _current_action.SetUp(battleObject);
    }

    // Update is called once per frame
    public override void ManualUpdate () {
        _current_action.stateTransitions();
        _current_action.Update();
        _current_action.LateUpdate();
    }

    public void DoAction(string _actionName)
    {
        //Debug.Log("GameAction: "+_actionName);
        GameAction old_action = _current_action;
        _current_action = LoadAction(_actionName);
        _current_action.SetDynamicAction(actions_file.Get(_actionName));
        old_action.TearDown(_current_action);
        _current_action.SetUp(battleObject);
    }

    public static GameAction LoadAction(string _name)
    {
        switch (_name)
        {
            case "NeutralAction": return new NeutralAction();
            case "Fall": return new Fall();
            case "Jump": return new Jump();
            case "AirJump": return new AirJump();
            case "Crouch": return new Crouch();
            case "CrouchGetup": return new CrouchGetup();
            case "Move": return new Move();
            case "StandingPivot": return new StandingPivot();
            case "Stop": return new Stop();
            case "Pivot": return new Pivot();
            case "Land": return new Land();
            case "Dash": return new Dash();
            case "LedgeGrab": return new LedgeGrab();
            case "HitStun": return new HitStun();

            //Attacks
            case "NeutralAttack": return new BaseAttack();
            case "ForwardAttack": return new BaseAttack();
            case "ForwardSmash": return new BaseAttack();
            case "UpAttack": return new BaseAttack();
            case "UpSmash": return new BaseAttack();
            case "DownAttack": return new BaseAttack();
            case "DownSmash": return new BaseAttack();
            case "NeutralAir": return new AirAttack();
            case "ForwardAir": return new AirAttack();
            case "BackAir": return new AirAttack();
            case "UpAir": return new AirAttack();
            case "DownAir": return new AirAttack();
            case "NeutralSpecial": return new BaseAttack();
            case "ForwardSpecial": return new BaseAttack();
            case "UpSpecial": return new BaseAttack();
            case "DownSpecial": return new BaseAttack();

            default:
                Debug.LogWarning("Null action: " + _name);
                return new GameAction();
        }
    }
}
