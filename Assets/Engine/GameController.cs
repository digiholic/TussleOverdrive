using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public List<AbstractFighter> fighters;
    public PercentIcon iconPrefab;

    private bool recording = false;
    [SerializeField] private Camera cam;
    
    // Use this for initialization
    void Start () {
        int num = 1;
		foreach (AbstractFighter fighter in fighters)
        {
            PercentIcon icon = Instantiate(iconPrefab);
            icon.fighter = fighter;
            icon.transform.SetParent(FindObjectOfType<Canvas>().transform,false);
            icon.transform.position = new Vector3(num * 300, 64, 0);
            num++;
        }

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Fighters"), LayerMask.NameToLayer("Fighters"), true);
	}
	
	// Update is called once per frame
	void Update () {
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
        if (Input.GetKeyDown(KeyCode.LeftBracket))
            Application.CaptureScreenshot("screenshot");
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
