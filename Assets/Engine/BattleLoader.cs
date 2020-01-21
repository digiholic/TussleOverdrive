using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLoader : MonoBehaviour {
    public static BattleLoader current_loader;

    public List<FighterInfo> fighters = new List<FighterInfo>();
    public string[] fighter_strings;

    public bool stockInfinity;
    public int stockCount;
    public bool timeInfinity;
    public int timeCount;
    public bool teams;

    [SerializeField]
    private GameObject FighterPrefab;
    [SerializeField]
    private Transform[] spawnPoints = new Transform[4];

    /// <summary>
    /// Singleton code. Will destroy any superfluous battle controllers that are in the scenes it loads into.
    /// When the battle processing is done, this object should be destroyed to make room for a new battle.
    /// </summary>
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (current_loader == null) //if we don't have a settings object
        {
            current_loader = this;
        }
        else //if it's already set
        {
            current_loader.spawnPoints = spawnPoints; //Set the spawn points to this one's
            Destroy(gameObject); //Destroy the new one
        }

        for (int i = 0; i < fighter_strings.Length; i++)
        {
            if (fighter_strings[i] != "")
            {
                FighterInfo info = FighterInfo.LoadFighterInfoFile(fighter_strings[i]);
                if (info != null)
                {
                    fighters.Add(info);
                }
            }
        }
    }


    public void LoadBattle() { 
        for (int i = 0; i < fighters.Count; i++)
        { 
            if (fighters[i] != null && fighters[i].directory_name != null)
            {
                FighterPrefab.GetComponent<FighterInfoLoader>().SetFighterInfo(fighters[i]);
                
                GameObject fighter = Instantiate(FighterPrefab);
                fighter.SendMessage("SetPlayerNum", i);
                fighter.SendMessage("SetFighterInfo", fighters[i]);
                fighter.transform.position = spawnPoints[i].position;
                CameraControl3D.current_camera.follows.Add(fighter.transform);
            }
        }
	}

    public void ClearBattle()
    {
        fighters = new List<FighterInfo>() { null, null, null, null };
        fighter_strings = new string[0];
    }

    public void addFighter(FighterInfo info, int player_num){
        //Make sure there's room for this fighter by adding nulls in the between slots if necessary
        while(fighters.Count < player_num+1){
            fighters.Add(null);
        }
        fighters[player_num] = info;
    }

}
