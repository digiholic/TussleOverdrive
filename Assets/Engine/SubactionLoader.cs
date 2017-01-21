using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionLoader : ScriptableObject {
    
    public static void executeSubaction(string subact, AbstractFighter actor, Action action)
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
            case "changeFrame":
                /* changeFrame frameNumber:int|1 relative:boolean|true
                 *      Changes the action frame. If relative is set, will change the action by that many frames.
                 *      If it is not set, will set directly to the given number. If given no arguments, defaults to incrementing the frame.
                 */
                if (args[2] == "true")
                    action.current_frame += int.Parse(args[1]);
                else
                    action.current_frame = int.Parse(args[1]);
                break;
            case "setVar":
                /* setVar source:string name:string type:string value:dynamic relative:bool|false
                 *      Sets the variable from Action, Fighter, Global with the given name to the given value and type.
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
        }
    }
}
