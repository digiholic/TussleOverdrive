using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGridCell : MonoBehaviour {

    [SerializeField]
    private int _height;
    public int height {
        get { return _height; }
        set {
            _height = value;
            if (bg != null)
                bg.transform.localScale = new Vector3(bg.transform.localScale.x, value, bg.transform.localScale.z);
            owner.Reposition();
        }
    }

    public DynamicGridLayout owner { get; set; }
    public UISprite bg;
}
