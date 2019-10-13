using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxDisplayMessenger : MonoBehaviour
{
    public Hitbox hitbox;
    public ViewerBoxDisplayer displayer;

    public void displayHitbox(Hitbox hbox){
        hitbox = hbox;
        displayer.boxRect = hbox.hitboxRect;
    }
}
