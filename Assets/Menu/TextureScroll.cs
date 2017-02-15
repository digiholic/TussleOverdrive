using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour {
    public int matIndex = 0;
    public Vector2 AnimRate = new Vector2(0.0f, 0.0f);
    public string textureName = "_MainTex";

    Vector2 Offset = Vector2.zero;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

	// Update is called once per frame
	void LateUpdate () {
        Offset += (AnimRate * Time.deltaTime);

        if (rend.enabled)
        {
            rend.materials[matIndex].SetTextureOffset(textureName, Offset);
        }
	}
}
