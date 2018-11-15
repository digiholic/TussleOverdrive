using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionGroup {
    public static string SETUP = "Set Up";
    public static string TEARDOWN = "Tear Down";
    public static string STATETRANSITION = "Transitions";
    public static string ONFRAME(int frameNo) { return "frame_" + frameNo.ToString(); }

    public static string[] CATEGORY_NAMES = new string[] { "Set Up", "Tear Down", "Transitions", "Current Frame" };
}
