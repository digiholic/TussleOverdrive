using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConcreteSubaction : Subaction {
    public override void Execute(BattleObject actor, GameAction action)
    {
        Debug.Log("Hey! It works!");
    }
}
