using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starSpawner : MonoBehaviour {
    [SerializeField] private GameObject starPrefab;
    public int starTimer = 10;

    private float hue = 0.0f; //hue hue hue
    private int spawnCounter = 0;

    // Use this for initialization
    void Start () {
        spawnCounter = starTimer;
	}
	
	// Update is called once per frame
	void Update () {
        hue = (hue + 0.05f * Time.deltaTime) % 1.0f;
        if (spawnCounter == 0 && starPrefab != null)
        {
            GameObject star = Instantiate(starPrefab) as GameObject;
            //star.transform.position = new Vector3(backdrop.GetComponent<Renderer>().bounds.size.x/2, Random.Range(-10.0f,10.0f), Random.Range(2.0f, backdrop.position.z));
            star.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Random.Range(0.0f,Camera.main.pixelHeight), Random.Range(10.0f, 25.0f)));
            
            star.GetComponent<star>().speed = -0.5f;
            star.GetComponent<star>().setHue(hue);
            spawnCounter = starTimer;
        }
        else
            spawnCounter -= 1;
    }

    public float getHue()
    {
        return hue;
    }
}
