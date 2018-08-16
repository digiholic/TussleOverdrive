using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyEditorConstants {
    public static string[] LeftDropdownOptions = new string[] {
        "Actions", "Methods", "Sprites", "Animations"
        };

    public static string[] RightDropdownOptions = new string[] {
        "Properties"
        };

    public static Dictionary<string, string[]> RightDropdownOptionsDict = new Dictionary<string, string[]>
    {
        { "Actions",    new string[] { "Properties", "SetUp", "TearDown", "Frame" } },
        { "Methods",    new string[] { "" } },
        { "Sprites",    new string[] { "Cropping", "Anchors" } },
        { "Animations", new string[] { "Frame" } }
    };
}