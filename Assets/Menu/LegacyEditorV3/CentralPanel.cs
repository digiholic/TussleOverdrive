using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CentralPanel : MonoBehaviour
{
    public static bool isHovered;
    
    public void setIsHovering(bool isHovering)
    {
        isHovered = isHovering;
    }
}
