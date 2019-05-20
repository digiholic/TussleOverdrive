using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanishablePanel : MonoBehaviour {
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
    }

    public void Unbanish()
    {
        LegacyEditorData.Unbanish(gameObject,intendedParent);
    }
}
