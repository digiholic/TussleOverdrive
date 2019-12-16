using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The SpawnPoint mainly serves as a quick way to get specific locations on the map to spawn a fighter.
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    private static List<SpawnPoint> spawnPoints = new List<SpawnPoint>(){ null, null, null, null };

    public int playerIndex = 0;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        //Whenever the game starts, this spawn point registers itself as the spawn point for a player index
        spawnPoints[playerIndex] = this;
    }

    /// <summary>
    /// Get the spawn point associated with a given player index
    /// </summary>
    /// <param name="playerIndex"></param>
    /// <returns></returns>
    public static SpawnPoint getSpawnPointForPlayer(int playerIndex){
        return spawnPoints[playerIndex];
    }

    public static Transform getSpawnLocationForPlayer(int playerIndex){
        return spawnPoints[playerIndex].transform;
    }
}
