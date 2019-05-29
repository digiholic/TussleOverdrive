using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionVarDataRig : LegacyEditorWidget {
    public GameObject varDataCardPrefab;
    public GameObject deleteButtonPrefab;

    private List<GameObject> children = new List<GameObject>();
    private UIGrid grid;

	// Use this for initialization
	void Awake () {
        grid = GetComponent<UIGrid>();
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
                InstantiateSubactionVarDataCard(varData);
            }
        }

        InstantiateDeleteButton();

        grid.Reposition();
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

    public override void RegisterListeners()
    {
        editor.CurrentSubactionChangedEvent += OnSubactionChanged;
    }

    public override void UnregisterListeners()
    {
        editor.CurrentSubactionChangedEvent -= OnSubactionChanged;
    }
}
