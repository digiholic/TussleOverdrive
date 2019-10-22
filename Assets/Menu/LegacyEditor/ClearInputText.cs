using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearInputText : MonoBehaviour
{
    public void ClearInput(UIInput inputToClear){
        inputToClear.Set("",true);
    }
}
