using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader {
	public enum TussleScene{
		SplashScreen,
		TopMenu
	}

	public static void Load(TussleScene scene){
		SceneManager.LoadScene(scene.ToString());
	}
}
