using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAutoSizer : MonoBehaviour
{
    public float leftOffset, rightOffset;
    public bool resizePanel;
    public bool resizeSprite;

    private UIPanel panel = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float width = LegacyEditorData.anchors.width - leftOffset - rightOffset;

        if (resizePanel)
        {

        }

        if (resizeSprite)
        {
            Vector3 scale = transform.localScale;
            scale.x = width;
            transform.localScale = scale;
        }
    }
}
