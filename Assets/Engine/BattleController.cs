using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleController : MonoBehaviour {
    public int current_game_frame = 0;

    public static BattleController current_battle;
    public CameraControl3D battleCamera;

    private List<BattleObject> objects = new List<BattleObject>();
    private Dictionary<int, AbstractFighter> fighterDict = new Dictionary<int, AbstractFighter>();
    private Dictionary<int, FighterResults> resultsDict = new Dictionary<int, FighterResults>();

    [SerializeField] private List<AbstractFighter> fighters = new List<AbstractFighter>();
    private List<Hitbox> hitboxes = new List<Hitbox>();
    public bool UpdateOnFrame;

    private bool isEnding = false;

    public int frameDelay = 1;
    private int currentDelay = 0;

    [SerializeField] private Text ScreenDisplayText;
    [SerializeField] private Text clockDisplayText;
    public float currentTime = 0;
    // Use this for initialization
    void Start()
    {
        //Fighters shouldn't collide with fighters
        current_battle = this;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Fighters"), LayerMask.NameToLayer("Fighters"), true);
        BattleLoader.current_loader.LoadBattle();

        if (!BattleLoader.current_loader.timeInfinity){
            clockDisplayText.enabled = true;
            clockDisplayText.text = BattleLoader.current_loader.timeCount+":00";
        }
        //Add all the fighters to the battle camera
        foreach (AbstractFighter fighter in fighters){
            battleCamera?.addFollow(fighter.transform);
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (UpdateOnFrame)
        {
            UpdateClock();
            currentDelay += 1;
            if (frameDelay == 0 || currentDelay % frameDelay == 0){
                StepFrame();
                currentDelay = 0;
            }
        } else {
            if (Input.GetKeyDown(KeyCode.Period))
            {
                StepFrame();
            }
        }

        if (Input.GetKeyDown(KeyCode.Slash))
            UpdateOnFrame = !UpdateOnFrame;
        
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            BattleLoader.current_loader.ClearBattle();
            SceneManager.LoadScene("GhettoCSS", LoadSceneMode.Single);
        }
    }

    void UpdateClock(){
        currentTime += Time.deltaTime;
            
        int timeElapsedInSeconds = Mathf.FloorToInt(currentTime);
        int maxTimeInSeconds = BattleLoader.current_loader.timeCount * 60;
        int timeRemainingInSeconds = maxTimeInSeconds - timeElapsedInSeconds;
        if (timeRemainingInSeconds > 0){
            int minutesRemaining = timeRemainingInSeconds / 60;
            int secondsRemaining = timeRemainingInSeconds % 60;

            clockDisplayText.text = string.Format("{0}:{1}",minutesRemaining.ToString(),secondsRemaining.ToString("D2"));
        } else {
            clockDisplayText.enabled = false;
            EndBattle();
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
            int fighterNum = fighter.GetIntVar(TussleConstants.FighterVariableNames.PLAYER_NUM);
            fighterDict.Add(fighterNum, fighter);
            resultsDict.Add(fighterNum, new FighterResults(BattleLoader.current_loader.fighters[fighterNum])); //FIXME I think this breaks if you skip a fighter. Check that.
            //If stocks are infinite, we don't populate the stock dict and will just ignore it whenever a player dies
            if (!BattleLoader.current_loader.stockInfinity){
                resultsDict[fighterNum].stocks = BattleLoader.current_loader.stockCount;
            }
            
            SendMessage("LoadFighterIcons"); //Reload icons when a new fighter is added
        }
    }

    /// <summary>
    /// Removes an object from the list of active battle objects
    /// </summary>
    /// <param name="obj"></param>
    public void UnregisterObject(BattleObject obj)
    {
        objects.Remove(obj);
        battleCamera?.removeFollow(obj.transform);
        if (fighters.Contains(obj.GetAbstractFighter())){
            fighters.Remove(obj.GetAbstractFighter());
        }
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

    public void FighterDies(AbstractFighter fighter, AbstractFighter killer){
        int fighterNum = fighter.GetIntVar(TussleConstants.FighterVariableNames.PLAYER_NUM);
        FighterResults fighterResults = resultsDict[fighterNum];
        if (!BattleLoader.current_loader.stockInfinity){
            fighterResults.stocks -= 1;
            if (fighterResults.stocks <= 0){
                UnregisterObject(fighter.getBattleObject());
                //If there's one or less players left, end the battle
                if (fighters.Count <= 1){
                    EndBattle();
                }
            } else {
                fighter.Respawn();
            }
        }
        fighterResults.falls += 1;
        fighterResults.score -= 1;
        
        //If the killer is null, it's a self destruct and we need to adjust the score as such
        if (killer == null){ 
            fighterResults.score -= 1;
            fighterResults.selfDestructs += 1;

            fighterResults.deathsAgainst[fighterNum]+=1;
            fighterResults.killsAgainst[fighterNum]+=1;
        } else {
            int killerNum = killer.GetIntVar(TussleConstants.FighterVariableNames.PLAYER_NUM);
            FighterResults killerResults = resultsDict[killerNum];
            
            killerResults.score += 1;
            killerResults.killsAgainst[fighterNum] += 1;
            fighterResults.deathsAgainst[killerNum] += 1;
        }
    }

    private void EndBattle(){
        frameDelay = 2;
        ScreenDisplayText.enabled = true;
        ScreenDisplayText.text = "GAME SET";
        StartCoroutine(EndGameSlowDown());
    }

    private IEnumerator EndGameSlowDown()
    {
        yield return new WaitForSeconds(1);
        if (frameDelay < 5){
            frameDelay++;
            StartCoroutine(EndGameSlowDown());
        } else {
            Debug.Log("Game End");
            frameDelay = 1;
            UpdateOnFrame = false;

            //Go to the results screen
            GameObject resultsObject = new GameObject("Results Screen");
            DontDestroyOnLoad(resultsObject);
            ResultsScreen results = resultsObject.AddComponent<ResultsScreen>();
            for (int i=0;i<3;i++){
                if (resultsDict.ContainsKey(i)){
                    results.AddFighterResult(resultsDict[i]);
                } else {
                    results.AddFighterResult(null);
                }
            }

            SceneManager.LoadScene("Results", LoadSceneMode.Single);
        }
    }
}