using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickNavigate : ButtonOnClick {
    public string childMenu;

	public override void OnClick()
    {
        if (childMenu != "")
            gameObject.SendMessageUpwards("SwitchToString", childMenu);
    }
}
