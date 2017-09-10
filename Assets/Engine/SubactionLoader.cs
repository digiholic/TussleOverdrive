using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SubactionLoader : BattleComponent {
    
    public static void executeSubaction(string subact, BattleObject actor, GameAction action)
    {
        string[] args = subact.Split(' ');
        
        switch (args[0])
        {
            // ====== CONTROL SUBACTIONS ======\\
            case "DoAction":
                /* doAction actionName:string
                 *      Switches the fighter's action to actionName
                 */
                actor.BroadcastMessage("DoAction",args[1]);
                break;
            case "DoTransition":
                /* doTransition transitionState:string
                 * 	    Executes the named helper StateTransition
                 */
                StateTransitions.LoadTransitionState(args[1], actor.GetAbstractFighter());
                break;
            case "SetFrame":
                /* setFrame frameNumber:int
                 *      Sets the current frame to the given number
                 */
                action.current_frame = int.Parse(args[1]);
                break;
            case "ChangeFrame":
                /* changeFrame frameNumber:int|1
                 *      Changes the action frame by the specified amount.
                 */
                action.current_frame += int.Parse(args[1]);
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
                if (args[1] != "_")
                    actor.GetMotionHandler().ChangeXSpeed(float.Parse(args[1]));
                if (args[2] != "_")
                    actor.GetMotionHandler().ChangeYSpeed(float.Parse(args[2]));
                if (args[3] != "_")
                    actor.GetMotionHandler().ChangeXPreferred(float.Parse(args[3]));
                if (args[4] != "_")
                    actor.GetMotionHandler().ChangeYPreferred(float.Parse(args[4]));
                break;
            case "ChangeXSpeed":
                /* changeXSpeed x:float rel:bool
                 *      changes the xSpeed of the fighter
                 */
                if (args.Length > 2)
                    actor.GetMotionHandler().ChangeXSpeedBy(float.Parse(args[1]) * actor.GetIntVar("facing"));
                else
                    actor.GetMotionHandler().ChangeXSpeed(float.Parse(args[1]) * actor.GetIntVar("facing"));
                break;
            case "ChangeYSpeed":
                /* changeYSpeed y:float rel:bool
                 *      changes the ySpeed of the fighter
                 */
                if (args.Length > 2)
                    actor.GetMotionHandler().ChangeYSpeedBy(float.Parse(args[1]));
                else
                    actor.GetMotionHandler().ChangeYSpeed(float.Parse(args[1]));
                break;
            case "ChangeXPreferred":
                /* changeXPreferred x:float rel:bool
                 *      changes the preferred xSpeed of the fighter
                 */
                if (args.Length > 2)
                    actor.GetMotionHandler().ChangeXPreferredBy(float.Parse(args[1]) * actor.GetIntVar("facing"));
                else
                    actor.GetMotionHandler().ChangeXPreferred(float.Parse(args[1]) * actor.GetIntVar("facing"));
                break;
            case "ChangeYPreferred":
                /* changeXPreferred y:float rel:bool
                 *      changes the yPreferred of the fighter
                 */
                if (args.Length > 2)
                    actor.GetMotionHandler().ChangeYPreferredBy(float.Parse(args[1]) * actor.GetIntVar("facing"));
                else
                    actor.GetMotionHandler().ChangeYPreferred(float.Parse(args[1]) * actor.GetIntVar("facing"));
                break;
            case "ShiftPosition":
                /* shiftPosition x:float|0 y:float|0 relative:bool|true
                 *      Displaces the fighter by a certain amount in either direction
                 */
                //TODO
                break;
            // ====== CONTROL SUBACTIONS ======\\
            case "ChangeAnim":
                /* changeAnim animName:string
                 *      Changes to the specified animation.
                 *      ALIAS: ChangeSprite
                 */
                actor.BroadcastMessage("ChangeSprite",args[1]);
                break;
            case "ChangeSprite":
                /* changeSprite animName:string
                 *      Changes to the specified animation.
                 *      ALIAS: ChangeAnim
                 */
                actor.BroadcastMessage("ChangeSprite",args[1]);
                break;

            case "ChangeSubimage":
                /* changeSpriteSubimage index:int
                 *      SPRITE MODE ONLY
                 *      Changes to the sprite subimage of the current animation with the given index
                 */
                action.sprite_rate = 0; //We've broken the integrity of the sprite_rate calculator, so we have to turn it off
                actor.BroadcastMessage("ChangeSubimage",int.Parse(args[1]));
                break;
            case "Flip":
                /* flipFighter
                 *      Flips the fighter horizontally, so they are facing the other direction
                 */
                actor.BroadcastMessage("flip");
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
                actor.BroadcastMessage("PlaySound",args[1]);
                break;
            // ====== HITBOX SUBACTIONS ======\\
            case "CreateHitbox":
                /* createHitbox name:string [argumentName:string value:dynamic]
                 *      Creates a hitbox with the given name. Every pair of arguments from then after is the name of a value, and what to set it to.
                 *      Hitboxes will be able to parse the property name and extract the right value out.
                 */
                string name = args[1];
                Dictionary<string, string> hbox_dict = new Dictionary<string, string>();
                for (int i = 2; i < args.Length; i = i + 2)
                {
                    hbox_dict[args[i]] = args[i + 1];
                }
                Hitbox hbox = FindObjectOfType<HitboxLoader>().LoadHitbox(actor.GetAbstractFighter(), action, hbox_dict);
                action.hitboxes.Add(name, hbox);
                break;
            case "ActivateHitbox":
                /* activateHitbox name:string life:int
                 *      Activates the named hitbox, if it exists, for the given number of frames.
                 *      If life is -1, hitbox will persist until manually deactivated.
                 */
                name = args[1];
                if (action.hitboxes.ContainsKey(args[1]))
                    action.hitboxes[args[1]].Activate(int.Parse(args[2]));
                else
                    Debug.LogWarning("Current action has no hitbox named " + name);
                break;
            case "DeactivateHitbox":
                /* activateHitbox name:string life:int
                 *      Activates the named hitbox, if it exists, for the given number of frames.
                 */
                name = args[1];
                if (action.hitboxes.ContainsKey(args[1]))
                    action.hitboxes[args[1]].Deactivate();
                else
                    Debug.LogWarning("Current action has no hitbox names " + name);
                break;
            case "ModifyHitbox":
                /* createHitbox name:string [argumentName:string value:dynamic]
                 *      Creates a hitbox with the given name. Every pair of arguments from then after is the name of a value, and what to set it to.
                 *      Hitboxes will be able to parse the property name and extract the right value out.
                 */
                name = args[1];
                hbox_dict = new Dictionary<string, string>();
                for (int i = 2; i < args.Length; i = i + 2)
                {
                    hbox_dict[args[i]] = args[i + 1];
                }
                if (action.hitboxes.ContainsKey(name))
                {
                    action.hitboxes[name].LoadValuesFromDict(actor.GetAbstractFighter(),hbox_dict);
                }
                break;
            default:
                //Debug.LogWarning("Could not load subaction " + args[0]);
                break;

        }
    }
}