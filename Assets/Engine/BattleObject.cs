using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleObject : MonoBehaviour {
    private GameAction _current_action;
    public GameAction CurrentAction { get { return _current_action; } }
    public ActionFile actions_file_json = new ActionFile();
    private DynamicAction current_dynamic_action;
    private actionLoader action_loader;

    //MEGA BIG TODO: CHANGE THIS TO NOT BE HERE. BATTLE OBJECT SHOULD WORK FOR EVERYTHING
    private AbstractFighter actor;

    // Use this for initialization
    void Start () {
        actor = GetComponent<AbstractFighter>(); //NUKE THIS
        action_loader = GetComponent<actionLoader>();

        _current_action = ScriptableObject.CreateInstance<NeutralAction>();
        current_dynamic_action = actions_file_json.Get("NeutralAction");
        current_dynamic_action.StartAnim(_current_action); //Sets the animation state in the current action
        _current_action.SetUp(actor, current_dynamic_action);
        current_dynamic_action.ExecuteGroup("SetUp", actor, _current_action);
    }
	
	// ManualUpdate is called once per frame by the calling object
	public void ManualUpdate () {
        _current_action.stateTransitions();
        current_dynamic_action.ExecuteGroup("StateTransitions", actor, _current_action);
        current_dynamic_action.ExecuteGroup("BeforeFrame", actor, _current_action);
        _current_action.Update();
        current_dynamic_action.ExecuteFrame(actor, _current_action);
        _current_action.LateUpdate();
        current_dynamic_action.ExecuteGroup("AfterFrame", actor, _current_action);
    }


    public void doAction(string _actionName)
    {
        //Debug.Log("GameAction: "+_actionName);
        GameAction old_action = _current_action;
        _current_action = action_loader.LoadAction(_actionName);
        current_dynamic_action.ExecuteGroup("TearDown", actor, old_action);
        old_action.TearDown(_current_action);
        Destroy(old_action);
        current_dynamic_action = actions_file_json.Get(_actionName);
        current_dynamic_action.StartAnim(_current_action); //Sets the animation state in the current action
        _current_action.SetUp(actor, current_dynamic_action);
        current_dynamic_action.ExecuteGroup("SetUp", actor, _current_action);
    }
}
