using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickOpenFighter : ButtonOnClick {
    public string xml_data;

    public override void OnClick()
    {
        gameObject.SendMessageUpwards("SwitchToString", "FighterStats");
        GameObject fighterScreen = GameObject.Find("FighterStatsScreen");
        fighterScreen.GetComponent<FighterPanel>().ChangeFighter(xml_data);
    }
}
