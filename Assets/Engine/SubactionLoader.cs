using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionLoader : ScriptableObject {
    
    public static void executeSubaction(string subact, AbstractFighter actor, GameAction action)
    {
        string[] args = subact.Split(' ');
        
        switch (args[0])
        {
            // ====== CONTROL SUBACTIONS ======\\
            case "doAction":
                /* doAction actionName:string
                 *      Switches the fighter's action to actionName
                 */
                actor.doAction(args[1]);
                break;
            case "doTransition":
                /* doTransition transitionState:string
                 * 	    Executes the named helper StateTransition
                 */
                StateTransitions.LoadTransitionState(args[1], actor);
                break;
            case "setFrame":
                /* setFrame frameNumber:int
                 *      Sets the current frame to the given number
                 */
                action.current_frame = int.Parse(args[1]);
                break;
            case "changeFrame":
                /* changeFrame frameNumber:int|1
                 *      Changes the action frame by the specified amount.
                 */
                action.current_frame += int.Parse(args[1]);
                break;
            case "setVar":
                /* setVar source:string name:string type:string value:dynamic relative:bool|false
                 *      Sets the variable from GameAction, Fighter, Global with the given name to the given value and type.
                 *      If relative is set and type is something that can be relative, such as integer, it will increment
                 *      the variable instead of changing it
                 */
                 //TODO
                break;
            case "ifVar":
                /* ifVar source:string name:string compare:string|== value:dynamic|true
                 *      Sets the action condition to the result of the logical equation compare(source|name, value)
                 */
                //TODO
                break;
            case "else":
                /* else
                 *      inverts the current action condition
                 */
                //TODO
                break;
            case "endif":
                /* endif
                 *      unsets the current action condition
                 */
                //TODO
                break;
            
            // ====== CONTROL SUBACTIONS ======\\
            case "changeSpeed":
                /* changeSpeed x:float|_ y:float|_ xpref:float|_ ypref:float|_ relative:bool|false
                 *      changes the xSpeed, ySpeed, xPreffered, yPreferred speeds. If set to null, value will remain the same
                 */
                if (args[1] != "_")
                    actor._xSpeed = float.Parse(args[1]);
                if (args[2] != "_")
                    actor._ySpeed = float.Parse(args[2]);
                if (args[3] != "_")
                    actor._xPreferred = float.Parse(args[3]);
                if (args[4] != "_")
                    actor._yPreferred = float.Parse(args[4]);
                break;
            case "shiftPosition":
                /* shiftPosition x:float|0 y:float|0 relative:bool|true
                 *      Displaces the fighter by a certain amount in either direction
                 */
                //TODO
                break;
            // ====== CONTROL SUBACTIONS ======\\
            case "changeAnim":
                /* changeAnim animName:string
                 *      Changes to the specified animation.
                 */
                actor.ChangeSprite(args[1]);
                break;
            case "changeSpriteSubimage":
                /* changeSpriteSubimage index:int
                 *      SPRITE MODE ONLY
                 *      Changes to the sprite subimage of the current animation with the given index
                 */
                actor.ChangeSubimage(int.Parse(args[1]));
                break;
            case "flipFighter":
                /* flipFighter
                 *      Flips the fighter horizontally, so they are facing the other direction
                 */
                actor.flip();
                break;
            case "rotateFighter":
                /* rotateFighter deg:int
                 *      Rotates the fighter by the given degrees
                 */
                //TODO
                break;
            case "unrotateFighter":
                /* unrotateFighter
                 *      Sets the fighter back to upright, no matter how many times it has been rotated
                 */
                //TODO
                break;
            case "shiftSprite":
                /* shiftSprite x:float y:float
                 *      Shifts the sprite by the given X and Y without moving the fighter
                 */
                //TODO
                break;
            default:
                //Debug.LogWarning("Could not load subaction " + args[0]);
                break;

        }
    }
}

[System.Serializable]
public class ActionFile
{
    public List<DynamicAction> actions = new List<DynamicAction>();

    public void Add(DynamicAction newAction) //Overwrites an action with the given name if it exists. Otherwise, adds it
    {
        actions.RemoveAll(s => s.name == newAction.name); //Removes all objects that have the same name as the new action
        actions.Add(newAction);
    }

    public DynamicAction Get(string name)
    {
        foreach (DynamicAction action in actions)
        {
            if (action.name == name)
                return action;
        }
        //Debug.LogWarning("Could not find action " + name + " in ActionFile");
        return new DynamicAction("Null");
    }
}

[System.Serializable]
public class DynamicAction
{
    public string name;
    public int length;
    public string sprite;
    public int sprite_rate;
    public bool loop;

    public ActionGroup set_up_actions = new ActionGroup();
    public ActionGroup state_transition_actions = new ActionGroup();
    public ActionGroup actions_before_frame = new ActionGroup();
    public ActionGroup actions_after_frame = new ActionGroup();
    public ActionGroup actions_at_last_frame = new ActionGroup();
    public ActionGroup tear_down_actions = new ActionGroup();
    public List<ActionGroup> actions_at_frame = new List<ActionGroup>();

    private Dictionary<int, ActionGroup> actions_at_frame_dict = new Dictionary<int, ActionGroup>();

    public DynamicAction(string _name, int _length=1, string _sprite="idle", int _sprite_rate=1,bool _loop=false)
    {
        name = _name;
        length = _length;
        sprite = _sprite;
        sprite_rate = _sprite_rate;
        loop = _loop;
    }

    public void BuildDict()
    {
        foreach (ActionGroup group in actions_at_frame)
        {
            actions_at_frame_dict[int.Parse(group.frames)] = group;
        }
    }

    public void ExecuteGroup(string group, AbstractFighter actor, GameAction action)
    {
        switch (group)
        {
            case "SetUp":
                foreach (string subaction in set_up_actions.subactions)
                    SubactionLoader.executeSubaction(subaction, actor, action);
                break;
            case "StateTransitions":
                foreach (string subaction in state_transition_actions.subactions)
                    SubactionLoader.executeSubaction(subaction, actor, action);
                break;
            case "BeforeFrame":
                foreach (string subaction in actions_before_frame.subactions)
                    SubactionLoader.executeSubaction(subaction, actor, action);
                break;
            case "AfterFrame":
                foreach (string subaction in actions_after_frame.subactions)
                    SubactionLoader.executeSubaction(subaction, actor, action);
                break;
            case "LastFrame":
                foreach (string subaction in actions_at_last_frame.subactions)
                    SubactionLoader.executeSubaction(subaction, actor, action);
                break;
            case "TearDown":
                foreach (string subaction in tear_down_actions.subactions)
                    SubactionLoader.executeSubaction(subaction, actor, action);
                break;
        }
    }

    public void ExecuteFrame(AbstractFighter actor, GameAction action)
    {
        //TODO parse the action frames field to see if the frame number falls into range, function, etc.
        //  Like if the frames field is 0-2, execute subactions for frames 0,1,2

        int frame = int.Parse(action.current_frame.ToString());
        foreach (string subaction in actions_at_frame_dict[frame].subactions)
        {
            SubactionLoader.executeSubaction(subaction, actor, action);
        }
    }
}

[System.Serializable]
public class ActionGroup
{
    public string frames; //Used only for actions_at_frame, parses the string to see if the current frame is in the allowed list
    public List<string> subactions = new List<string>();
}