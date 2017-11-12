using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgumentDataRow : MonoBehaviour {
    public SubactionVarData data;

    public UIInput argname;
    public UIPopupList vartype;
    public UIPopupList source;
    public UIInput value;

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
        if (!data.editable)
        {
            argname.SendMessage("SetEditable", false);
            vartype.SendMessage("SetEditable", false);
        }
        else
        {
            argname.SendMessage("SetEditable", true);
            vartype.SendMessage("SetEditable", true);
        }

        argname.text = data.name;
        vartype.selection = data.type;
        source.selection = data.source;
        value.text = data.data;
    }
}
