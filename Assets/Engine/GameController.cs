using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public List<AbstractFighter> fighters;
    public List<PercentIcon> icons;

    public PercentIcon iconPrefab;

    private bool recording = false;
    [SerializeField] private Camera cam;

    public int current_game_frame = 0;

    int screen_width; //Used to decide when to redraw icons

    // Use this for initialization
    void Start () {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Fighters"), LayerMask.NameToLayer("Fighters"), true);
        screen_width = Screen.width;
        Canvas canv = FindObjectOfType<Canvas>();

        foreach (AbstractFighter fighter in fighters)
        {
            PercentIcon icon = Instantiate(iconPrefab);
            icon.fighter = fighter;
            icon.transform.SetParent(canv.transform, false);
            icons.Add(icon);
        }
        LoadFighterIcons();
        //Resources res = Resources.Load("Fighters");
        //Debug.Log(res);
    }

    void LoadFighterIcons()
    {
        int num = 1;
        Canvas canv = FindObjectOfType<Canvas>();
        float width = canv.GetComponent<RectTransform>().rect.width;
        float dist = width / (fighters.Count + 1);
        
        foreach (PercentIcon icon in icons)
        {
            icon.transform.position = new Vector3(num * dist, 64, 0);
            num++;
        }
    }

    // Update is called once per frame
    void Update () {
        current_game_frame++;
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (recording)
            {
                cam.GetComponent<Moments.Recorder>().Save();
                recording = false;
                Debug.Log("Finished recording");
            } else
            {
                cam.GetComponent<Moments.Recorder>().Record();
                recording = true;
                Debug.Log("Started recording");
            }
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
        if (screen_width != Screen.width)
        {
            screen_width = Screen.width;
            LoadFighterIcons();
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
