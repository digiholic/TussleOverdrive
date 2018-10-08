using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSubactionContextPanel : ContextualPanelData
{
    #region Selected Type - Which Subaction Type is currently selected
    [SerializeField]
    private SubactionType _selectedType;
    public bool selectedTypeDirty { get; private set; }
    public SubactionType selectedCategory
    {
        get { return _selectedType; }
        set
        {
            _selectedType = value;
            selectedTypeDirty = true;
        }
    }
    #endregion

    public void OnModelChanged()
    {
        if (LegacyEditorData.instance.currentSubactionDirty)
        {
            if (LegacyEditorData.instance.currentSubaction == null)
            {
                LegacyEditorData.Unbanish(gameObject);
            }
            else
            {
                LegacyEditorData.Banish(gameObject);
            }
        }
    }

    public override void FireContextualPanelChange()
    {
        BroadcastMessage("OnContextualPanelChanged");

        //After the broadcast, clear all the "dirty" bits
        selectedTypeDirty = false;
    }
}
