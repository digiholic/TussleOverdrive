using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLoader : MonoBehaviour {
    public static BattleLoader current_loader;

    public FighterInfo[] fighters = new FighterInfo[4];

    public int stockCount;
    public int timeCount;
    public bool teams;

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
            Destroy(gameObject); //Destroy the new one
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
