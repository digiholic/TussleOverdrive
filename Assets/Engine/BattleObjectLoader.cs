using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleObjectLoader : MonoBehaviour {
    public AbstractFighterLoader abstract_fighter_info;
    public SpriteHandlerLoader sprite_handler_info;
    public ActionHandlerLoader action_handler_info;
    public MotionHandlerLoader motion_handler_info;
    public EnvironmentColliderLoader ecb_info;

    public List<VarData> variables;

    public BattleObject LoadFighter(int player_num)
    {
        GameObject obj = new GameObject();
        BattleObject battleObj = obj.AddComponent<BattleObject>();
        if (abstract_fighter_info != null)
        {
            AbstractFighter fighter = obj.AddComponent<AbstractFighter>();
            fighter.fighter_name = abstract_fighter_info.fighter_name;
            fighter.franchise_icon = abstract_fighter_info.franchise_icon_path;
            fighter.css_icon = abstract_fighter_info.css_icon_path;
        }
        if (sprite_handler_info != null)
        {
            SpriteHandler sprite = obj.AddComponent<SpriteHandler>();
            sprite.directory = sprite_handler_info.sprite_directory;
            sprite.default_sprite = sprite_handler_info.default_sprite;
            sprite.prefix = sprite_handler_info.sprite_prefix;
            sprite.pixelsPerUnit = sprite_handler_info.pixels_per_unit;
            //sprite.orientation = sprite_handler_info.orientation;
        }
        if (action_handler_info != null)
        {
            ActionHandler actions = obj.AddComponent<ActionHandler>();
            actions.action_json_path = action_handler_info.action_file_path;
            //actions.action_json = action_handler_info.action_json;
            //actions.starting_action = action_handler_info.starting_action;
        }
        if (motion_handler_info != null)
        {
            MotionHandler mot = obj.AddComponent<MotionHandler>();
        }
        if (ecb_info != null)
        {
            EnvironmentCollider ecb = obj.AddComponent<EnvironmentCollider>();
        }
        foreach(VarData vardata in variables)
        {
            battleObj.SetVar(vardata.name, vardata.value);
        }
        battleObj.LoadComponents();
        return battleObj;
    }
}

[System.Serializable]
public struct VarData
{
    public string name;
    public string value;
}

[System.Serializable]
public class AbstractFighterLoader
{
    public string resource_path;

    public string fighter_name;
    public string franchise_icon_path;
    public string css_icon_path;
    public string css_portrait_path;

    //AbstractFighter
    //public float weight;
    //public int jumps;
    //public float jump_height;
    //public float short_hop_height;
    //public float air_jump_height;
    //public int heavy_land_lag;
    //public int wavedash_lag;
    //public float fastfall_multiplier;
    //public float hitstun_elasticity;
    //public float shield_size;

}

[System.Serializable]
public class SpriteHandlerLoader
{
    //Sprite Handler
    public string sprite_directory;
    public string default_sprite;
    public string sprite_prefix;
    public float pixels_per_unit;
    public string orientation;

}

[System.Serializable]
public class ActionHandlerLoader
{
    //Action Handler
    public string action_file_path;
    public string action_json;
    public string starting_action;
}

[System.Serializable]
public class MotionHandlerLoader { 
    //Motion Handler
    //public float gravity;
    //public float max_fall_speed;
    //public float max_ground_speed;
    //public float run_speed;
    //public float max_air_speed;
    //public float aerial_transition_speed;
    //public float crawl_speed;
    //public float dodge_speed;
    //public float friction;
    //public float static_grip;
    //public float pivot_grip;
    //public float air_resistance;
    //public float air_control;
}

[System.Serializable]
public class EnvironmentColliderLoader
{
    //Environment Collider
    public Vector3 ecb_center;
    public float ecb_radius;
    public float ecb_height;
}