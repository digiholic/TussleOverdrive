using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DebugPanelViewport : MonoBehaviour {

	public static DebugPanelViewport main;

	RectTransform categoriesRect;
	RectTransform fieldsRect;
	RectTransform clearButton;
	RectTransform ignoreButton;
	RectTransform showAllButton;
	RectTransform hideAllButton;
	Image ignoreButtonImg;
	CanvasGroup panelGroup;
	CanvasGroup rootGroup;
	public static Font arial;

	bool uiLock = false;
	bool fieldsListChanged = false;
	
	Dictionary<string, Button> categories = new Dictionary<string, Button>();
	Dictionary<string, DebugPanelField> fields = new Dictionary<string, DebugPanelField>();
	List<string> keys = new List<string> ();

	// Colors
	Color systemColor = new Color (0.4f, 1f, 0.6f, 1);
	Color warningColor = new Color (0.9f, 0.9f, 0.4f, 1);
	Color errorColor = new Color (1f, 0.4f, 0.4f, 1);

	void Awake () {
		main = this;
		arial = Resources.GetBuiltinResource<Font>("Arial.ttf");
		CreateUI ();
		Lock (DebugPanel.main.hideOnAwake);
		IngorDefLog (DebugPanel.main.ignoreDefLog);
	}

	void Update () {
		UpdateFields (DebugPanel.main.parameters);
		if (!uiLock)
			UpdateCategories (DebugPanel.main.categories);
	}

	void CreateUI () {
		Vector2 controlButtonSize = new Vector2 (60, 60);

		// Root canvas
		GameObject root = new GameObject ();
		root.transform.name = "ViewportDebugPanel";
		root.transform.parent = transform;
		root.transform.SetSiblingIndex (0);
		Canvas rootCanvas = root.AddComponent<Canvas>();
		rootCanvas.sortingOrder = 32767;
		rootCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
		rootCanvas.pixelPerfect = true;
		CanvasScaler scaler = root.AddComponent<CanvasScaler> ();
		scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
		scaler.matchWidthOrHeight = 0.7f;
		scaler.referencePixelsPerUnit = 100;
		rootGroup = root.AddComponent<CanvasGroup> ();
		root.AddComponent<GraphicRaycaster> ();
		
		// Panel
		GameObject panel = new GameObject();
		panel.name = "Panel";
		panel.transform.parent = root.transform;
		ContentSizeFitter csFitter = panel.AddComponent<ContentSizeFitter> ();
		csFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
		VerticalLayoutGroup vLayout = panel.AddComponent<VerticalLayoutGroup> ();
		vLayout.childForceExpandWidth = true;
		vLayout.childForceExpandHeight = true;
		vLayout.padding = new RectOffset (0, (int) controlButtonSize.x + 5, 0, 0);
		Image background = panel.AddComponent<Image> ();
		background.sprite = null;
		background.color = DebugPanel.main.bgColor;
		panelGroup = panel.AddComponent<CanvasGroup> ();
		RectTransform rect = (RectTransform) panel.transform;
		rect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Right, 0, 0);
		rect.anchorMin = new Vector2 (0, 1);
		rect.anchorMax = new Vector2 (1, 1);
		rect.pivot = new Vector2 (0, 1);
		rect.anchoredPosition = new Vector2(0, 0);

		// Categories Layout
		GameObject categoriesLayout = new GameObject();
		categoriesLayout.name = "Categories";
		categoriesLayout.transform.parent = panel.transform;
		GridLayoutGroup gLayout = categoriesLayout.AddComponent<GridLayoutGroup>();
		gLayout.padding = new RectOffset(5, 5, 5, 5);
		gLayout.cellSize = new Vector2 (100, 40);
		gLayout.spacing = new Vector2 (4, 4);
		categoriesRect = (RectTransform)gLayout.transform;
		categoriesRect.pivot = new Vector2 (0, 1);

		// Fields Layout
		GameObject fieldsLayout = new GameObject();
		fieldsLayout.name = "Fields";
		fieldsLayout.transform.parent = panel.transform;
		vLayout = fieldsLayout.AddComponent<VerticalLayoutGroup>();
		vLayout.padding = new RectOffset (5, 5, 0, 5);
		vLayout.spacing = 3;
		vLayout.childForceExpandWidth = false;
		vLayout.childForceExpandHeight = false;
		fieldsRect = (RectTransform)vLayout.transform;
		fieldsRect.pivot = new Vector2 (0, 1);

		// Controls Layout
		GameObject controlsLayout = new GameObject();
		controlsLayout.name = "Controls";
		controlsLayout.transform.parent = root.transform;
		vLayout = controlsLayout.AddComponent<VerticalLayoutGroup> ();
		vLayout.padding = new RectOffset (5, 5, 5, 5);
		vLayout.spacing = 3;
		vLayout.childAlignment = TextAnchor.UpperRight;
		vLayout.childForceExpandHeight = false;
		vLayout.childForceExpandWidth = false;
		csFitter = controlsLayout.AddComponent<ContentSizeFitter> ();
		csFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
		csFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
		rect = (RectTransform)vLayout.transform;
		rect.pivot = new Vector2 (1, 1);
		rect.anchorMin = new Vector2 (1, 1);
		rect.anchorMax = new Vector2 (1, 1);
		rect.anchoredPosition = new Vector2 (0, 0);

		// Lock Button
		Button button = CreateButton("Lock", "LOCK", new Color(1,1,1,0.5f));
		button.transform.SetParent(controlsLayout.transform);
		rect = (RectTransform)button.transform;
		rect.pivot = new Vector2 (1, 1);
		LayoutElement lElement = button.gameObject.AddComponent<LayoutElement> ();
		lElement.minWidth = controlButtonSize.x;
		lElement.minHeight = controlButtonSize.y;
		button.onClick.AddListener(() => Lock ()); 

		// Clear Button
		button = CreateButton("Clear", "CLEAR", new Color(1,1,1,0.5f));
		button.transform.SetParent(controlsLayout.transform);
		clearButton = (RectTransform)button.transform;
		clearButton.pivot = new Vector2 (1, 1);
		lElement = button.gameObject.AddComponent<LayoutElement> ();
		lElement.minWidth = controlButtonSize.x;
		lElement.minHeight = controlButtonSize.y;
		button.onClick.AddListener(() => DebugPanel.Clear ()); 

		// Ignore Deflog Button
		button = CreateButton("IgnoreDefLog", "IGNORE\nDEFLOG", new Color(1,1,1,0.5f));
		ignoreButtonImg = button.gameObject.GetComponent<Image> ();
		button.transform.SetParent(controlsLayout.transform);
		ignoreButton = (RectTransform)button.transform;
		ignoreButton.pivot = new Vector2 (1, 1);
		lElement = button.gameObject.AddComponent<LayoutElement> ();
		lElement.minWidth = controlButtonSize.x;
		lElement.minHeight = controlButtonSize.y;
		button.onClick.AddListener(() => IngorDefLog(!DebugPanel.main.ignoreDefLog)); 

		// Show-all Button
		button = CreateButton("ShowAll", "SHOW\nALL", new Color(1,1,1,0.5f));
		button.transform.SetParent(controlsLayout.transform);
		showAllButton = (RectTransform)button.transform;
		showAllButton.pivot = new Vector2 (1, 1);
		lElement = button.gameObject.AddComponent<LayoutElement> ();
		lElement.minWidth = controlButtonSize.x;
		lElement.minHeight = controlButtonSize.y;
		button.onClick.AddListener(() => DebugPanel.TurnAll (true)); 

		// Hide-all Button
		button = CreateButton("HideAll", "HIDE\nALL", new Color(1,1,1,0.5f));
		button.transform.SetParent(controlsLayout.transform);
		hideAllButton = (RectTransform)button.transform;
		hideAllButton.pivot = new Vector2 (1, 1);
		lElement = button.gameObject.AddComponent<LayoutElement> ();
		lElement.minWidth = controlButtonSize.x;
		lElement.minHeight = controlButtonSize.y;
		button.onClick.AddListener(() => DebugPanel.TurnAll (false)); 

		Canvas.ForceUpdateCanvases ();
	}

	public void UpdateCategories(Dictionary<string, bool> target) {
		keys = new List<string> (target.Keys);
		Button button;
		Color color;
		bool changed = false;
		foreach (string key in keys) {
			if (!categories.ContainsKey(key)) {
				if (key == "") continue;
				if (DebugPanel.main.ignoreDefLog && DebugPanel.IsDeflog(key)) continue;
				color = GetColorbyCategory(key);
				color.a = 0.6f;
				button = CreateButton(key, key, color);
				button.name = key;
				button.transform.SetParent(categoriesRect.transform);
				button.transform.localScale = new Vector3(1, 1, 1);
				AddCategoryListener(button, key);
				categories.Add(key, button);
				changed = true;
			}
		}
		keys = new List<string> (categories.Keys);
		foreach (string key in keys) {
			if (!target.ContainsKey(key)) {
				Destroy(categories[key].gameObject);
				categories.Remove(key);
				changed = true;
			}
		}
		if (changed) {
			keys = new List<string> (categories.Keys);
			keys.Sort (delegate(string x, string y) {
				if (x == "Warning") return 1;
				if (y == "Warning") return -1;
				if (x == "Error") return 1;
				if (y == "Error") return -1;
				if (x == "System") return 1;
				if (y == "System") return -1;
				return x.CompareTo(y);
			});
			for (int i = 0; i < keys.Count; i++)
				categories[keys[i]].transform.SetSiblingIndex(0);
		}
	}


	void AddCategoryListener (Button button, string key) {
		button.onClick.AddListener(() => {
			DebugPanel.main.categories[key] = !DebugPanel.main.categories[key];
			Image img = button.gameObject.GetComponent<Image>();
			Color color = GetColorbyCategory(button.name);
			if (!DebugPanel.main.categories[key]) color = Color.Lerp(color, Color.black, 0.5f);
			color.a = 0.6f;
			img.color = color;
		});

	}

	Color GetColorbyCategory(string key) {
		Color color;
		switch (key) {
			case "System": color = systemColor; break;
			case "Warning": color = warningColor; break;
			case "Error": color = errorColor; break;
			default: color = Color.white; break;
		}
		return color;
	}

	void IngorDefLog (bool i) {
		DebugPanel.IngorDefLog (i);
		ignoreButtonImg.color = DebugPanel.main.ignoreDefLog ? systemColor : new Color (1, 1, 1, 0.5f);
		if (i) {
			keys = new List<string>();
			keys.Add("Error");
			keys.Add("Warning");
			foreach (string key in keys) {
				if (categories.ContainsKey(key)) {
					Destroy(categories[key].gameObject);
					categories.Remove(key);
				}
			}

		}
	}

	public void UpdateFields(Dictionary<string, DebugPanel.Field> target) {
		keys = new List<string> (target.Keys);

		foreach (string key in keys) {
			if (!fields.ContainsKey(key) && DebugPanel.main.categories[target[key].category]) {
				fields.Add(key, CreateField(key));
				fieldsListChanged = true;
			}
		}

		foreach (Transform child in fieldsRect) {
			if (!child.gameObject.activeSelf) {
				fields.Remove(child.name);
				Destroy(child.gameObject);
				fieldsListChanged = true;
			}
		}

		if (fieldsListChanged) {
			StartCoroutine("SortFields");
		}
	}

	IEnumerator SortFields() {
		yield return 0;
		keys = new List<string> (categories.Keys);
		keys.Add("");
		keys.Sort (delegate(string y, string x) {
			if (x == "Warning") return 1;
			if (y == "Warning") return -1;
			if (x == "Error") return 1;
			if (y == "Error") return -1;
			if (x == "System") return 1;
			if (y == "System") return -1;
			return x.CompareTo(y);
		});
		List<string> keys2 = new List<string>(fields.Keys);
		keys2.Sort(delegate(string x, string y) {
			return x.CompareTo(y);
		});

		int index = 0;
		foreach(string cat in keys)
			foreach(string field in keys2)
				if (DebugPanel.main.parameters[field].category == cat) {
					fields[field].transform.SetSiblingIndex(index);
					index ++;
				}

		fieldsListChanged = false;
	}
	
	public static void RemoveField (string key) {
		if (!main.fields.ContainsKey(key)) return;
		main.fields [key].gameObject.SetActive (false);
	}

	DebugPanelField CreateField(string key) {
		GameObject layout = new GameObject ();
		layout.name = key;
		layout.transform.parent = fieldsRect;
		Color color;

		switch (DebugPanel.main.parameters[key].category) {
			case "System": color = systemColor; break;
			case "Warning": color = warningColor; break;
			case "Error": color = errorColor; break;
			default: color = Color.white; break;
		}

		DebugPanelField field = layout.AddComponent<DebugPanelField> ();
		field.color = color;
		
		return field;
	}

	void Lock(bool l) {
		uiLock = l;
		clearButton.gameObject.SetActive (!l);
		showAllButton.gameObject.SetActive (!l);
		hideAllButton.gameObject.SetActive (!l);
		ignoreButton.gameObject.SetActive (!l);
		categoriesRect.gameObject.SetActive (!l);
		panelGroup.blocksRaycasts = !l;
		rootGroup.alpha = l ? DebugPanel.main.lockAlphaMultiplier : 1.0f;
	}

	void Lock() {
		Lock (!uiLock);
		}

	Button CreateButton(string buttonName, string label, Color color) {
		GameObject buttonGO = new GameObject();
		buttonGO.name = buttonName;
		Image img = buttonGO.AddComponent<Image> ();
		img.sprite = null;
		img.color = color;
		Button button = buttonGO.AddComponent<Button> ();
		GameObject labelGO = new GameObject();
		labelGO.name = "Label";
		labelGO.transform.parent = buttonGO.transform;
		Text text = labelGO.AddComponent<Text>();
		text.text = label;
		text.font = arial;
		text.fontSize = 12;
		text.color = Color.black;
		text.alignment = TextAnchor.MiddleCenter;
		RectTransform rect = (RectTransform) labelGO.transform;
		rect.anchorMin = new Vector2 (0, 0);
		rect.anchorMax = new Vector2 (1, 1);
		rect.offsetMin = new Vector2 (0, 0);
		rect.offsetMax = new Vector2 (0, 0);
		return button;
	}
}
