using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour {
    public enum Side { LEFT,RIGHT }
    public Side grabSide;

    private List<AbstractFighter> fighters_grabbed = new List<AbstractFighter>();
    public Vector3 hang_point;

    void Start()
    {
        hang_point = transform.GetChild(0).position;
        //TODO don't hard code this
    }

    /// <summary>
    /// Adds the fighter to this ledge's list of grabbed fighters, obeying rules set
    /// in the settings. Triggers the fighters LedgeGrab function.
    /// </summary>
    /// <param name="fighter"></param>
    void FighterGrabs(AbstractFighter fighter)
    {
        if (fighters_grabbed.Count == 0) //If there's no conflict, we just let him grab
        {
            fighters_grabbed.Add(fighter);
            fighter.SendMessage("GrabLedge", this);
        } else
        {
            SettingsPreset.ConflictType conflict = Settings.current_settings.preset.conflict_type;
            if (conflict == SettingsPreset.ConflictType.SHARE)
            {
                fighters_grabbed.Add(fighter);
                fighter.SendMessage("GrabLedge", this);
            }
            else if (conflict == SettingsPreset.ConflictType.HOG)
            {
                return; //The grabbing fighter entirely ignores the ledge if he can't grab it
            }
            else if (conflict == SettingsPreset.ConflictType.TRUMP)
            {
                foreach (AbstractFighter other in fighters_grabbed)
                {
                    FighterLeaves(other);
                    other.SendMessage("GetTrumped", this);
                }
                fighters_grabbed.Add(fighter);
                fighter.SendMessage("GrabLedge", this);
            }


        }
    }

    void FighterLeaves(AbstractFighter fighter)
    {
        if (fighters_grabbed.Contains(fighter))
        {
            fighters_grabbed.Remove(fighter);
        } else
        {
            Debug.LogWarning("Attempted to remove a fighter from a ledge when the fighter was not present.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Fighters"))
            other.SendMessage("EnterLedge", this);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Fighters"))
            other.SendMessage("ExitLedge", this);
    }
}
