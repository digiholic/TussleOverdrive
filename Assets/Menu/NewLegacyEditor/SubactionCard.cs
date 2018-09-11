using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionCard : MonoBehaviour {
    [SerializeField]
    private UISprite selectedBg;
    [SerializeField]
    private UISprite unselectedBg;
    [SerializeField]
    private UILabel label;

    public SubactionData subaction;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        label.text = subaction.SubactionName;
	}
}
