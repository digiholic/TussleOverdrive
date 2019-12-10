using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NGUIScaleFix : MonoBehaviour
{
    [MenuItem("TussleUtils/FixNGUIScale")]
    public static void FixNGUIScale(){
        UILabel[] labels = GameObject.FindObjectsOfType<UILabel>();
        UISprite[] sprites = GameObject.FindObjectsOfType<UISprite>();

        foreach (UILabel label in labels){
            Vector3 oldScale = label.transform.localScale;
            if (oldScale != Vector3.one){
                label.width = (int) oldScale.x;
                label.height = (int) oldScale.y;
                label.transform.localScale = Vector3.one;
                Debug.Log(string.Format("Modified Label {0} - Old Scale {1} - Widget Size {2}",label.name,oldScale,label.localSize),label.gameObject);
            }
        }

        foreach (UISprite sprite in sprites){
            Vector3 oldScale = sprite.transform.localScale;
            if (oldScale != Vector3.one){
                sprite.width = (int) oldScale.x;
                sprite.height = (int) oldScale.y;
                sprite.transform.localScale = Vector3.one;
                Debug.Log(string.Format("Modified Sprite {0} - Old Scale {1} - Widget Size {2}",sprite.name,oldScale,sprite.localSize),sprite.gameObject);
            }
        }
    }
}
