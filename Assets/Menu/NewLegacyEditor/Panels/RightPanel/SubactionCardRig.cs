using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionCardRig : MonoBehaviour {
    public GameObject SubactionCardPrefab;

    private List<GameObject> children = new List<GameObject>();
    private UIGrid grid;
    [SerializeField]
    private UIDraggablePanel dragPanel;

    // Use this for initialization
    void Awake() {
        grid = GetComponent<UIGrid>();	
	}

    void OnModelChanged()
    {
        if (LegacyEditorData.instance.currentActionDirty || LegacyEditorData.instance.subactionGroupDirty)
        {
            //Get rid of our old list
            foreach (GameObject child in children)
            {
                NGUITools.Destroy(child);
            }
            children.Clear(); //Empty the list for future use

            //Create all the new buttons
            DynamicAction action = LegacyEditorData.instance.currentAction;
            string category = LegacyEditorData.instance.subactionGroup;
            
            if (action != null)
            {
                //Create all the new buttons
                foreach (SubactionData subData in action.subactionCategories.GetIfKeyExists(category))
                {
                    instantiateButton(subData);
                }

                //Realign the grid
                grid.Reposition();
                dragPanel.ResetPosition();
            }
        }
    }

    private void instantiateButton(SubactionData subDataToSet)
    {
        GameObject go = NGUITools.AddChild(gameObject, SubactionCardPrefab);
        SubactionCard card = go.GetComponent<SubactionCard>();
        card.SetSubaction(subDataToSet);
        children.Add(go);
    }
}
