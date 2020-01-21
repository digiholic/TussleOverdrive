using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class handles the color changing of the backgrounds in the menu. It is a singleton with DontDestroyOnLoad, so it's always
/// pulsing colors. Any other object can poll this object for it's color to set itself to a matching color.
/// </summary>
public class MenuColorChanger : MonoBehaviour {
    public static MenuColorChanger menu_color;

    public float hue = 0.0f; //hue hue hue
    public Color hsvColor = Color.black;

    public List<string> excludedScenes = new List<string>();
    /// <summary>
    /// At the start of the scene, create an instance of this class if there isn't one already. If there is, get rid of this one and keep using that one.
    /// </summary>
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (menu_color == null) //if we don't have a settings object
            menu_color = this;
        
        else //if it's already set
            Destroy(gameObject); //Destroy the new one
    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        hue = (hue + 0.05f * Time.deltaTime) % 1.0f;
        hsvColor = Color.HSVToRGB(hue, 0.8f, 1.0f);
    }

    /// <summary>
    /// Get the current color of the menu
    /// </summary>
    /// <returns></returns>
    public static Color getColor()
    {
        if (menu_color != null) return menu_color.hsvColor;
        else return Color.black;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode) {
       if (excludedScenes.Contains(scene.name)){
           if (gameObject != null){
               Destroy(this.gameObject,1);
           }
       }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
