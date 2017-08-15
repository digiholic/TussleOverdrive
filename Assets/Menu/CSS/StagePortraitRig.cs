using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePortraitRig : MonoBehaviour {
    public int count = 1;
    private List<StageSelectorPanel> panels = new List<StageSelectorPanel>();

    private Dictionary<Vector2, StageSelectorPanel> panelsByLoc = new Dictionary<Vector2, StageSelectorPanel>();

    // Use this for initialization
    void Awake()
    {
        foreach (Transform child in transform)
        {
            StageSelectorPanel panel = child.GetComponent<StageSelectorPanel>();
            panels.Add(panel);

            panel.GridLoc.x = panel.transform.localPosition.x;
            panel.GridLoc.y = panel.transform.localPosition.y;
            panelsByLoc[panel.GridLoc] = panel;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public StageSelectorPanel GetPanel(int index)
    {
        return panels[index];
    }

    public StageSelectorPanel GetPanel(Vector2 gridLoc)
    {
        if (panelsByLoc.ContainsKey(gridLoc))
            return panelsByLoc[gridLoc];
        else
            return null;
    }

    public void AddPanel(StageInfo info)
    {
        StageSelectorPanel panel = panels[count];
        panel.SetPortrait(info);
        count++;
    }

    public StageInfo GetRandomStage()
    {
        int r = Random.Range(1, count);
        return GetPanel(r).stage_info;
    }
}
