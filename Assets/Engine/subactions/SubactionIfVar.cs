using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CONTROL SUBACTION
/// Enters a conditional block if the conditional is met.
/// 
/// Arguments:
///     var1 - required object - The first part of our equation
///     var2 - required object - The object being compared to
///     cond - optional string (default "==") - The conditional to compare the variables with.
///         These conditionals are available for all objects:
///         == - true if both values are equal
///         != - true if both values are different
///         
///         These conditionals are available if the data are both ints or floats
///         &lt; - true if var1 is less than var2
///         &gt; - true if var1 is greater than var2
///         &lt;= - true if var1 is less than or equal to var2
///         &gt;= - true if var1 is greater than or equal to var2
///         
///         These conditionals are available if the data are both boolean
///         && - true if both values are true
///         || - true if either value is true
///          ^ - true if exactly one value is true
///         !& - true if both variables are not true
///         !| - true if both values are false
///         !^ - true if both values are the same
/// </summary>
public class SubactionIfVar : Subaction {
    public override void Execute(BattleObject obj, GameAction action)
    {
        SubactionVarData var1 = arg_dict["var1"];
        SubactionVarData var2 = arg_dict["var2"];
        string cond = (string)GetArgument("cond", obj, action, "==");

        bool value = false; //If the cond isn't set right, this subaction will always return false

        if (cond == "==") value = (var1.GetData(obj, action) == var2.GetData(obj, action));
        if (cond == "!=") value = (var1.GetData(obj, action) != var2.GetData(obj, action));

        if (var1.IsNumeric() && var2.IsNumeric()) //If they're both numeric, we can compare them
        {
            float var1data = (float) var1.GetData(obj, action);
            float var2data = (float) var2.GetData(obj, action);

            if (cond == "<") value = (var1data < var2data);
            if (cond == ">") value = (var1data > var2data);
            if (cond == "<=") value = (var1data <= var2data);
            if (cond == ">=") value = (var1data >= var2data);
        }

        if (var1.type == "bool" && var2.type == "bool")
        {
            bool var1data = (bool)var1.GetData(obj, action);
            bool var2data = (bool)var2.GetData(obj, action);

            if (cond == "&&") value = var1data && var2data;
            if (cond == "||") value = var1data || var2data;
            if (cond == "^") value = var1data ^ var2data;
            if (cond == "!&") value = !(var1data && var2data);
            if (cond == "!|") value = !(var1data || var2data);
            if (cond == "!^") value = !(var1data ^ var2data);
        }
        
        action.cond_list.Add(value);
        action.cond_depth++;
    }

    public override SubactionCategory getCategory()
    {
        return SubactionCategory.CONTROL;
    }

    public override bool isConditional()
    {
        return true;
    }
}
