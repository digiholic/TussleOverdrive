using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubactionButton : MonoBehaviour
{
    public SubactionData subaction;

    [SerializeField] private TextMeshProUGUI text;

    private void Update()
    {
        text.text = subaction.ToString();
    }
}
