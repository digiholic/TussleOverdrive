using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionVarDataRig : LegacyEditorWidget {
    public GameObject varDataCardPrefab;
    public GameObject deleteButtonPrefab;

    private List<GameObject> children = new List<GameObject>();
    private UIGrid grid;
    private UIScrollView scrollPanel;

	// Use this for initialization
	void Awake () {
        grid = GetComponent<UIGrid>();
        scrollPanel = GetComponentInParent<UIScrollView>();
    }

    void OnSubactionChanged(SubactionData subaction)
    {
        SubactionData sub = LegacyEditorData.instance.currentSubaction;

        //Since we want to clear the list if we deselect a subaction, we take this part out of the null check
        foreach (GameObject child in children)
        {
            NGUITools.Destroy(child);
        }
        children.Clear();

        if (sub != null)
        {
            foreach (SubactionVarData varData in sub.arguments.GetItems())
            {
                InstantiateSubactionVarDataCard(varData,subaction);
            }
        }

        InstantiateDeleteButton();

        scrollPanel.ResetPosition();
        grid.Reposition();
    }
    
    void InstantiateSubactionVarDataCard(SubactionVarData varData,SubactionData subaction)
    {
        GameObject go = NGUITools.AddChild(gameObject, varDataCardPrefab);
        SubactionVarDataPanel panel = go.GetComponent<SubactionVarDataPanel>();
        panel.varData = varData;
        panel.BroadcastMessage("OnSubactionChanged",subaction,SendMessageOptions.DontRequireReceiver);
        children.Add(go);
    }

    void InstantiateDeleteButton()
    {
        GameObject go = NGUITools.AddChild(gameObject, deleteButtonPrefab);
        children.Add(go);
    }

    public override void RegisterListeners()
    {
        editor.CurrentSubactionChangedEvent += OnSubactionChanged;
    }

    public override void UnregisterListeners()
    {
        editor.CurrentSubactionChangedEvent -= OnSubactionChanged;
    }
}
