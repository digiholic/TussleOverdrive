using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionVarDataRig : MonoBehaviour {
    public GameObject varDataCardPrefab;
    public GameObject deleteButtonPrefab;

    private List<GameObject> children = new List<GameObject>();
    private UIGrid grid;
    [SerializeField]
    private UIDraggablePanel dragPanel;
	// Use this for initialization
	void Awake () {
        grid = GetComponent<UIGrid>();
	}

    public void OnModelChanged()
    {
        if (LegacyEditorData.instance.currentSubactionDirty)
        {
            Debug.Log("Loading new Subaction");
            SubactionData sub = LegacyEditorData.instance.currentSubaction;

            //Since we want to clear the list if we deselect a subaction, we take this part out of the null check
            foreach (GameObject child in children)
            {
                NGUITools.Destroy(child);
            }
            children.Clear();

            if (sub != null)
            {
                foreach(SubactionVarData varData in sub.arguments.GetItems())
                {
                    InstantiateSubactionVarDataCard(varData);
                }
            }

            InstantiateDeleteButton();

            grid.Reposition();
            dragPanel.ResetPosition();
        }
    }
    
    void InstantiateSubactionVarDataCard(SubactionVarData varData)
    {
        GameObject go = NGUITools.AddChild(gameObject, varDataCardPrefab);
        SubactionVarDataPanel panel = go.GetComponent<SubactionVarDataPanel>();
        panel.varData = varData;
        children.Add(go);
    }

    void InstantiateDeleteButton()
    {
        GameObject go = NGUITools.AddChild(gameObject, deleteButtonPrefab);
        children.Add(go);
    }
}
