using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionGroup {
    public static string SETUP = "set_up";
    public static string TEARDOWN = "tear_down";
    public static string STATETRANSITION = "state_transitions";
    public static string ONFRAME(int frameNo) { return "frame_" + frameNo.ToString(); }

    public static string[] CATEGORY_NAMES = new string[] { "Set Up", "Tear Down", "Transitions", "Current Frame" };
}
