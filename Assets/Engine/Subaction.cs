using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subaction
{
    public List<string> Requirements = new List<string>(); //TODO

    public virtual void Execute(BattleObject obj, GameAction action)
    {

    }

    public virtual List<string> GetRequirements()
    {
        return new List<string>();
    }

    /// <summary>
    /// This method will take a string representation of a subaction and return the
    /// actual subaction object. The object it returns is of the type "Subaction"
    /// rather than it's narrow type, but you should still be able to call any of the
    /// virtual methods defined in Subaction.
    /// </summary>
    /// <returns>The corresponding Subaction object to the string that is given</returns>
    public static Subaction FromString(string subactionString)
    {
        string[] args = subactionString.Split(' ');

        switch (args[0])
        {
            case "DoAction":
                return new SubactionDoAction(args[1]);
            case "DoTransition":
                return new SubactionDoTransition(args[1]);
            case "SetFrame":
                return new SubactionSetFrame(int.Parse(args[1]));
            case "ChangeFrame":
                return new SubactionChangeFrame(int.Parse(args[1]));
            case "SetVar":
                /* setVar source:string name:string type:string value:dynamic relative:bool|false
                 *      Sets the variable from GameAction, Fighter, Global with the given name to the given value and type.
                 *      If relative is set and type is something that can be relative, such as integer, it will increment
                 *      the variable instead of changing it
                 */
                return new SubactionSetVar(args[1], args[2], args[3], args[4], bool.Parse(args[5]));
            case "IfVar":
                /* ifVar source:string name:string compare:string|== value:dynamic|true
                 *      Sets the action condition to the result of the logical equation compare(source|name, value)
                 */
                return new SubactionIfVar(args[1], args[2], args[3], args[4]);
            case "Else":
                /* else
                 *      inverts the current action condition
                 */
                return new SubactionElse();
            case "Endif":
                /* endif
                 *      unsets the current action condition
                 */
                return new SubactionEndIf();

            // ====== CONTROL SUBACTIONS ======\\
            case "ChangeSpeed":
                /* changeSpeed x:float|_ y:float|_ xpref:float|_ ypref:float|_ relative:bool|false
                 *      changes the xSpeed, ySpeed, xPreferred, yPreferred speeds. If set to null, value will remain the same
                 */
                throw new System.Exception("Deprecated Subaction ChangeSpeed, please use ChangeXSpeed and ChangeYSpeed instead.");
                return null;
            case "ChangeXSpeed":
                /* changeXSpeed x:float rel:bool
                 *      changes the xSpeed of the fighter
                 */
                return new SubactionChangeXSpeed(int.Parse(args[1]), bool.Parse(args[2]));
            case "ChangeYSpeed":
                /* changeYSpeed y:float rel:bool
                 *      changes the ySpeed of the fighter
                 */
                return new SubactionChangeYSpeed(int.Parse(args[1]), bool.Parse(args[2]));
            case "ChangeXPreferred":
                /* changeXPreferred x:float rel:bool
                 *      changes the preferred xSpeed of the fighter
                 */
                return new SubactionChangeXPreferred(int.Parse(args[1]), bool.Parse(args[2]));
            case "ChangeYPreferred":
                /* changeXPreferred y:float rel:bool
                 *      changes the yPreferred of the fighter
                 */
                return new SubactionChangeYPreferred(int.Parse(args[1]), bool.Parse(args[2]));
            case "ShiftPosition":
                /* shiftPosition x:float|0 y:float|0 relative:bool|true
                 *      Displaces the fighter by a certain amount in either direction
                 */
                return new SubactionShiftPosition(float.Parse(args[1]), float.Parse(args[2]), bool.Parse(args[3]));
            // ====== CONTROL SUBACTIONS ======\\
            case "ChangeAnim":
                /* changeAnim animName:string
                 *      Changes to the specified animation.
                 *      ALIAS: ChangeSprite
                 */
                return new SubactionChangeAnim(args[1]);
            case "ChangeSprite":
                /* changeSprite animName:string
                 *      Changes to the specified animation.
                 *      ALIAS: ChangeAnim
                 */
                return new SubactionChangeAnim(args[1]);
            case "ChangeSubimage":
                /* changeSpriteSubimage index:int
                 *      SPRITE MODE ONLY
                 *      Changes to the sprite subimage of the current animation with the given index
                 */
                return new SubactionChangeSubimage(int.Parse(args[1]));
            case "Flip":
                /* flipFighter
                 *      Flips the fighter horizontally, so they are facing the other direction
                 */
                return new SubactionFlip();
            case "Rotate":
                /* rotateFighter deg:int
                 *      Rotates the fighter by the given degrees
                 */
                return new SubactionRotate(int.Parse(args[1]));
            case "Unrotate":
                /* unrotateFighter
                 *      Sets the fighter back to upright, no matter how many times it has been rotated
                 */
                return new SubactionUnrotate();
            case "ShiftSprite":
                /* shiftSprite x:float y:float
                 *      Shifts the sprite by the given X and Y without moving the fighter
                 */
                return new SubactionShiftSprite(float.Parse(args[1]), float.Parse(args[2]));
            case "PlaySound":
                /* Playsound sound:string
                 *      Plays the sound with the given name from the fighter's sound library
                 */
                return new SubactionPlaySound(args[1]);
            // ====== HITBOX SUBACTIONS ======\\
            case "CreateHitbox":
                /* createHitbox name:string [argumentName:string value:dynamic]
                 *      Creates a hitbox with the given name. Every pair of arguments from then after is the name of a value, and what to set it to.
                 *      Hitboxes will be able to parse the property name and extract the right value out.
                 */
                return new SubactionCreateHitbox(subactionString);
            case "ActivateHitbox":
                /* activateHitbox name:string life:int
                 *      Activates the named hitbox, if it exists, for the given number of frames.
                 *      If life is -1, hitbox will persist until manually deactivated.
                 */
                return new SubactionActivateHitbox(args[1], int.Parse(args[2]));
            case "DeactivateHitbox":
                /* activateHitbox name:string life:int
                 *      Activates the named hitbox, if it exists, for the given number of frames.
                 */
                return new SubactionDeactivateHitbox(args[1]);
            case "ModifyHitbox":
                /* createHitbox name:string [argumentName:string value:dynamic]
                 *      Creates a hitbox with the given name. Every pair of arguments from then after is the name of a value, and what to set it to.
                 *      Hitboxes will be able to parse the property name and extract the right value out.
                 */
                return new SubactionModifyHitbox(subactionString);
            default:
                Debug.LogWarning("Could not load subaction " + args[0]);
                return null;

        }
    }
}
