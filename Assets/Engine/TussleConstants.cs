using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TussleConstants {

    public class FighterAttributes
    {
        public static string WEIGHT                  = "weight";
        public static string GRAVITY                 = "gravity";
        public static string MAX_FALL_SPEED          = "max_fall_speed";
        public static string MAX_GROUND_SPEED        = "max_ground_speed";
        public static string RUN_SPEED               = "run_speed";
        public static string MAX_AIR_SPEED           = "max_air_speed";
        public static string CRAWL_SPEED             = "crawl_speed";
        public static string DODGE_SPEED             = "dodge_speed";
        public static string FRICTION                = "friction";
        public static string STATIC_GRIP             = "static_grip";
        public static string PIVOT_GRIP              = "pivot_grip";
        public static string AIR_RESISTANCE          = "air_resistance";
        public static string AIR_CONTROL             = "air_control";
        public static string JUMP_HEIGHT             = "jump_height";
        public static string SHORT_HOP_HEIGHT        = "short_hop_height";
        public static string AIR_JUMP_HEIGHT         = "air_jump_height";
        public static string FASTFALL_MULTIPLIER     = "fastfall_multiplier";
        public static string HITSTUN_ELASTICITY      = "hitstun_elasticity";
        public static string SHIELD_SIZE             = "shield_size";
        public static string AERIAL_TRANSITION_SPEED = "aerial_transition_speed";
        public static string PIXELS_PER_UNIT         = "pixels_per_unit";
        public static string MAX_JUMPS               = "max_jumps";
        public static string HEAVY_LANDING_LAG       = "heavy_land_lag";
        public static string WAVEDASH_LAG            = "wavedash_lag";
    }

    public class FighterVariableNames
    {
        public static string PLAYER_NUM = "player_num";
        public static string JUMPS_REMAINING = "jumps";
        public static string FACING_DIRECTION = "facing";
        public static string LANDING_LAG = "landing_lag";
        public static string TECH_WINDOW = "tech_window";
        public static string AIR_DODGES_REMAINING = "air_dodges";
        public static string IS_GROUNDED = "grounded";
        public static string ELASTICITY = "elasticity";
    }

    public class MotionVariableNames
    {
        public static string XSPEED = "XSpeed";
        public static string YSPEED = "YSpeed";
        public static string XPREF = "XPref";
        public static string YPREF = "YPref";
    }

    public class SpriteVariableNames
    {
        public static string SPRITE_CURRENT = "currentSprite";
        public static string FRAME_CURRENT = "currentFrame";
        public static string PIXELS_PER_UNIT = "PixelsPerUnit";
    }
}
