using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyEditorData : MonoBehaviour {
    public FighterInfo loadedFighter;
    public ActionFile loadedActionFile;
    public GameAction currentAction;
    public BuilderLeftDropdown leftDropdown;
    public BuilderRightDropdown rightDropdown;
    public int currentFrame;
    public BuilderSelectable currentSelected;
    public string contextFighterCategory;
    public string contextSubactionCategory;
    public string actionSearchText;
    public string spriteSearchText;
    public SortOrder sortOrder;
}

public enum BuilderLeftDropdown
{
    ACTION,
    SPRITE
}

public enum BuilderRightDropdown
{
    PROPERTIES
}

public enum SortOrder
{
    NAME,
    CATEGORY
}