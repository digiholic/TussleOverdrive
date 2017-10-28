using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Subaction
{
    public string SubactionName;
    public List<SubactionVarData> arg_list;
    protected Dictionary<string, SubactionVarData> arg_dict;

    //This is used to denote if a subaction should be executed, regardless of conditional
    private bool is_control_subaction = false;
    //This is used to denote if a subaction should execute in the builder
    private bool execute_in_builder = false;

    /// <summary>
    /// Builds the dictionary of variables keyed by name for easier access.
    /// Called when the subaction is generated.
    /// </summary>
    private void BuildDict()
    {
        arg_dict = new Dictionary<string, SubactionVarData>();
        foreach (SubactionVarData data in arg_list)
        {
            arg_dict[data.name] = data;
        }
    }

    /// <summary>
    /// Executes the subaction
    /// </summary>
    /// <param name="actor">The BattleObject the subaction is being executed by</param>
    /// <param name="action">The action that is calling the subaction</param>
    public virtual void Execute(BattleObject actor, GameAction action)
    {
        //Since subaction types can't be seperate objects, the only thing we can do is a big honking case block.
        //Sorry.

        switch (SubactionName)
        {
            // ====== CONTROL SUBACTIONS ======\\
            case "DoAction":
                /* doAction actionName:string
                 *      Switches the fighter's action to actionName
                 */
                {
                    actor.BroadcastMessage("DoAction", (string)GetArgument("actionName", actor, action));
                }
                break;
            case "DoTransition":
                /* doTransition transitionState:string
                 * 	    Executes the named helper StateTransition
                 */
                {
                    StateTransitions.LoadTransitionState((string)GetArgument("transitionState", actor, action), actor.GetAbstractFighter());
                }
                break;
            case "ChangeFrame":
                /* setFrame frameNumber:int relative:bool
                 *      Sets the current frame to the given number
                 */
                {
                    bool relative = (bool)GetArgument("relative", actor, action, true);
                    int frame = (int)GetArgument("frameNumber", actor, action, 1);
                    if (relative) {
                        action.current_frame += frame;
                    } else
                    {
                        action.current_frame = frame;
                    }
                }
                break;
            case "SetVar":
                /* setVar source:string name:string type:string value:dynamic relative:bool|false
                 *      Sets the variable from GameAction, Fighter, Global with the given name to the given value and type.
                 *      If relative is set and type is something that can be relative, such as integer, it will increment
                 *      the variable instead of changing it
                 */
                 //TODO
                break;
            case "IfVar":
                /* ifVar source:string name:string compare:string|== value:dynamic|true
                 *      Sets the action condition to the result of the logical equation compare(source|name, value)
                 */
                //TODO
                break;
            case "Else":
                /* else
                 *      inverts the current action condition
                 */
                //TODO
                break;
            case "Endif":
                /* endif
                 *      unsets the current action condition
                 */
                //TODO
                break;
            
            // ====== CONTROL SUBACTIONS ======\\
            case "ChangeSpeed":
                /* changeSpeed x:float|_ y:float|_ xpref:float|_ ypref:float|_ relative:bool|false
                 *      changes the xSpeed, ySpeed, xPreferred, yPreferred speeds. If set to null, value will remain the same
                 */
                {
                    bool relative = (bool)GetArgument("relative", actor, action, false);
                    if (arg_dict.ContainsKey("xSpeed"))
                    {
                        if (relative) actor.SendMessage("ChangeXSpeedBy", GetArgument("xSpeed", actor, action));
                        else actor.SendMessage("ChangeXSpeed", GetArgument("xSpeed", actor, action));
                    }
                    if (arg_dict.ContainsKey("ySpeed"))
                    {
                        if (relative) actor.SendMessage("ChangeYSpeedBy", GetArgument("ySpeed", actor, action));
                        else actor.SendMessage("ChangeYSpeed", GetArgument("ySpeed", actor, action));
                    }
                    if (arg_dict.ContainsKey("xPreferred"))
                    {
                        if (relative) actor.SendMessage("ChangeXPreferredby", GetArgument("xPreferred", actor, action));
                        else actor.SendMessage("ChangeXPreferred", GetArgument("xPreferred", actor, action));
                    }
                    if (arg_dict.ContainsKey("yPreferred"))
                    {
                        if (relative) actor.SendMessage("ChangeYPreferredby", GetArgument("yPreferred", actor, action));
                        else actor.SendMessage("ChangeYPreferred", GetArgument("yPreferred", actor, action));
                    }
                }
                break;
            case "ShiftPosition":
                /* shiftPosition x:float|0 y:float|0 relative:bool|true
                 *      Displaces the fighter by a certain amount in either direction
                 */
                //TODO
                break;
            // ====== CONTROL SUBACTIONS ======\\
            case "ChangeSprite":
                /* changeAnim animName:string
                 *      Changes to the specified animation.
                 *      ALIAS: ChangeSprite
                 */
                {
                    string sprite = (string)GetArgument("spriteName", actor, action);
                    actor.BroadcastMessage("ChangeSprite", sprite);
                }
                break;
            case "ChangeSubimage":
                /* changeSpriteSubimage index:int
                 *      SPRITE MODE ONLY
                 *      Changes to the sprite subimage of the current animation with the given index
                 */
                {
                    int subimage = (int) GetArgument("index", actor, action);
                    action.sprite_rate = 0; //We've broken the integrity of the sprite_rate calculator, so we have to turn it off
                    actor.BroadcastMessage("ChangeSubimage", subimage);
                }
                break;
            case "Flip":
                /* flipFighter
                 *      Flips the fighter horizontally, so they are facing the other direction
                 */
                {
                    actor.BroadcastMessage("flip");
                }
                break;
            case "RotateSprite":
                /* rotateFighter deg:int
                 *      Rotates the fighter by the given degrees
                 */
                //TODO
                break;
            case "Unrotate":
                /* unrotateFighter
                 *      Sets the fighter back to upright, no matter how many times it has been rotated
                 */
                //TODO
                break;
            case "ShiftSprite":
                /* shiftSprite x:float y:float
                 *      Shifts the sprite by the given X and Y without moving the fighter
                 */
                //TODO
                break;
            case "PlaySound":
                /* Playsound sound:string
                 *      Plays the sound with the given name from the fighter's sound library
                 */
                {
                    string sound_name = (string) GetArgument("sound", actor, action);
                    actor.BroadcastMessage("PlaySound", sound_name);
                }
                break;
            // ====== HITBOX SUBACTIONS ======\\
            case "CreateHitbox":
                /* createHitbox name:string [argumentName:string value:dynamic]
                 *      Creates a hitbox with the given name. Every pair of arguments from then after is the name of a value, and what to set it to.
                 *      Hitboxes will be able to parse the property name and extract the right value out.
                 */
                {
                    string name = "";
                    Dictionary<string, string> hbox_dict = new Dictionary<string, string>();
                    foreach (SubactionVarData data in arg_list)
                    {
                        if (data.name == "name")
                            name = (string)data.GetData(actor, action);
                        else
                        {
                            hbox_dict.Add(data.name, (string) data.GetData(actor, action));
                        }
                    }
                    if (name != "")
                    {
                        Hitbox hbox = HitboxLoader.loader.LoadHitbox(actor.GetAbstractFighter(), action, hbox_dict);
                        action.hitboxes.Add(name, hbox);
                    }
                }
                break;
            case "ActivateHitbox":
                /* activateHitbox name:string life:int
                 *      Activates the named hitbox, if it exists, for the given number of frames.
                 *      If life is -1, hitbox will persist until manually deactivated.
                 */
                {
                    string name = (string) GetArgument("name", actor, action);
                    int life = (int)GetArgument("life", actor, action, -1);
                    if (action.hitboxes.ContainsKey(name))
                        action.hitboxes[name].Activate(life);
                    else
                        Debug.LogWarning("Current action has no hitbox named " + name);
                }
                break;
            case "DeactivateHitbox":
                /* activateHitbox name:string life:int
                 *      Activates the named hitbox, if it exists, for the given number of frames.
                 */
                {
                    string name = (string)GetArgument("name", actor, action);
                    if (action.hitboxes.ContainsKey(name))
                        action.hitboxes[name].Deactivate();
                    else
                        Debug.LogWarning("Current action has no hitbox named " + name);
                }
                break;
            case "ModifyHitbox":
                /* createHitbox name:string [argumentName:string value:dynamic]
                 *      Creates a hitbox with the given name. Every pair of arguments from then after is the name of a value, and what to set it to.
                 *      Hitboxes will be able to parse the property name and extract the right value out.
                 */
                {
                    string name = "";
                    Dictionary<string, string> hbox_dict = new Dictionary<string, string>();
                    foreach (SubactionVarData data in arg_list)
                    {
                        if (data.name == "name")
                            name = (string)data.GetData(actor, action);
                        else
                        {
                            hbox_dict.Add(data.name, (string)data.GetData(actor, action));
                        }
                    }
                    if (name != "" && action.hitboxes.ContainsKey(name))
                    {
                        action.hitboxes[name].LoadValuesFromDict(actor.GetAbstractFighter(), hbox_dict);
                    }
                }
                break;
            default:
                //Debug.LogWarning("Could not load subaction " + args[0]);
                break;
        }
    }

    /// <summary>
    /// A quick helper function for execute to get the data from the arg dict
    /// </summary>
    /// <param name="arg_name"></param>
    /// <param name="owner"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public object GetArgument(string arg_name, BattleObject owner, GameAction action, object defaultValue=null)
    {
        if (arg_dict == null) BuildDict();
        if (arg_dict.ContainsKey(arg_name))
        {
            return arg_dict[arg_name].GetData(owner,action);
        } else
        {
            Debug.LogWarning("Argument not found in subaction: " + arg_name);
        }
        return defaultValue;
    }

    /// <summary>
    /// Get a list of modules that are required for the subaction to execute
    /// </summary>
    /// <returns>A list of names of modules that are required by the subaction</returns>
    public virtual List<string> GetRequirements()
    {
        return null;
    }

    /// <summary>
    /// Check if the subaction is a conditional subaction, which would execute even if the conditional flag isn't set for execution.
    /// </summary>
    /// <returns>If the subaction should be executed regardless of conditional status</returns>
    public bool isConditional()
    {
        return is_control_subaction;
    }

    /// <summary>
    /// Check if the subaction should execute in build mode, like animation and control flow subactions.
    /// </summary>
    /// <returns>If the subaction should be executed in the builder</returns>
    public bool executeInBuilder()
    {
        return execute_in_builder;
    }
}


[System.Serializable]
public class SubactionVarData
{
    private bool editable = true;

    public string name; //The name of the variable in the subaction, so you can easily set arguments without needing to remember order
    public string source; //Constant, Owner, or Action. The source of the variable
    public string type; //String, Int, Float, or Bool
    public string data; //The string representation of the data or the name of the variable to use

    public SubactionVarData(string _name, string _source, string _type, string _data, bool _editable = true)
    {
        name = _name;
        source = _source;
        type = _type;
        data = _data;
        editable = _editable;
    }

    public object GetData(BattleObject owner, GameAction action)
    {
        if (source == "Constant")
        {
            if (type == "string") return data;
            else if (type == "int") return int.Parse(data);
            else if (type == "float") return float.Parse(data);
            else if (type == "bool") return bool.Parse(data);
            else
            {
                Debug.LogError("SubactionVarData incorrect type: " + type);
                return null;
            }
        }
        else if (source == "Owner")
        {
            if (type == "string") return owner.GetStringVar(data);
            else if (type == "int") return owner.GetIntVar(data);
            else if (type == "float") return owner.GetFloatVar(data);
            else if (type == "bool") return owner.GetBoolVar(data);
            else
            {
                Debug.LogError("SubactionVarData incorrect type: " + type);
                return null;
            }
        }
        else if (source == "Action")
        {
            if (type == "string") return action.GetStringVar(data);
            else if (type == "int") return action.GetIntVar(data);
            else if (type == "float") return action.GetFloatVar(data);
            else if (type == "bool") return action.GetBoolVar(data);
            else
            {
                Debug.LogError("SubactionVarData incorrect type: " + type);
                return null;
            }
        }
        else
        {
            Debug.LogError("SubactionVarData incorrect source: " + source);
            return null;
        }
    }
}
