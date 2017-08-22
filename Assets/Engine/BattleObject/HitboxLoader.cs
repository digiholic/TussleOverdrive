using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HitboxLoader : BattleComponent {

    public Hitbox hitbox_prefab;

    public Hitbox LoadHitbox(AbstractFighter owner, GameAction action, Dictionary<string, string> dict)
    {
        Hitbox hbox = Instantiate(hitbox_prefab);
        hbox.owner = owner.battleObject;
        hbox.transform.parent = owner.transform;
        hbox.LoadValuesFromDict(owner, dict);

        //Flip it if the fighter is flipped
        if (owner.GetIntVar("facing") == -1)
            hbox.trajectory = 180 - hbox.trajectory;

        //Set up the hitbox lock, if applicable
        if (hbox.lock_name != "") //If it has a name, we need to check if it's got a lock already
        {
            if (action.hitbox_locks.ContainsKey(hbox.lock_name)) //If it's already there, just assign it to the hitbox
            {
                hbox.hitbox_lock = action.hitbox_locks[hbox.lock_name];
            }
            else //If it has a name, but isn't in the list, we need to add it
            {
                HitboxLock new_lock = new HitboxLock(hbox.lock_name);
                hbox.hitbox_lock = new_lock;
                action.hitbox_locks.Add(hbox.lock_name, new_lock);
            }
         }
        else //If it's unnamed, we just need to create a new lock for this hitbox
        {
            HitboxLock new_lock = new HitboxLock("GenericLockName"+action.hitbox_locks.Count.ToString());
            hbox.hitbox_lock = new_lock;
            action.hitbox_locks.Add(new_lock.name, new_lock);
        }

        return hbox;
    }
    
}
