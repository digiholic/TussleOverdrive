using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleController : MonoBehaviour {
    public List<AbstractFighter> fighters;
    public int current_game_frame = 0;

    public static BattleController current_battle;


    /// <summary>
    /// Singleton code. Will destroy any superfluous battle controllers that are in the scenes it loads into.
    /// When the battle processing is done, this object should be destroyed to make room for a new battle.
    /// </summary>
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (current_battle == null) //if we don't have a settings object
        {
            current_battle = this;
        }
        else //if it's already set
        {
            Destroy(gameObject); //Destroy the new one
        }
    }

    // Use this for initialization
    void Start()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Fighters"), LayerMask.NameToLayer("Fighters"), true);
    }

    // Update is called once per frame
    void Update () {
        current_game_frame++;
        
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

    /// <summary>
    /// Gets the fighter with the given player number from the list.
    /// </summary>
    /// <param name="playerNum">The number of the player to to find</param>
    /// <returns>A fighter with the given player number, or null if none is found</returns>
    public AbstractFighter GetFighter(int playerNum)
    {
        foreach (AbstractFighter fighter in fighters)
        {
            if (fighter.player_num == playerNum)
                return fighter;
        }
        return null;
    }
}
