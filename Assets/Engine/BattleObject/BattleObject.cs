﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Battle Object serves as a sort of roadmap of components that can make up an object.
/// The best way to think of things is to think of anything that changes in a scene as a BattleObject that has
/// multiple BattleComponents. These components determine how the object behaves. They can be everything from
/// fighters to projectiles to stage hazards.
/// </summary>
/// 
[System.Serializable]
public class BattleObject : MonoBehaviour
{
    /// <summary>
    /// DebugLevel will be checked against all throughout the object to determine when to log information.
    /// 
    /// 0: None - No information is printed.
    /// 1: Error - Print when an error occurs.
    /// 2: Debug - Prints various debug information as needed in code.
    /// 3: All - Prints everything. WARNING: HUGE PERFORMANCE LOSS!
    /// </summary>
    public int DebugLevel = 2;

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
    private Dictionary<string, AnchorPoint> anchor_points = new Dictionary<string, AnchorPoint>();

    //These are used only when serializing the main object
    [SerializeField,HideInInspector]
    private string abstractFighterJSON, actionHandlerJSON, environmentColliderJSON, hitboxLoaderJSON, hurtboxLoaderJSON, modelHandlerJSON, motionHandlerJSON, platformJSON, spriteHandlerJSON = "";
    
    // Use this for initialization
    void Awake()
    {
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

    public void Start()
    {
        //Debug.Log(ToJson(true));
        FromJson(ToJson());
    }

    public string ToJson(bool prettyPrint = false)
    {
        if (abstractFighter != null)
            abstractFighterJSON = JsonUtility.ToJson(abstractFighter);
        if (actionHandler != null)
            actionHandlerJSON = JsonUtility.ToJson(actionHandler);
        if (environmentCollider != null)
            environmentColliderJSON = JsonUtility.ToJson(environmentCollider);
        if (hitboxLoader != null)
            hitboxLoaderJSON = JsonUtility.ToJson(hitboxLoader);
        if (hurtboxLoader != null)
            hurtboxLoaderJSON = JsonUtility.ToJson(hurtboxLoader);
        if (modelHandler != null)
            modelHandlerJSON = JsonUtility.ToJson(modelHandler);
        if (motionHandler != null)
            motionHandlerJSON = JsonUtility.ToJson(motionHandler);
        if (platform != null)
            platformJSON = JsonUtility.ToJson(platform);
        if (spriteHandler != null)
            spriteHandlerJSON = JsonUtility.ToJson(spriteHandler);
        PrintDebug(this, 2, JsonUtility.ToJson(this, true));
        return JsonUtility.ToJson(this,prettyPrint);
    }

    public void FromJson(string JSONObject)
    {
        JsonUtility.FromJsonOverwrite(JSONObject, this);
        if (abstractFighterJSON != "")
        {
            if (GetComponent<AbstractFighter>() == null)
                abstractFighter = gameObject.AddComponent<AbstractFighter>();
            JsonUtility.FromJsonOverwrite(abstractFighterJSON, abstractFighter);
        }
        if (actionHandlerJSON != "")
        {
            if (GetComponent<ActionHandler>() == null)
                actionHandler = gameObject.AddComponent<ActionHandler>();
            JsonUtility.FromJsonOverwrite(actionHandlerJSON, actionHandler);
        }
    }

    // ManualUpdate is called once per frame by the calling object
    public void ManualUpdate()
    {

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
            switch (req)
            {
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

    public void SetVar(string var_name, object obj)
    {
        variable[var_name] = obj;
    }

    /// <summary>
    /// Gets the variable with the given name from the variables list
    /// </summary>
    /// <param name="var_name">The name of the variable to pull</param>
    /// <returns>The variable from the dict as an object</returns>
    public object GetVar(string var_name)
    {
        if (variable.ContainsKey(var_name))
        {
            return variable[var_name];
        }
        else
        {
            Debug.LogWarning("Could not find variable " + var_name + " in BattleObject " + this.ToString());
            return null;
        }
    }

    public int GetIntVar(string var_name)
    {
        return (int)GetVar(var_name);
    }

    public float GetFloatVar(string var_name)
    {
        return (float)GetVar(var_name);
    }

    public bool GetBoolVar(string var_name)
    {
        return (bool)GetVar(var_name);
    }

    public string GetStringVar(string var_name)
    {
        return (string)GetVar(var_name);
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

    public AnchorPoint GetAnchorPoint(string name)
    {
        if (!anchor_points.ContainsKey(name)) //If we don't have this anchor point
        {
            PrintDebug(this, 2, "Creating new Anchor Point Object: " + name);
            GameObject anchorobject = new GameObject(name);
            anchorobject.transform.parent = gameObject.transform;
            AnchorPoint anchorpoint = anchorobject.AddComponent<AnchorPoint>();
            anchorpoint.MoveAnchorPixel(0, 0);
            anchor_points.Add(name, anchorpoint);
        }
        return anchor_points[name];
    }

    public void PrintDebug(object callingObject, int debugLevel, string message)
    {
        if (DebugLevel >= debugLevel)
        {
            string retval = "[" + System.DateTime.Now.ToString(@"MM\/dd\/yyyy h\:mm tt") + "]";
            retval += callingObject.GetType().Name;
            retval += " - ";
            retval += message;
            Debug.Log(retval);
        }
    }
}