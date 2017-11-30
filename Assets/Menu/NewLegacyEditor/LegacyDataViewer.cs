using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface LegacyDataViewer{

    void WindowChanged(string window_name);
    void SubWindowChanged(string sub_window_name);
    void FighterChanged(FighterInfo fighter_info);
    void ActionsChanged(ActionFile actions);
    void SelectedActionChanged(DynamicAction action);
    void CategoryChanged(string category_name);
    void SubactionGroupChanged(SubActionGroup group);

}
