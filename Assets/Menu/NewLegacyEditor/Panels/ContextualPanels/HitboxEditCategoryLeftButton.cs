using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxEditCategoryLeftButton : MonoBehaviour
{
    public GameObject selectedSprite;
    public GameObject unselectedSprite;
    public CreateHitboxEditContextPanel.HitboxEditorCategory hitboxCategory;

    private UILabel label;

    private void OnEnable()
    {
        label = GetComponentInChildren<UILabel>();
        OnContextualPanelChanged();
    }

    private void Start()
    {
        label.text = CreateHitboxEditContextPanel.StringFromCategory(hitboxCategory);
        HitboxContextChangeCategory legacyAction = ScriptableObject.CreateInstance<HitboxContextChangeCategory>();
        legacyAction.init(hitboxCategory);
        GetComponent<OnClickSendAction>().action = legacyAction;
    }

    void OnContextualPanelChanged()
    {
        //Only execute if it's the right kind of contextual panel
        if (ContextualPanelData.isOfType(typeof(CreateHitboxEditContextPanel)))
        {
            CreateHitboxEditContextPanel panel = (CreateHitboxEditContextPanel)LegacyEditorData.contextualPanel;
            if (panel.selectedCategory == hitboxCategory)
            {
                NGUITools.SetActive(selectedSprite, true);
                NGUITools.SetActive(unselectedSprite, false);
            }
            else
            {
                NGUITools.SetActive(selectedSprite, false);
                NGUITools.SetActive(unselectedSprite, true);
            }
        }
    }
}
