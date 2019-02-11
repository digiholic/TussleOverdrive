using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour {
	public string LevelName = "Undefined";
	public string[] LevelMusic = new string[] { "Undefined" };
	public string Icon = "Undefined";
	public List<string[]> Scripts = new List<string[]>();
	public List<string[]> Visuals = new List<string[]>();
	public List<StageObject> Objects = new List<StageObject>();
}
