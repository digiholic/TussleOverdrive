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
[System.Serializable]
public class BattleObject : MonoBehaviour, IVarDataContainer
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
    private InputBuffer inputBuffer;

    private List<BattleComponent> components;

    [SerializeField,HideInInspector]
    private List<string> componentsAsJson;

    public Dictionary<string, BattleObjectVarData> variables = new Dictionary<string, BattleObjectVarData>();
    private Dictionary<string, AnchorPoint> anchor_points = new Dictionary<string, AnchorPoint>();

    public GameObject hurtboxObject;
    public GameObject spriteObject;

    // Use this for initialization
    void Awake()
    {
        components = new List<BattleComponent>(gameObject.GetComponents<BattleComponent>());
    }
    
    public void LoadComponents()
    {
        components = new List<BattleComponent>(gameObject.GetComponents<BattleComponent>());

        abstractFighter = GetComponent<AbstractFighter>();
        inputBuffer = GetComponent<InputBuffer>();

        /*
        actionHandler = GetComponent<ActionHandler>();
        environmentCollider = GetComponent<EnvironmentCollider>();
        hitboxLoader = GetComponent<HitboxLoader>();
        hurtboxLoader = GetComponent<HurtboxLoader>();
        modelHandler = GetComponent<ModelHandler>();
        motionHandler = GetComponent<MotionHandler>();
        platform = GetComponent<Platform>();
        spriteHandler = GetComponent<SpriteHandler>();
        */
    }

    public void Start()
    {
        SetVar("StopFrames", 0);
        if (BattleController.current_battle != null)
        {
            BattleController.current_battle.RegisterObject(this);
        }
    }

    public string ToJson(bool prettyPrint = false)
    {
        componentsAsJson = new List<string>();
        foreach (BattleComponent comp in components)
        {
            componentsAsJson.Add(comp.ToJson(prettyPrint));
        }
        PrintDebug(this, 2, JsonUtility.ToJson(this, true));
        return JsonUtility.ToJson(this,prettyPrint);
    }

    public void FromJson(string JSONObject)
    {
        JsonUtility.FromJsonOverwrite(JSONObject, this);
        foreach (string compJSON in componentsAsJson)
        {
            BattleComponent comp = JsonUtility.FromJson<BattleComponent>(compJSON);
            AddComponent(comp.BattleComponentType, compJSON);
        }
    }
    
    public void AddComponent(string componentType, string componentJSON)
    {
        switch (componentType)
        {
            case "AbstractFighter":
                AbstractFighter abstractFighter = GetComponent<AbstractFighter>();
                if (abstractFighter == null)
                    abstractFighter = gameObject.AddComponent<AbstractFighter>();
                JsonUtility.FromJsonOverwrite(componentJSON, abstractFighter);
                break;
            case "ActionHandler":
                ActionHandler actionHandler = GetComponent<ActionHandler>();
                if (actionHandler == null)
                    actionHandler = gameObject.AddComponent<ActionHandler>();
                JsonUtility.FromJsonOverwrite(componentJSON, actionHandler);
                break;
            default:
                PrintDebug(this, 1, "Battle Component of Unrecognized type in JSON: " + componentType);
                BattleComponent comp = gameObject.AddComponent<BattleComponent>();
                JsonUtility.FromJsonOverwrite(componentJSON, comp);
                break;
        }
    }

    public void StepFrame()
    {
        //Process Hitstop
        int StopFrames = GetIntVar("StopFrames");
        if (StopFrames > 0)
        {
            SetVar("StopFrames", StopFrames - 1);
            return;
        }

        SendMessage("ManualUpdate");

        //Motion must be done after all of the components resolve.
        SendMessage("ExecuteMovement");
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
    //Initialize the variable if it's not set yet, then return it
    public BattleObjectVarData GetVar(string var_name)
    {
        if (!HasVar(var_name))
        {
            Debug.Log("Attempting to get variable without setting one: " + var_name);
            //return new BattleObjectVarData(var_name, null);
            SetVar(var_name, null);
        }
        return variables[var_name];
    }


    public void SetVar(string var_name, object obj)
    {
        if (HasVar(var_name))
        {
            variables[var_name].SetData(obj);
        } else
        {
            variables[var_name] = new BattleObjectVarData(var_name,obj);
        }
    }

    public bool HasVar(string var_name)
    {
        if (variables.ContainsKey(var_name))
            return true;
        else
            return false;
    }

    /// <summary>
    /// Gets the variable with the given name from the variables list
    /// </summary>
    /// <param name="var_name">The name of the variable to pull</param>
    /// <returns>The variable from the dict as an object</returns>
    public object GetVarData(string var_name)
    {
        return GetVar(var_name).GetData();
    }

    public AbstractFighter GetAbstractFighter()
    {
        if (abstractFighter == null)
            abstractFighter = GetComponent<AbstractFighter>();
        return abstractFighter;
    }

    public InputBuffer GetInputBuffer()
    {
        if (inputBuffer == null)
            inputBuffer = GetComponent<InputBuffer>();
        return inputBuffer;
    }

    public MotionHandler GetMotionHandler(){
        if (motionHandler == null)
            motionHandler = GetComponent<MotionHandler>();
        return motionHandler;
    }

    public int GetIntVar(string var_name)
    {
        return GetVar(var_name).GetIntData();
    }

    public float GetFloatVar(string var_name)
    {
        return GetVar(var_name).GetFloatData();
    }

    public bool GetBoolVar(string var_name)
    {
        return GetVar(var_name).GetBoolData();
    }

    public string GetStringVar(string var_name)
    {
        return GetVar(var_name).GetStringData();
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

    void OnDestroy()
    {
        if (BattleController.current_battle != null)
            BattleController.current_battle.UnregisterObject(this);
    }
}