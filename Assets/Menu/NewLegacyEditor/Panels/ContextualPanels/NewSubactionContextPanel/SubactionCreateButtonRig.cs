using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionCreateButtonRig : MonoBehaviour {
    public GameObject subactionButtonPrefab;

    private Dictionary<SubactionType, List<SubactionDataDefault>> subactionsByCategory = new Dictionary<SubactionType, List<SubactionDataDefault>>();
    private List<GameObject> children = new List<GameObject>();
    private UIGrid grid;
    public UIScrollView dragPanel;

    void Awake()
    {
        grid = GetComponent<UIGrid>();
        SubactionDataDefault[] data = Resources.LoadAll<SubactionDataDefault>("SubactionData");
        foreach (SubactionDataDefault sub in data)
        {
            if (!subactionsByCategory.ContainsKey(sub.subType))
                subactionsByCategory[sub.subType] = new List<SubactionDataDefault>();

            subactionsByCategory[sub.subType].Add(sub);
        }
    }

    void OnContextualPanelChanged()
    {
        //Only execute if it's the right kind of contextual panel
        if (ContextualPanelData.isOfType(typeof(NewSubactionContextPanel)))
        {
            NewSubactionContextPanel panel = (NewSubactionContextPanel)LegacyEditorData.contextualPanel;
            if (panel.selectedTypeDirty)
            {
                //Clear away all the old buttons
                foreach(GameObject child in children)
                {
                    NGUITools.Destroy(child);
                }
                children.Clear();

                foreach(SubactionDataDefault subData in subactionsByCategory[panel.selectedType])
                {
                    instantiateSubactionButton(subData);
                }

                //Realign the grid
                grid.Reposition();
                dragPanel.ResetPosition();
            }
        }
    }

    void instantiateSubactionButton(SubactionDataDefault subData)
    {
        GameObject go = NGUITools.AddChild(gameObject, subactionButtonPrefab);
        SubactionCreateButton button = go.GetComponent<SubactionCreateButton>();
        button.SetAction(subData);
        children.Add(go);
    }
}
