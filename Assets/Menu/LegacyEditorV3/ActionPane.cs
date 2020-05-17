using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionPane : MonoBehaviour
{
    public DynamicAction action;

    [SerializeField] private TextMeshProUGUI buttonText;

    // Start is called before the first frame update
    void Start()
    {
        buttonText.text = action.name;
    }
}
