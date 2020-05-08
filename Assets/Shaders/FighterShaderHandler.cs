using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterShaderHandler : MonoBehaviour
{
    private const string MAIN_TEX = "_MainTex";
    private const string COLS = "cols";
    private const string ROWS = "rows";
    private const string SUBIMAGE = "subimage";

    public Texture2D mainTex;
    public int cols;
    public int rows;
    public int subimage;

    private Material FighterMaterial;
    private Shader FighterShader;
    private Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer.material = new Material(GetFighterShader());
        FighterMaterial = renderer.material;

        FighterMaterial.SetTexture(MAIN_TEX, mainTex);
        FighterMaterial.SetInt(COLS, cols);
        FighterMaterial.SetInt(ROWS, rows);
        FighterMaterial.SetInt(SUBIMAGE, subimage);
    }

    private void Update()
    {
        FighterMaterial.SetInt(SUBIMAGE, subimage);
    }

    public static Shader GetFighterShader()
    {
        return Shader.Find("Shader Graphs/FighterShader");
    }
}
