using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSubactionButtonRig : MonoBehaviour
{
    public NewSubactionButton subactionButtonPrefab;

    private List<NewSubactionButton> myButtons = new List<NewSubactionButton>();

    //Can't null an enum. Don't want to make an "All" type, since that'd confuse a lot of other UI elements. So we set this bool if we're using all types
    private bool allTypes = true;
    private SubactionType currentType = SubactionType.CONTROL;
    private string searchText = "";

    public bool AllTypes {
        get => allTypes; set
        {
            allTypes = value;
            Filter();
        }
    }
    public SubactionType CurrentType
    {
        get => currentType; set
        {
            currentType = value;
            AllTypes = false;
            Filter();
        }
    }
    public string SearchText { get => searchText; set
        {
            searchText = value;
            Filter();
        }
    }

    private void Start()
    {
        foreach (SubactionDataDefault subData in SubactionDataDefaultManager.AllSubactions)
        {
            NewSubactionButton newButton = Instantiate(subactionButtonPrefab, transform);
            newButton.SetSubData(subData);
            newButton.gameObject.SetActive(true);
            myButtons.Add(newButton);
        }
    }

    public void Filter()
    {
        //I have been going absolutely HAM on streams lately
        //Sets each button's active state based on 1) if all types are visible OR the types match and 2) if the subaction's name contains our search text
        myButtons.ForEach(button =>
            button.gameObject.SetActive(
                (allTypes || button.subData.subType == CurrentType) && 
                button.subData.name.ToLower().Contains(SearchText.ToLower())
                )
            );
    }

    public void SetTypeByIndex(int index)
    {
        CurrentType = (SubactionType)index;
    }
}
