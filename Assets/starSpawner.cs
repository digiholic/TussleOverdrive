using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starSpawner : MonoBehaviour {
    [SerializeField] private GameObject starPrefab;
    [SerializeField] private Transform backdrop;

    private float hue = 0.0f;

    private int starCount = 0;
    private int spawnCounter = 10;

    // Use this for initialization
    void Start () {
	    	
	}
	
	// Update is called once per frame
	void Update () {
        hue = (hue + 0.1f * Time.deltaTime) % 1.0f;
        if (spawnCounter == 0 && starPrefab != null)
        {
            GameObject star = Instantiate(starPrefab) as GameObject;
            star.transform.position = new Vector3(backdrop.GetComponent<Renderer>().bounds.size.x/2, Random.Range(-10.0f,10.0f), Random.Range(2.0f, backdrop.position.z));
            star.GetComponent<star>().speed = -0.5f;
            star.GetComponent<star>().setHue(hue);
            spawnCounter = 10;
        }
        else
            spawnCounter -= 1;
    }
}
