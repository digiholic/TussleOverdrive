using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgumentDataRow : MonoBehaviour {
    public SubactionVarData data;

    public UILabel argname;
    public UILabel source;
    public UILabel vartype;
    public UILabel value;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateArgument(SubactionVarData argData)
    {
        data = argData;
        UpdateText();
    }

    public void UpdateText()
    {
        if (data.editable)
        {
            //TODO make some of these from UILabels to an object that can turn on or off it's editability if this is a required argument
        }

        argname.text = data.name;
        source.text = data.source;
        vartype.text = data.type;
        value.text = data.data;
    }
}
