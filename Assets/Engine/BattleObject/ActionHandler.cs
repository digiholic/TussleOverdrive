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
    [SerializeField]
    private bool isInBuilder;

    // Use this for initialization
    void Start () {
        //_current_action = new NeutralAction();
        _current_action = new GameAction();
        _current_action.SetDynamicAction(actions_file.Get("NeutralAction"));
        _current_action.SetUp(getBattleObject());
        if (isInBuilder) _current_action.setIsInBuilder(true);
    }

    public void OnFighterInfoReady(FighterInfo fInfo)
    {
        fighter_info = fInfo;
        actions_file = fighter_info.action_file;
        //_current_action = new NeutralAction();
        _current_action = new GameAction();
        _current_action.SetDynamicAction(actions_file.Get("NeutralAction"));
        if (isInBuilder) _current_action.setIsInBuilder(true);
        _current_action.SetUp(getBattleObject());
    }

    public override void ManualUpdate () {
        if (!isInBuilder) _current_action.stateTransitions();
        _current_action.current_frame++;
        _current_action.Update();
    }

    public void DoAction(string _actionName)
    {
        GameAction old_action = _current_action;
        _current_action = LoadAction(_actionName);
        _current_action.SetDynamicAction(actions_file.Get(_actionName));
        if (isInBuilder) _current_action.setIsInBuilder(true);
        old_action.TearDown(_current_action);
        _current_action.SetUp(getBattleObject());
    }

    public void DoAction(DynamicAction act)
    {
        //Debug.Log("GameAction: "+_actionName);
        GameAction old_action = _current_action;
        _current_action = LoadAction(act.name);
        _current_action.SetDynamicAction(act);
        if (isInBuilder) _current_action.setIsInBuilder(true);
        if (old_action != null)
            old_action.TearDown(_current_action);
        _current_action.SetUp(getBattleObject());
    }

    public bool ActionIsOfType(System.Type actionType) {
            bool ret = CurrentAction.GetType() == actionType;
            return ret;
    }

    public static GameAction LoadAction(string _name)
    {
        switch (_name)
        {
            default:
                //Debug.LogWarning("Null action: " + _name);
                return new GameAction();
        }
    }
}
