using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LESelectedSubaction : MonoBehaviour
{
    public static LESelectedSubaction instance;

    public delegate void SubactionSelectDelegate(SubactionData newSub);
    public static event SubactionSelectDelegate OnSubactionSelected;

    [SerializeField] private SubactionData currentSubaction;
    public SubactionData CurrentSubaction
    {
        get => currentSubaction; set
        {
            currentSubaction = value;
            OnSubactionSelected?.Invoke(currentSubaction);
        }
    }

    [SerializeField] private string subactionName;

    public string SubactionName { get; private set; }

    private void OnEnable()
    {
        instance = this;
    }

    public void SelectSubaction(SubactionData newSubaction)
    {
        CurrentSubaction = newSubaction;
        if (newSubaction == null)
        {
            SubactionName = "";
        }
        else
        {
            SubactionName = newSubaction.SubactionName;
        }
    }
}
