using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Battle Object serves as a sort of roadmap of components that can make up an object.
/// The best way to think of things is to think of anything that changes in a scene as a BattleObject that has
/// multiple BattleComponents. These components determine how the object behaves. They can be everything from
/// fighters to projectiles to stage hazards.
/// </summary>
/// 
public class BattleObject : MonoBehaviour {
    
    /* Each component has a public accessor that will route commands to the right objects for the purposes of reading data,
     * but most methods should be called via the BroadcastMessage function, so that it could potentially hit multiple Components.
     */
    private ActionHandler actionHandler;
    private MotionHandler motionHandler;

    // Use this for initialization
    void Start () {

    }
	
	// ManualUpdate is called once per frame by the calling object
	public void ManualUpdate () {
        
    }

    public void Update()
    {
        
    }

    /// <summary>
    /// Returns true if the object has an action handler
    /// </summary>
    /// <returns>Whether the ActionHandler is set up</returns>
    public bool HasActionHandler()
    {
        return (actionHandler != null);
    }

    /// <summary>
    /// Gets the Action Handler component of the object.
    /// </summary>
    /// <returns>The Action Handler component of the Object</returns>
    public ActionHandler GetActionHandler()
    {
        return actionHandler;
    }

    /// <summary>
    /// Returns true if the object has amotion handler
    /// </summary>
    /// <returns>Whether the MotionHandler is set up</returns>
    public bool HasMotionHandler()
    {
        return (motionHandler != null);
    }

    /// <summary>
    /// Gets the Motion Handler component of the object.
    /// </summary>
    /// <returns>The Motion Handler component of the Object</returns>
    public MotionHandler GetMotionHandler()
    {
        return motionHandler;
    }
}
