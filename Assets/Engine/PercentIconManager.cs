using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentIconManager : MonoBehaviour {
    private List<PercentIcon> icons = new List<PercentIcon>();
    public List<PercentIcon> Icons {get; set;}

    public PercentIcon iconPrefab;
    
    private int screen_width; //Used to decide when to redraw icons

    // Use this for initialization
    void Start () {
        screen_width = Screen.width;
        Canvas canv = FindObjectOfType<Canvas>();

        foreach (AbstractFighter fighter in BattleController.current_battle.fighters)
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
        float dist = width / (BattleController.current_battle.fighters.Count + 1);

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
