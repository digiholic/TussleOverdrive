using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VarDataRig : MonoBehaviour
{
    public VarDataRow templateRow;

    private List<VarDataRow> instantiatedRows = new List<VarDataRow>();

    void OnSubactionSelected(SubactionData data)
    {
        instantiatedRows.ForEach(row => Destroy(row.gameObject));
        instantiatedRows.Clear();

        if (data != null)
        {
            foreach (SubactionVarData varData in data.arguments.Values)
            {
                VarDataRow row = Instantiate(templateRow, transform);
                instantiatedRows.Add(row);
                row.setVarData(varData);
                row.gameObject.SetActive(true);
            }
        }
    }

    private void OnEnable()
    {
        LESelectedSubaction.OnSubactionSelected += OnSubactionSelected;
        //This is enabled first when we select something so we need to pass that message through on enable. We'll listen for changes if the subaction changes while it's already still open
        OnSubactionSelected(LESelectedSubaction.instance.CurrentSubaction);
    }

    private void OnDisable()
    {
        LESelectedSubaction.OnSubactionSelected -= OnSubactionSelected;
    }
}
