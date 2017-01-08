using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public Transform target = null;
    private GameObject bullet;
    private bool recording = false;

    [SerializeField] private Camera cam;
    [SerializeField] private GameObject bulletPrefab;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 objectPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, 10));
            objectPos.z = 0;
            bullet = Instantiate(bulletPrefab) as GameObject;
            bullet.transform.position = objectPos;
            bullet.GetComponent<launch>().target = target;
        }
        */
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (recording)
            {
                cam.GetComponent<Moments.Recorder>().Save();
                recording = false;
                Debug.Log("Finished recording");
            } else
            {
                cam.GetComponent<Moments.Recorder>().Record();
                recording = true;
                Debug.Log("Started recording");
            }
            
        }
    }
}
