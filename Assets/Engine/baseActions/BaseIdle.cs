using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseIdle : DynamicAction
{
    public List<SubactionData> stateTransitionDataList = new List<SubactionData>();

    [SerializeField]
    private List<SubactionData> subactionDataList = new List<SubactionData>()
    {
        SubactionDataDefault.GetByName("DoTransition"),
        SubactionDataDefault.GetByName("DoTransition")
    };

    public BaseIdle(): base("NeutralAction", 1, "idle", 1, false, "NeutralAction")
    {
        SubactionData transitionData = SubactionDataDefault.GetByName("DoTransition");
        transitionData.SetArgument("transitionState", "NeutralState");
        subactionCategories.Add(SubactionGroup.STATETRANSITION, stateTransitionDataList);
    }
    
}
