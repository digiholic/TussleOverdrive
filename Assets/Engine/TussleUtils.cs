using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TussleUtils
{
    
    public static int ParseInt(string s){
        return ParseInt(s,0);
    }

    public static int ParseInt(string s, int def){
        int result = def;
        if (int.TryParse(s, out result)){
            return result;
        } else {
            return def;
        }
    }

    public static float ParseFloat(string s){
        return ParseFloat(s,0f);
    }

    public static float ParseFloat(string s, float def){
        float result = def;
        if (float.TryParse(s, out result)){
            return result;
        } else {
            return def;
        }
    }

    public static bool ParseBool(string s){
        return ParseBool(s,false);
    }

    public static bool ParseBool(string s, bool def){
        bool result = def;
        if (bool.TryParse(s, out result)){
            return result;
        } else {
            return def;
        }
    }
}
