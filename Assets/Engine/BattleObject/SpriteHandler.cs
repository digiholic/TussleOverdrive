using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.U2D;

[System.Serializable]
public class SpriteHandler : BattleComponent {
    public static string LEFT = "left";
    public static string RIGHT = "right";

    public string orientation;
    
    private FighterInfo fighter_info;
    [SerializeField]
    private SpriteInfo sprite_info;

    private AnimationDefinition currentAnimation;
    
    //The sprite render is the Unity component that actually draws stuff on the screen
    private SpriteRenderer sprite_renderer;
    //The sprite component is the GameObject that the sprite is being drawn by, a child object of the Fighter
    private GameObject spriteComponent;

    private float rot_degrees;

    [SerializeField] private ImageDefinition currentSubimage;

    void Awake()
    {
        //If the battle object has a sprite object, that's what we'll use instead of making a new one.
        spriteComponent = getBattleObject()?.spriteObject;
        
        SetVar(TussleConstants.SpriteVariableNames.SPRITE_CURRENT, "idle");
        SetVar(TussleConstants.SpriteVariableNames.FRAME_CURRENT, 0);
        if (spriteComponent == null)
        {
            spriteComponent = new GameObject("Sprite");
            spriteComponent.transform.SetParent(getBattleObject()?.PivotTransform);
            spriteComponent.transform.localPosition = Vector3.zero;

            
            sprite_renderer = spriteComponent.AddComponent<SpriteRenderer>();
        } else
        {
            sprite_renderer = spriteComponent.GetComponent<SpriteRenderer>();
        }
    }

    /// <summary>
    /// Sets parameters from the FighterInfo once it's ready loading all its files
    /// </summary>
    /// <param name="fInfo">The Fighter Info that has finished loading</param>
    public void OnFighterInfoReady(FighterInfo fInfo)
    {
        fighter_info = fInfo;
        sprite_info = fighter_info.sprite_info;
    }

    /// <summary>
    /// this BattleComponent's manual update method, called once per frame in gameplay, or when rolling forward a replay
    /// </summary>
    public override void ManualUpdate()
    {
        RenderSprite();
    }
    
    /// <summary>
    /// Gets the center point of the Sprite Component
    /// </summary>
    /// <returns>The Vector3 of the Sprite Component's Transform</returns>
    public Vector3 getCenterPoint(){
        ImageDefinition currentSubimage = getCurrentSubimage();
        if (currentSubimage != null){
            float xPivotRatio = currentSubimage.Pivot.xPos; //0.5
            float yPivotRatio = currentSubimage.Pivot.yPos; //0.0

            float xPos = spriteComponent.transform.position.x; //0
            float yPos = spriteComponent.transform.position.y; //0
            
            float width = currentSubimage.Width / currentSubimage.PixelsPerUnit; //100
            float height = currentSubimage.Height / currentSubimage.PixelsPerUnit; //100

            float xPivotAbs = xPivotRatio * width; //50
            float yPivotAbs = yPivotRatio * height; //0

            float leftPos = xPos - xPivotAbs; //-50
            float bottomPos = yPos - yPivotAbs; //0
            
            float spriteCenterPosX = leftPos + (width / 2);    //0
            float spriteCenterPosY = bottomPos + (height / 2); //50
            return new Vector3(spriteCenterPosX,spriteCenterPosY,spriteComponent.transform.position.z);
        }

        return spriteComponent.transform.position;
    }

    /// <summary>
    /// Change the current Animation of the Sprite Handler, starting at frame 0
    /// </summary>
    /// <param name="animationName">The name of the Animation to get from the Sprite Info</param>
    public void ChangeAnimation(string animationName)
    {
        ChangeAnimation(animationName, 0);
    }

    /// <summary>
    /// Change the current Animation of the Sprite Handler, starting 
    /// </summary>
    /// <param name="animationName">The name of the Animation to get from the Sprite Info</param>
    /// <param name="startingFrame">The frame to start the animation on</param>
    public void ChangeAnimation(string animationName,int startingFrame)
    {
        AnimationDefinition anim = sprite_info.getAnimationByName(animationName);
        currentAnimation = anim; //Animation can be null, but if it is, we don't set the sprite
        if (anim != AnimationDefinition.NullAnimation)
        { 
            anim.setFrame(startingFrame);
            ChangeSubimage(startingFrame);
        }
    }
    
    /// <summary>
    /// Change the subimage of the current animation
    /// </summary>
    /// <param name="frame">The frame to change to</param>
    public void ChangeSubimage(int frame)
    {
        if (currentAnimation != null && currentAnimation != AnimationDefinition.NullAnimation)
        {
            currentAnimation.setFrame(frame);
            sprite_renderer.sprite = sprite_info.getSpriteFromAnimation(currentAnimation.AnimationName, frame);
        }
    }

    /// <summary>
    /// Get the sprite's current animation definition
    /// </summary>
    /// <returns></returns>
    public AnimationDefinition getAnimation()
    {
        return currentAnimation;
    }

    /// <summary>
    /// Get the current subimage of the current animation
    /// </summary>
    /// <returns></returns>
    public ImageDefinition getCurrentSubimage(){
        return sprite_info?.GetImageByName(currentAnimation.getCurrentSubimage(false));
    }

    /// <summary>
    /// Get a list of all Subimages currently loaded into the sprite info
    /// </summary>
    /// <returns></returns>
    public List<ImageDefinition> getImages(){
        return sprite_info.imageDefinitions;
    }

    /// <summary>
    /// Get a list of all Animations currently loaded into the sprite info
    /// </summary>
    /// <returns></returns>
    public List<AnimationDefinition> getAnimations(){
        return sprite_info.animations;
    }

    /// <summary>
    /// Update the sprite render to use the current Sprite from the current Animation
    /// </summary>
    private void RenderSprite()
    {
        sprite_renderer.sprite = sprite_info.getSpriteFromAnimation(currentAnimation.AnimationName);
    }

    public void RotateSprite(float degrees)
    {
        sprite_renderer.transform.Rotate(new Vector3(0, 0, degrees));
        rot_degrees = degrees;
    }

    public void SetRotation(float degrees)
    {
        if (rot_degrees != degrees)
        {
            UnRotate();
            RotateSprite(degrees);
        }
    }

    public void UnRotate()
    {
        sprite_renderer.transform.rotation = Quaternion.identity;
        rot_degrees = 0.0f;
    }

    public void flip()
    {
        //FIXME inverting the scale can cause some issues, replace this with a real rotation
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        if (orientation == LEFT)
            orientation = RIGHT;
        else
            orientation = LEFT;
        Vector3 transfVec = sprite_renderer.transform.localScale;
        transfVec.x *= -1;
    }

    public void ReconcileDirection()
    {
        if (HasVar(TussleConstants.FighterVariableNames.FACING_DIRECTION))
        {
            int facing = GetIntVar(TussleConstants.FighterVariableNames.FACING_DIRECTION);
            if ((facing == 1 && orientation == LEFT) ||
                    (facing == -1 && orientation == RIGHT))
                flip();
        }
    }
}