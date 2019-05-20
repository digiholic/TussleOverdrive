using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectIfVar : MonoBehaviour
{
    public GameObject objectToToggle;
    [Tooltip("If this is checked, any source will count to enable the object")]
    public bool anySource = false;
    [Tooltip("If this is checked, any source except the given one will count to enable the object")]
    public bool notSource = false;
    public SubactionSource source;

    [Tooltip("If this is checked, any type will count to enable the object")]
    public bool anyType = false;
    [Tooltip("If this is checked, any type except the given one will count to enable the object")]
    public bool notType = false;
    public SubactionVarType type;

    [Tooltip("If this is checked, the object will hide itself if these are valid instead of showing itself if these are valid")]
    public bool hideIfTrue = false;

    private SubactionVarDataPanel panel;

    // Start is called before the first frame update
    void Awake()
    {
        panel = GetComponent<SubactionVarDataPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        //Start with a true, and unset it if either of the checked data is false
        bool sourceValid = true;
        if (!anySource)
        {
            //This will fire if notSource and source match. You might have to think about it a bit, but this makes perfect sense. Trust me.
            if (notSource == (panel.varData.source == source))
            {
                sourceValid = false;
            }
        }
        bool typeValid = true;
        if (!anyType)
        {
            if (notType == (panel.varData.type == type))
            {
                typeValid = false;
            }
        }
        //There's a cooler way to do this with logic I'm sure but I don't wanna think about it rn I forgot I had to do this
        bool valid = sourceValid && typeValid;
        if (hideIfTrue) valid = !valid;
        //NGUITools.SetActive(objectToToggle, valid);
        objectToToggle.SetActive(valid);
    }
}
