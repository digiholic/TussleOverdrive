using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSubactionContextPanel : ContextualPanelData
{
    #region Selected Type - Which Subaction Type is currently selected
    [SerializeField]
    private SubactionType _selectedType = SubactionType.CONTROL;
    public bool selectedTypeDirty { get; private set; }
    public SubactionType selectedType
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
        bool leftValid = false;
        bool rightValid = false;
        bool subValid = false;

        if (LegacyEditorData.instance.leftDropdown.Equals("Actions"))
        {
            leftValid = true;
        }
        if (LegacyEditorData.instance.rightDropdown.Equals("Subactions"))
        {
            rightValid = true;
        }
        if (LegacyEditorData.instance.currentSubaction == null)
        {
            subValid = true;
        }
            
        if (leftValid && rightValid && subValid) {
            selectedTypeDirty = true;
            ActivatePanel();
        } else {
            DeactivatePanel();
        }
    }
    public void Start()
    {
        
    }

    public override void FireContextualPanelChange()
    {
        BroadcastMessage("OnContextualPanelChanged");

        //After the broadcast, clear all the "dirty" bits
        selectedTypeDirty = false;
    }
}
