using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class NewSubactionButton : MonoBehaviour
{
    public SubactionDataDefault subData;

    [SerializeField] private TextMeshProUGUI buttonText;

    private void Start()
    {
        if (buttonText == null) buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = CamelCaseToSpaced(subData.SubactionName);
    }

    public void SetSubData(SubactionDataDefault subData)
    {
        this.subData = subData;
        buttonText.text = CamelCaseToSpaced(subData.SubactionName);
    }

    public SubactionData GetSubaction()
    {
        return subData.CreateSubactionData();
    }

    public string CamelCaseToSpaced(string s)
    {
        //I found this online. It's basically witchcraft
        return Regex.Replace(s, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1");
    }
}
