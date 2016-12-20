using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class star : MonoBehaviour {

    private float h = 0.0f;
    private float s = 0.8f;
    private float v = 1.0f;

    private Light mylight;
    private Material mat;
    private SpriteRenderer sprite;
    public float speed = 0.0f;

    private int life = 100;

	// Use this for initialization
	void Start () {
        mylight = GetComponent<Light>();
        //mat = GetComponent<Renderer>().material;
        sprite = GetComponent<SpriteRenderer>();
        //speed = Random.Range(0.1f, 2.0f);
    }
	
	// Update is called once per frame
	void Update () {
        h = (h + 0.1f * Time.deltaTime) % 1.0f;
        mylight.color = Color.HSVToRGB(h, s, v);
        sprite.color = Color.HSVToRGB(h, s, v);

        transform.Translate(speed, 0, 0);
        life -= 1;
        if (life == 0)
        {
            Destroy(this.gameObject);
        }
	}

    public void setHue(float _h)
    {
        h = _h;
    }
}
