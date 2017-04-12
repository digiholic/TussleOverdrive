using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : BattleComponent {
    private GameAction _current_action;
    public GameAction CurrentAction { get { return _current_action; } }
    public ActionFile actions_file_json = new ActionFile();
    private DynamicAction current_dynamic_action;
    private actionLoader action_loader;

    // Use this for initialization
    void Start () {
        action_loader = GetComponent<actionLoader>();

        _current_action = ScriptableObject.CreateInstance<NeutralAction>();
        current_dynamic_action = actions_file_json.Get("NeutralAction");
        current_dynamic_action.StartAnim(_current_action); //Sets the animation state in the current action
        _current_action.SetUp(battleObject, current_dynamic_action);
        current_dynamic_action.ExecuteGroup("SetUp", battleObject, _current_action);
    }

    // Update is called once per frame
    void Update () {
        _current_action.stateTransitions();
        current_dynamic_action.ExecuteGroup("StateTransitions", battleObject, _current_action);
        current_dynamic_action.ExecuteGroup("BeforeFrame", battleObject, _current_action);
        _current_action.Update();
        current_dynamic_action.ExecuteFrame(battleObject, _current_action);
        _current_action.LateUpdate();
        current_dynamic_action.ExecuteGroup("AfterFrame", battleObject, _current_action);
    }

    public void DoAction(string _actionName)
    {
        //Debug.Log("GameAction: "+_actionName);
        GameAction old_action = _current_action;
        _current_action = action_loader.LoadAction(_actionName);
        current_dynamic_action.ExecuteGroup("TearDown", battleObject, old_action);
        old_action.TearDown(_current_action);
        Destroy(old_action);
        current_dynamic_action = actions_file_json.Get(_actionName);
        current_dynamic_action.StartAnim(_current_action); //Sets the animation state in the current action
        _current_action.SetUp(battleObject, current_dynamic_action);
        current_dynamic_action.ExecuteGroup("SetUp", battleObject, _current_action);
    }
}
