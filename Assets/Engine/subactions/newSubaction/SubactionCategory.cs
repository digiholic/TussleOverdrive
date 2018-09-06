using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionCategory {
    public static string SETUP = "set_up";
    public static string TEARDOWN = "tear_down";
    public static string STATETRANSITION = "state_transitions";
    public static string ONFRAME(int frameNo) { return "frame_" + frameNo.ToString(); }
}
