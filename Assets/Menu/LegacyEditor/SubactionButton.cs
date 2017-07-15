using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionButton : MonoBehaviour {
    private UILabel label;
    private string subactionText;

	// Use this for initialization
	void Awake() {
        label = transform.Find("Label").GetComponent<UILabel>();
	}
	
    public void LoadSubaction(string subaction, int number)
    {
        label.text = subaction;
        subactionText = subaction;
        Vector3 pos = transform.localPosition;
        pos.y -= (40 * number);
        transform.localPosition = pos;
    }
}
