using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SubactionDataDefaultManager
{
    private static List<SubactionDataDefault> _allSubactions;

    public static List<SubactionDataDefault> AllSubactions
    {
        get
        {
            if (_allSubactions == null)
                _allSubactions = new List<SubactionDataDefault>(Resources.LoadAll<SubactionDataDefault>("SubactionData"));
            return _allSubactions;
        }
    }

    private static List<SubactionDataDefault> _controlSubactions;
    public static List<SubactionDataDefault> ControlSubactions
    {
        get
        {
            if (_controlSubactions == null)
                _controlSubactions = AllSubactions.Where(sub => sub.subType == SubactionType.CONTROL).ToList();
            return _controlSubactions;
        }
    }

    private static List<SubactionDataDefault> _behaviorSubactions;
    public static List<SubactionDataDefault> BehaviorSubactions
    {
        get
        {
            if (_behaviorSubactions == null)
                _behaviorSubactions = AllSubactions.Where(sub => sub.subType == SubactionType.BEHAVIOR).ToList();
            return _behaviorSubactions;
        }
    }

    private static List<SubactionDataDefault> _animationSubactions;
    public static List<SubactionDataDefault> AnimationSubactions
    {
        get
        {
            if (_animationSubactions == null)
                _animationSubactions = AllSubactions.Where(sub => sub.subType == SubactionType.ANIMATION).ToList();
            return _animationSubactions;
        }
    }

    private static List<SubactionDataDefault> _hitboxSubactions;
    public static List<SubactionDataDefault> HitboxSubactions
    {
        get
        {
            if (_hitboxSubactions == null)
                _hitboxSubactions = AllSubactions.Where(sub => sub.subType == SubactionType.HITBOX).ToList();
            return _hitboxSubactions;
        }
    }

    private static List<SubactionDataDefault> _otherSubactions;
    public static List<SubactionDataDefault> OtherSubactions
    {
        get
        {
            if (_otherSubactions == null)
                _otherSubactions = AllSubactions.Where(sub => sub.subType == SubactionType.OTHER).ToList();
            return _otherSubactions;
        }
    }

    //This one is generic, but not cached. It'll be slower than using the specified type lists defined above, but can be useful if you don't know what type you want yet
    public static List<SubactionDataDefault> GetSubactionsOfType(SubactionType subType)
    {
        return AllSubactions.Where(sub => sub.subType == subType).ToList();
    }
}
