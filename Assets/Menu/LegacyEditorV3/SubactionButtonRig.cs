using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionButtonRig : MonoBehaviour
{
    public SubactionButton subactionButtonTemplate;
    private List<SubactionButton> childButtons = new List<SubactionButton>();

    public void BuildPanes(List<SubactionData> subactions)
    {
        childButtons.ForEach(button => Destroy(button.gameObject));
        childButtons.Clear();
        
        foreach (SubactionData subaction in subactions)
        {
            InstantiateSubactionButton(subaction);
        }
    }

    private void InstantiateSubactionButton(SubactionData subaction)
    {
        SubactionButton newButton = Instantiate(subactionButtonTemplate, transform);
        newButton.subaction = subaction;
        newButton.gameObject.SetActive(true);
        childButtons.Add(newButton);
    }
}
