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
}
