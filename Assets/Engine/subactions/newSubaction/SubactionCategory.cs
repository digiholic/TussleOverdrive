using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionGroup {
    public static string SETUP = "Set Up";
    public static string TEARDOWN = "Tear Down";
    public static string STATETRANSITION = "Transitions";
    public static string ONFRAME(int frameNo) { return "frame_" + frameNo.ToString(); }
    public static string BEFORE = "Before Frame";
    public static string AFTER = "After Frame";
    public static string LAST = "Last Frame";

    public static string[] CATEGORY_NAMES = new string[] { "Set Up", "Tear Down", "Transitions", "Before Frame", "Current Frame", "After Frame", "Last Frame" };
}
