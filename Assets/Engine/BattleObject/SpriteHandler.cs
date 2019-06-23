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
    private SpriteInfo sprite_info;

    private AnimationDefinition currentAnimation;
    
    //The sprite render is the Unity component that actually draws stuff on the screen
    private SpriteRenderer sprite_renderer;
    //The sprite component is the GameObject that the sprite is being drawn by, a child object of the Fighter
    private GameObject spriteComponent;

    private float rot_degrees;

    void Awake()
    {
        if (battleObject != null)
            battleObject.spriteObject = spriteComponent;

        SetVar(TussleConstants.SpriteVariableNames.SPRITE_CURRENT, "idle");
        SetVar(TussleConstants.SpriteVariableNames.FRAME_CURRENT, 0);
        if (spriteComponent == null)
        {
            spriteComponent = new GameObject("Sprite");
            RectTransform componentRect = spriteComponent.AddComponent<RectTransform>();
            spriteComponent.transform.SetParent(transform);
            componentRect.anchorMin = new Vector2(0.5f, 0);
            componentRect.anchorMax = new Vector2(0.5f, 0);
            componentRect.anchoredPosition = Vector3.zero;
            sprite_renderer = spriteComponent.AddComponent<SpriteRenderer>();
        } else
        {
            sprite_renderer = spriteComponent.GetComponent<SpriteRenderer>();
        }
    }

    public void OnFighterInfoReady(FighterInfo fInfo)
    {
        fighter_info = fInfo;
        sprite_info = fighter_info.sprite_info;
        //Might not need this var anymore
        //SetVar(TussleConstants.SpriteVariableNames.PIXELS_PER_UNIT, sprite_info.pixelsPerUnit);
    }

    public override void ManualUpdate()
    {
        RenderSprite();
    }
    
    public void ChangeAnimation(string animationName)
    {
        ChangeAnimation(animationName, 0);
    }

    public void ChangeAnimation(string animationName,int startingFrame)
    {
        AnimationDefinition anim = sprite_info.getAnimationByName(animationName);
        currentAnimation = anim; //Animation can be null, but if it is, we don't set the sprite
        if (anim != null)
        { 
            anim.setFrame(startingFrame);
        }
    }
    
    public AnimationDefinition getAnimation()
    {
        return currentAnimation;
    }

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
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        Debug.Log("Flipping");
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