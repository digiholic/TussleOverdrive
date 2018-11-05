using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionVarDataPanel : MonoBehaviour {
    public SubactionVarData varData;

	// Use this for initialization
	void Start () {
        varData = new SubactionVarData("TestName", SubactionSource.CONSTANT, SubactionVarType.STRING, "TestData");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
