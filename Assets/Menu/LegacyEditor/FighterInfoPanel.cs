using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterInfoPanel : MonoBehaviour {

    void ChangeName(string text)
    {
        LegacyEditor.editor.current_fighter.display_name = text;
    }
}
