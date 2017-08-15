using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitRig : MonoBehaviour {
    public int count = 1;
    private List<SelectorPanel> panels = new List<SelectorPanel>();

    private Dictionary<Vector2, SelectorPanel> panelsByLoc = new Dictionary<Vector2, SelectorPanel>();
    
	// Use this for initialization
	void Awake () {
        foreach (Transform child in transform)
        {
            SelectorPanel panel = child.GetComponent<SelectorPanel>();
            panels.Add(panel);
            panel.GridLoc.x = panel.transform.localPosition.x;
            panel.GridLoc.y = panel.transform.localPosition.y;
            panelsByLoc[panel.GridLoc] = panel;
        }
    }

    // Update is called once per frame
    void Update () {

    }
    public SelectorPanel GetPanel(int index)
    {
        return panels[index];
    }

    public SelectorPanel GetPanel(Vector2 gridLoc)
    {
        return panelsByLoc[gridLoc];
    }

    public void AddPanel(FighterInfo info)
    {
        SelectorPanel panel = panels[count];
        panel.SetPortrait(info);
        count++;
    }

    public FighterInfo GetRandomFighter()
    {
        int r = Random.Range(1, count);
        return GetPanel(r).fighter_info;
    }
}