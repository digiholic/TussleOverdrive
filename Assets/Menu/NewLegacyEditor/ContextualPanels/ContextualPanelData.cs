using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The contextual panel is similar to the LegacyEditorData on a smaller scale.
/// It has its own sets of data that it can manipulate and pass functionality
/// </summary>
public abstract class ContextualPanelData : MonoBehaviour {

    /// <summary>
    /// Calls all of the OnContextualPanelChanged methods in child objects of the contextual panel
    /// </summary>
    public abstract void FireContextualPanelChange();

}
