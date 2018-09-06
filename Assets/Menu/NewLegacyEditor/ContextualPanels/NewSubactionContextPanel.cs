using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSubactionContextPanel : ContextualPanelData
{
    #region Selected Type - Which Subaction Type is currently selected
    private string _selectedType;
    public bool selectedTypeDirty;
    public string selectedCategory
    {
        get { return _selectedType; }
        set
        {
            _selectedType = value;
            selectedTypeDirty = true;
        }
    }
    #endregion


}
