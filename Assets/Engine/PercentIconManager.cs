using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentIconManager : MonoBehaviour {
    private List<PercentIcon> icons = new List<PercentIcon>();
    public List<PercentIcon> Icons {get; set;}

    public PercentIcon iconPrefab;
    
    private int screen_width; //Used to decide when to redraw icons
    private Canvas canv;

    // Use this for initialization
    void Awake () {
        screen_width = Screen.width;
        canv = FindObjectOfType<Canvas>();
        
        //Resources res = Resources.Load("Fighters");
        //Debug.Log(res);
    }

    void LoadFighterIcons()
    {
        //First, clear all existing icons
        PercentIcon[] existingIcons = FindObjectsOfType<PercentIcon>();
        foreach (PercentIcon icon in existingIcons)
        {
            icons.Remove(icon);
            Destroy(icon.gameObject);
        }
            

        //Then, reload
        foreach (AbstractFighter fighter in BattleController.current_battle.GetFighters())
        {
            PercentIcon icon = Instantiate(iconPrefab);
            icon.fighter = fighter;
            icon.transform.SetParent(canv.transform, false);
            icons.Add(icon);
        }

        int num = 1;
        float width = canv.GetComponent<RectTransform>().rect.width;
        float dist = width / (BattleController.current_battle.GetFighters().Count + 1);
        
        foreach (PercentIcon icon in icons)
        {
            icon.transform.position = new Vector3(num * dist, 64, 0);
            num++;
        }
    }
    // Update is called once per frame
    void Update () {
        if (screen_width != Screen.width)
        {
            screen_width = Screen.width;
            LoadFighterIcons();
        }
    }
}
