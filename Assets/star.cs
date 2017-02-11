using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {
    private Light mylight;
    private Material mat;
    private SpriteRenderer sprite;
    private float speed = -50.0f;

    private starSpawner spawner;
    private Color hsvColor;

	// Use this for initialization
	void Start () {
        mylight = GetComponent<Light>();
        //mat = GetComponent<Renderer>().material;
        sprite = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        hsvColor = spawner.getColor();

        mylight.color = hsvColor;
        sprite.color = hsvColor;

        transform.Translate(speed * Time.deltaTime, 0, 0);
	}

    public void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    
    public void setSpawner(starSpawner _spawner)
    {
        spawner = _spawner;
    }
}
