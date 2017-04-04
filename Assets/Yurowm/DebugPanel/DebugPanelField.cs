using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugPanelField : MonoBehaviour {

	Text valueT;
	Text titleT;
	Color mColor = Color.white;
	
	public Color color {
		set {
			mColor = value;
			if (valueT != null)
				valueT.color = mColor;
			if (titleT != null)
				titleT.color = mColor;
		}
		get {
			return mColor;
		}
	}

	void Start() {
		HorizontalLayoutGroup hLayout = gameObject.AddComponent<HorizontalLayoutGroup> ();
		hLayout.padding = new RectOffset (5, 5, 0, 0);
		hLayout.childForceExpandHeight = false;
		hLayout.childForceExpandWidth = false;
		hLayout.transform.localScale = new Vector3 (1, 1, 1);
//
		titleT = (new GameObject ()).AddComponent<Text> ();
		titleT.transform.SetParent(transform);
		titleT.name = "Title";
		titleT.color = color;
		titleT.font = DebugPanelViewport.arial;
		titleT.text = name;
		titleT.fontStyle = FontStyle.Bold;
		titleT.fontSize = 14;
		titleT.transform.localScale = new Vector3 (1, 1, 1);
//
		LayoutElement lElement = titleT.gameObject.AddComponent<LayoutElement> ();
		lElement.minWidth = 100;
//
		valueT = (new GameObject ()).AddComponent<Text> ();
		valueT.transform.SetParent(transform);
		valueT.name = "Value";
		valueT.color = color;
		valueT.fontSize = 14;
		valueT.font = DebugPanelViewport.arial;
		valueT.transform.localScale = new Vector3 (1, 1, 1);
	}

	void Update () {
		if (!valueT) return;
		if (DebugPanel.main.parameters.ContainsKey(name) && !(DebugPanel.main.ignoreDefLog && DebugPanel.IsDeflog(DebugPanel.main.parameters [name].category)) && DebugPanel.main.categories[DebugPanel.main.parameters [name].category])
			valueT.text = DebugPanel.main.parameters [name].value;
		else
			DebugPanelViewport.RemoveField(name);
	}
}