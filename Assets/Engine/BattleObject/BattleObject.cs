using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Battle Object serves as a sort of roadmap of components that can make up an object.
/// The best way to think of things is to think of anything that changes in a scene as a BattleObject that has
/// multiple BattleComponents. These components determine how the object behaves. They can be everything from
/// fighters to projectiles to stage hazards.
/// </summary>
/// 
public class BattleObject : MonoBehaviour {
    /* Each component has a public accessor that will route commands to the right objects for the purposes of reading data,
     * but most methods should be called via the BroadcastMessage function, so that it could potentially hit multiple Components.
     */
    private AbstractFighter abstractFighter;
    private ActionHandler actionHandler;
    private EnvironmentCollider environmentCollider;
    private HitboxLoader hitboxLoader;
    private HurtboxLoader hurtboxLoader;
    private ModelHandler modelHandler;
    private MotionHandler motionHandler;
    private Platform platform;
    private SpriteHandler spriteHandler;

    public Dictionary<string, object> variable = new Dictionary<string, object>();

    // Use this for initialization
    void Awake () {
        abstractFighter = GetComponent<AbstractFighter>();
        actionHandler = GetComponent<ActionHandler>();
        environmentCollider = GetComponent<EnvironmentCollider>();
        hitboxLoader = GetComponent<HitboxLoader>();
        hurtboxLoader = GetComponent<HurtboxLoader>();
        modelHandler = GetComponent<ModelHandler>();
        motionHandler = GetComponent<MotionHandler>();
        platform = GetComponent<Platform>();
        spriteHandler = GetComponent<SpriteHandler>();
}
	
	// ManualUpdate is called once per frame by the calling object
	public void ManualUpdate () {
        
    }

    public void Update()
    {
        
    }

    /// <summary>
    /// Checks if the given subaction will work on this BattleObject.
    /// It does this by checking if all of the required components are included.
    /// </summary>
    /// <param name="subact">The Subaction to validate on</param>
    /// <returns>True if this BattleObject meets all of the requirements of the subaction, false otherwise.</returns>
    public bool ValidateSubaction(Subaction subact)
    {
        //We start with a true, and && it with the existance of each requirement.
        bool ret = true;
        foreach (string req in subact.GetRequirements())
        {
            switch (req) {
                case "ActionHandler":
                    ret = ret && (actionHandler != null); //if ret is true, and the action handler is set, it stays true
                    break;
                case "AbstractFighter":
                    ret = ret && (abstractFighter != null); //if ret is true, and the action handler is set, it stays true
                    break;
                case "motionHandler":
                    ret = ret && (motionHandler != null); //if ret is true, and the action handler is set, it stays true
                    break;
            }
        }
        return ret;
    }


    /// <summary>
    /// Gets the AbstractFighter component of the object.
    /// </summary>
    /// <returns>The Abstract Fighter component of the Object</returns>
    public AbstractFighter GetAbstractFighter()
    {
        return abstractFighter;
    }

    /// <summary>
    /// Gets the Action Handler component of the object.
    /// </summary>
    /// <returns>The Action Handler component of the Object</returns>
    public ActionHandler GetActionHandler()
    {
        return actionHandler;
    }

    /// <summary>
    /// Gets the EnvironmentCollider component of the object.
    /// </summary>
    /// <returns>The Environment Collider component of the Object</returns>
    public EnvironmentCollider GetEnvironmentCollider()
    {
        return environmentCollider;
    }

    /// <summary>
    /// Gets the HitboxLoader component of the object.
    /// </summary>
    /// <returns>The Hitbox Loader component of the Object</returns>
    public HitboxLoader GetHitboxLoader()
    {
        return hitboxLoader;
    }

    /// <summary>
    /// Gets the HurtboxLoader component of the object.
    /// </summary>
    /// <returns>The Hurtbox Loader component of the Object</returns>
    public HurtboxLoader GetHurtboxLoader()
    {
        return hurtboxLoader;
    }

    /// <summary>
    /// Gets the ModelHandler component of the object.
    /// </summary>
    /// <returns>The Model Handler component of the Object</returns>
    public ModelHandler GetModelHandler()
    {
        return modelHandler;
    }

    /// <summary>
    /// Gets the Motion Handler component of the object.
    /// </summary>
    /// <returns>The Motion Handler component of the Object</returns>
    public MotionHandler GetMotionHandler()
    {
        return motionHandler;
    }

    /// <summary>
    /// Gets the Platform component of the object.
    /// </summary>
    /// <returns>The Platform component of the Object</returns>
    public Platform GetPlatform()
    {
        return platform;
    }

    /// <summary>
    /// Gets the SpriteHandler component of the object.
    /// </summary>
    /// <returns>The Sprite Handler component of the Object</returns>
    public SpriteHandler GetSpriteHandler()
    {
        return spriteHandler;
    }
}
