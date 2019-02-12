using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartsBanished : MonoBehaviour {
    public GameObject intendedParent;
    public PanelHider shadowRealm;

    // Use this for initialization
    void Start()
    {
        shadowRealm.panelToParentDict.Add(gameObject, intendedParent);
    }
    
    public void Banish()
    {
        LegacyEditorData.Banish(gameObject);
        //if it's banished, we need to make sure this component is enabled
        this.enabled = true;
    }

    public void Unbanish()
    {
        LegacyEditorData.Unbanish(gameObject);
        //if it's not starting banished, we don't need to use this component
        this.enabled = false;
    }
}
