using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionVarDataSourceButton : MonoBehaviour {
    public SubactionVarDataPanel panel;
    [SerializeField]
    private UILabel label;

    //there's almost certainly a better way to do this but I don't know how
    private SubactionSource[] sources = new SubactionSource[] { SubactionSource.CONSTANT, SubactionSource.OWNER, SubactionSource.ACTION };
    private int index = 0;

    private void Awake()
    {
        index = Array.IndexOf(sources, panel.varData.source);
    }

    private void Update()
    {
        label.text = panel.varData.SourceAsString();   
    }

    public void OnSourceClicked()
    {
        index += 1;
        if (index == sources.Length) index = 0;
        //Generate action to do this later
        ChangeSubactionVarDataSource legacyAction = ScriptableObject.CreateInstance<ChangeSubactionVarDataSource>();
        legacyAction.init(panel.varData, sources[index]);
        LegacyEditorData.instance.DoAction(legacyAction);
    }
}
