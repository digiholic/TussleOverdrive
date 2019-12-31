using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleController : MonoBehaviour {
    public int current_game_frame = 0;

    public static BattleController current_battle;
    public CameraControl3D battleCamera;

    private List<BattleObject> objects = new List<BattleObject>();
    private Dictionary<int, AbstractFighter> fighterDict = new Dictionary<int, AbstractFighter>();
    [SerializeField] private List<AbstractFighter> fighters = new List<AbstractFighter>();
    private List<Hitbox> hitboxes = new List<Hitbox>();
    public bool UpdateOnFrame;


    // Use this for initialization
    void Start()
    {
        //Fighters shouldn't collide with fighters
        current_battle = this;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Fighters"), LayerMask.NameToLayer("Fighters"), true);
        BattleLoader.current_loader.LoadBattle();

        //Add all the fighters to the battle camera
        foreach (AbstractFighter fighter in fighters){
            battleCamera?.follows.Add(fighter.transform);
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (UpdateOnFrame)
        {
            StepFrame();
        }
        if (Input.GetKeyDown(KeyCode.Slash))
            UpdateOnFrame = !UpdateOnFrame;
        if (Input.GetKeyDown(KeyCode.Period))
        {
            StepFrame();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            BattleLoader.current_loader.ClearBattle();
            SceneManager.LoadScene("CSS", LoadSceneMode.Single);
        }
    }

    void StepFrame()
    {
        foreach (BattleObject obj in objects)
            obj.StepFrame();
        foreach (Hitbox hbox in hitboxes)
            hbox.StepFrame();
        CameraControl3D.current_camera.TrackObjects();            
        current_game_frame++;
    }
    
    /// <summary>
    /// Gets the fighter with the given player number from the list.
    /// </summary>
    /// <param name="playerNum">The number of the player to to find</param>
    /// <returns>A fighter with the given player number, or null if none is found</returns>
    public AbstractFighter GetFighter(int playerNum)
    {
        return fighterDict[playerNum];
    }

    public List<AbstractFighter> GetFighters()
    {
        return fighters;
    }

    /// <summary>
    /// Adds an object to the list of active battle objects
    /// </summary>
    /// <param name="obj"></param>
    public void RegisterObject(BattleObject obj)
    {
        objects.Add(obj);
        AbstractFighter fighter = obj.GetAbstractFighter();
        if (fighter != null)
        {
            fighters.Add(fighter);
            fighterDict.Add(fighter.GetIntVar(TussleConstants.FighterVariableNames.PLAYER_NUM), fighter);
            SendMessage("LoadFighterIcons"); //Reload icons when a new fighter is added
        }
    }

    /// <summary>
    /// Removes an object from the list of active battle objects
    /// </summary>
    /// <param name="obj"></param>
    public void UnregisterObject(BattleObject obj)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hit"></param>
    public void RegisterHitbox(Hitbox hit)
    {
        hitboxes.Add(hit);
    }

    public void UnregisterHitbox(Hitbox hit)
    {

    }
}