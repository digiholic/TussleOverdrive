using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_motion : MonoBehaviour
{

    private float minX, maxX, minY, maxY;
    public Transform[] follows;

    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void CalculateBounds()
    {
        minX = Mathf.Infinity; maxX = -Mathf.Infinity; minY = Mathf.Infinity; maxY = -Mathf.Infinity;

        foreach (Transform player in follows)
        {
            Vector3 tempPlayer = player.position;
            //X Bounds
            if (tempPlayer.x < minX)
                minX = tempPlayer.x;
            if (tempPlayer.x > maxX)
                maxX = tempPlayer.x;
            //Y Bounds
            if (tempPlayer.y < minY)
                minY = tempPlayer.y;
            if (tempPlayer.y > maxY)
                maxY = tempPlayer.y;
        }
    }

    void CalculateCameraPosAndSize()
    { //Position Vector3 cameraCenter = Vector3.zero;
        /*
        foreach (Transform player in follows)
        {
            cameraCenter += player.position;
        }
        Vector3 finalCameraCenter = cameraCenter / players.Length;
        //Rotates and Positions camera around a point
        rot = Quaternion.Euler(angles);
        pos = rot * new Vector3(0f, 0f, -camDist) + finalCameraCenter;
        transform.rotation = rot;
        transform.position = Vector3.Lerp(transform.position, pos, camSpeed * Time.deltaTime);
        finalLookAt = Vector3.Lerp(finalLookAt, finalCameraCenter, camSpeed * Time.deltaTime);
        transform.LookAt(finalLookAt);
        //Size
        float sizeX = maxX - minX + cameraBuffer.x;
        float sizeY = maxY - minY + cameraBuffer.y;
        camSize = (sizeX > sizeY ? sizeX : sizeY);
        camera.orthographicSize = camSize * 0.5f;
        */
    }

}
