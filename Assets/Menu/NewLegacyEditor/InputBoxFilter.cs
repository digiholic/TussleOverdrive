using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A helper class to filter text from inputs. The inputs will check if this component is on the object, and if they are, will apply this filter to their text before modifying a variable.
/// 
/// </summary>
public class InputBoxFilter : MonoBehaviour {
    public enum FilterType
    {
        DECIMAL,
        INT,
        NONE
    }

    public FilterType filterType;
    public string filterText(string str)
    {
        string retStr = "";
        bool decimalAllowed = true;
        if (filterType == FilterType.INT) decimalAllowed = false; //If it's gotta be an INT, then no decimals allowed
        if (filterType == FilterType.DECIMAL || filterType == FilterType.INT)
        {
            for (int i = 0; i < str.Length; i++)
            {
                //All digits are OK in my book
                if (char.IsDigit(str[i]))
                {
                    retStr += str[i];
                }
                //We can only allow the first decimal place. Any others after it will be ignored.
                if (decimalAllowed && str[i] == '.')
                {
                    decimalAllowed = false;
                    retStr += str[i];
                }
            }
        }
        else if (filterType == FilterType.NONE) retStr = str;
        return retStr;
    }
}
