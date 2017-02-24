Special Commands:
$fightervarname - Replace with the value of the variable named fightervarname in fighter
@actionvarname - Replace with the value of the variable names actionvarname in action
_ - Pass a null value. Some variables can take null parameters, such as speed changes

Command syntax:

Name argument:type|default argument:type|default argument:type|default
	Description

	
====== CONTROL SUBACTIONS ======
doAction actionName:string
	Switches the fighter's action to actionName
	
doTransition transitionState:string
	Executes the named helper StateTransition
	
changeFrame frameNumber:int|1 relative:boolean|true
	Changes the action frame. If relative is set, will change the action by that many frames. If it is not set, will set directly to the given number. If given no arguments, defaults to incrementing the frame.

setVar source:string name:string type:string value:dynamic relative:bool|false
	Sets the variable from Action, Fighter, Global with the given name to the given value and type. If relative is set and type is something that can be relative, such as integer, it will increment the variable instead of changing it
	
ifVar source:string name:string compare:string|== value:dynamic|true
	Sets the action condition to the result of the logical equation compare(source|name, value)

else
	inverts the current action condition

endif
	unsets the current action condition
	
	
====== BEHAVIOR SUBACTIONS ======
changeSpeed x:float|_ y:float|_ xpref:float|_ ypref:float|_ relative:bool|false
	changes the xSpeed, ySpeed, xPreffered, yPreferred speeds. If set to null, value will remain the same
	
changeXSpeed x:float relative:bool
	changes xSpeed

changeYSpeed y:float relative:bool
	changes ySpeed
	
changeXPref x:float relative:bool
	changes xPreferred
	
changeYPref y:float relative:bool
	changes yPref
	
shiftPosition x:float|0 y:float|0 relative:bool|true
	Displaces the fighter by a certain amount in either direction
	

====== ANIMATION SUBACTIONS ======
changeAnim animName:string
	Changes to the specified animation.
	
changeSpriteSubimage index:int
	SPRITE MODE ONLY
	Changes to the sprite subimage of the current animation with the given index
	
flipFighter
	Flips the fighter horizontally, so they are facing the other direction
	
rotateFighter deg:int
	Rotates the fighter by the given degrees

unrotateFighter
	Sets the fighter back to upright, no matter how many times it has been rotated
	
shiftSprite x:float y:float
	Shifts the sprite by the given X and Y without moving the fighter
	
