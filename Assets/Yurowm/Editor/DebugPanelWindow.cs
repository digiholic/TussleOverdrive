using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class DebugPanelWindow : EditorWindow {

	// Parameters
	float viewportScale = 800;
	bool viewportSettings = false;
	bool ignoreDeflog = false;

	// Arrays
	Dictionary<string, bool> categories = new Dictionary<string, bool>();
	List<string> keys = new List<string>();

	// Colors
	Color defaultColor;
	Color systemColor = new Color (0.2f, 0.8f, 0.4f, 1);
	Color warningColor = new Color (0.6f, 0.6f, 0.2f, 1);
	Color errorColor = new Color (0.8f, 0.2f, 0.2f, 1);

	// Scroll positions
	Vector2 scrollViewPosition = Vector2.zero;
	Vector2 infoScrollViewPosition = Vector2.zero;

	// Auto-size field temp veriables
	float titleWidth = 100;
	float fieldWidth = 100;
	float categoryWidth = 100;
	float tWidth = 70;
	float fWidth = 100;
	float cWidth = 100;

	// Info for stack trace panel
	string info = "";
	Rect oscRect = new Rect();

	// Styles
	GUIStyle infoAreaStyle;
	GUIStyle fieldTextStyle;
	GUIStyle categoryStyle;
	GUIStyle valueTextStyle;
	GUIStyle traceButtonStyle;

	// Temp
	GUIContent content = new GUIContent();
	float width;
	float cursor = 0;

	// OnCreate
	DebugPanelWindow () {
		valueTextStyle = new GUIStyle ();
		valueTextStyle.wordWrap = true;
		valueTextStyle.fontStyle = FontStyle.Bold;
		
		categoryStyle = new GUIStyle ();
		categoryStyle.alignment = TextAnchor.MiddleLeft;
		categoryStyle.normal.background = Texture2D.whiteTexture;
		categoryStyle.padding = new RectOffset (1, 1, 1, 1);
		
		fieldTextStyle = new GUIStyle ();
		fieldTextStyle.padding = new RectOffset (0, 0, -7, 0);
		
		infoAreaStyle = new GUIStyle ();
		infoAreaStyle.normal.textColor = Color.white;
		infoAreaStyle.normal.background = Texture2D.whiteTexture;
		infoAreaStyle.focused = infoAreaStyle.normal;
		infoAreaStyle.wordWrap = true;
		infoAreaStyle.padding = new RectOffset (3, 3, 3, 3);

		traceButtonStyle = new GUIStyle ();
		traceButtonStyle.normal.textColor = Color.white;
		traceButtonStyle.wordWrap = true;
		traceButtonStyle.fontStyle = FontStyle.Bold;
		traceButtonStyle.alignment = TextAnchor.MiddleCenter;
	}
	
	void OnInspectorUpdate () {
		keys = new List<string>(DebugPanel.main.categories.Keys);
		foreach (string key in keys)
			if (!categories.ContainsKey(key))
				categories.Add(key, true);
		Repaint ();
	}


	void OnGUI () {
		DrawToolbar ();
		DrawFields ();
		DrawInfo ();
		DrawViewportSettings ();
	}

	void DrawToolbar ()
	{
		defaultColor = GUI.backgroundColor;
		EditorGUILayout.BeginHorizontal (EditorStyles.toolbar);
		if (GUILayout.Button ("Clear", EditorStyles.toolbarButton, GUILayout.Width (40))) {
			DebugPanel.Clear();
		}
		if (GUILayout.Button ("Show all", EditorStyles.toolbarButton, GUILayout.Width (60))) {
			keys = new List<string>(categories.Keys);
			foreach(string key in keys)
				categories[key] = true;
		}
		if (GUILayout.Button ("Hide all", EditorStyles.toolbarButton, GUILayout.Width (60))) {
			keys = new List<string>(categories.Keys);
			foreach(string key in keys)
				categories[key] = false;
		}
		GUILayout.FlexibleSpace ();

		if (ignoreDeflog) GUI.backgroundColor = Color.Lerp (defaultColor, Color.black, 0.3f);
		if (GUILayout.Button ("Ignore Deflog", EditorStyles.toolbarButton, GUILayout.Width (80))) {
			ignoreDeflog = !ignoreDeflog;
		}
		GUI.backgroundColor = defaultColor;

		if (viewportSettings) GUI.backgroundColor = Color.Lerp (defaultColor, Color.black, 0.3f);
		if (GUILayout.Button ("Viewport Settings", EditorStyles.toolbarButton, GUILayout.Width (100))) {
			viewportSettings = !viewportSettings;
		}
		GUI.backgroundColor = defaultColor;

		EditorGUILayout.EndHorizontal ();
	}

	void DrawFields ()
	{
		defaultColor = GUI.backgroundColor;
		scrollViewPosition = EditorGUILayout.BeginScrollView (scrollViewPosition, EditorStyles.textField);
		
		GUI.backgroundColor = Color.black;
		EditorGUILayout.BeginVertical(EditorStyles.textArea, GUILayout.ExpandHeight(true));
		GUI.backgroundColor = defaultColor;
		
		// Fields header
		EditorGUILayout.BeginHorizontal(fieldTextStyle);
		
		fieldTextStyle.fontStyle = FontStyle.Bold;
		fieldTextStyle.normal.textColor = Color.white;
		fieldTextStyle.alignment = TextAnchor.MiddleLeft;
		GUILayout.Space(20);
		EditorGUILayout.LabelField("Title", fieldTextStyle, GUILayout.Width(titleWidth));
		EditorGUILayout.LabelField("Value", fieldTextStyle, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
		EditorGUILayout.LabelField("Category", fieldTextStyle, GUILayout.Width(categoryWidth));
		
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(2);
		
		// Minimal width
		tWidth = 70;
		fWidth = 100;
		cWidth = 100;
		
		// Fields
		
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
		DrawFieldsOfCategory ("");
		foreach (string key in keys) {
			if (ignoreDeflog && DebugPanel.IsDeflog(key)) continue;
			if (key == "") continue;
			DrawFieldHeader(key);
			if (!categories[key]) continue;
			DrawFieldsOfCategory(key);
			
		}
		EditorGUILayout.EndVertical();
		
		// Calculating widthes
		titleWidth += (tWidth - titleWidth) * 0.8f;
		fieldWidth += (fWidth - fieldWidth) * 0.8f;
		categoryWidth += (cWidth - categoryWidth) * 0.8f;
		
		GUI.backgroundColor = defaultColor;
		EditorGUILayout.EndScrollView ();
	}

	void DrawInfo ()
	{
		if (info == "") return;

		EditorGUILayout.BeginHorizontal (EditorStyles.toolbar);
		GUILayout.Label ("Stack Trace");
		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("X", EditorStyles.toolbarButton, GUILayout.Width (20))) {
			info = "";
			EditorGUILayout.EndHorizontal ();
			return;
		}	
		EditorGUILayout.EndHorizontal ();

		defaultColor = GUI.backgroundColor;
		infoScrollViewPosition = EditorGUILayout.BeginScrollView (infoScrollViewPosition, EditorStyles.textField,
		                                                          GUILayout.Height(Mathf.Min(150, 10 + infoAreaStyle.CalcHeight(new GUIContent(info), position.width))));

		GUI.backgroundColor = new Color (0,0,0,0.8f);
		EditorGUILayout.TextArea (info, infoAreaStyle, GUILayout.ExpandHeight(true));
		EditorGUILayout.EndScrollView ();
		GUI.backgroundColor = defaultColor;
	}

	void DrawViewportSettings ()
	{
		if (!viewportSettings) return;
		EditorGUILayout.BeginHorizontal (EditorStyles.toolbar);
		GUILayout.Label ("Viewport Settings");
		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("X", EditorStyles.toolbarButton, GUILayout.Width (20))) {
			viewportSettings = false;
			EditorGUILayout.EndHorizontal ();
			return;
		}	
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.BeginVertical();

		DebugPanel.main.visible = GUILayout.Toggle (DebugPanel.main.visible, "Show on game screen");

		GUI.enabled = DebugPanel.main.visible;

		DebugPanel.main.hideOnAwake = GUILayout.Toggle (DebugPanel.main.hideOnAwake, "Lock panel on awake");
		DebugPanel.main.hideInEditMode = GUILayout.Toggle (DebugPanel.main.hideInEditMode, "Hide in edit mode");
		DebugPanel.main.ignoreDefLogByDefault = GUILayout.Toggle (DebugPanel.main.ignoreDefLogByDefault, "Ignore Deflog by default");

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Locked background alpha multiplier");
		DebugPanel.main.lockAlphaMultiplier = EditorGUILayout.Slider(DebugPanel.main.lockAlphaMultiplier, 0.1f, 1.0f);
		EditorGUILayout.EndHorizontal ();

		DebugPanel.main.bgColor = EditorGUILayout.ColorField ("Background Color", DebugPanel.main.bgColor);

		GUI.enabled = true;

		GUILayout.Space(10);

		EditorGUILayout.EndVertical();
	}

	void DrawFieldsOfCategory (string key)
	{
		foreach (KeyValuePair<string, DebugPanel.Field> pair in DebugPanel.main.parameters)
			if (pair.Value.category == key)
				DrawField(pair.Value.name);
	}

	void DrawFieldHeader (string key) {
		defaultColor = GUI.backgroundColor;
		switch (key) {
		case "System": GUI.backgroundColor = systemColor; break;
		case "Warning": GUI.backgroundColor = warningColor; break;
		case "Error": GUI.backgroundColor = errorColor; break;
		default: GUI.backgroundColor = Color.gray; break;
		}
		categoryStyle.normal.textColor = Color.black;

		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button ((categories [key] ? ((char)8212).ToString() : "+") + " " + key, categoryStyle))
			categories [key] = !categories [key];

		EditorGUILayout.EndHorizontal();
		GUI.backgroundColor = defaultColor;
		GUILayout.Space(3);
	}

	void DrawField (string key)
	{
		switch (DebugPanel.main.parameters[key].category) {
			case "System": fieldTextStyle.normal.textColor = systemColor; break;
			case "Warning": fieldTextStyle.normal.textColor = warningColor; break;
			case "Error": fieldTextStyle.normal.textColor = errorColor; break;
			default: fieldTextStyle.normal.textColor = Color.white; break;
		}
		valueTextStyle.normal.textColor = fieldTextStyle.normal.textColor;
		traceButtonStyle.normal.textColor = fieldTextStyle.normal.textColor;

		EditorGUILayout.BeginHorizontal();

		if (DebugPanel.main.parameters[key].trace != "") {
			if (GUILayout.Button("?", traceButtonStyle, GUILayout.Width(20)))
				info = DebugPanel.main.parameters[key].trace;
		} else {
			GUILayout.Space(20);
		}

		content.text = key;
		fieldTextStyle.fontStyle = FontStyle.Normal;
		tWidth = Mathf.Clamp(tWidth, fieldTextStyle.CalcSize(content).x, 300);
		EditorGUILayout.LabelField(content, fieldTextStyle, GUILayout.Width(titleWidth));

		content.text = DebugPanel.main.parameters[key].value;
		fWidth = Mathf.Max(fWidth, valueTextStyle.CalcSize(content).x);
		EditorGUILayout.LabelField(DebugPanel.main.parameters[key].value, valueTextStyle, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));

		content.text = DebugPanel.main.parameters[key].category;
		fieldTextStyle.fontStyle = FontStyle.Normal;
		cWidth = Mathf.Max(cWidth, fieldTextStyle.CalcSize(content).x);
		EditorGUILayout.LabelField(content, fieldTextStyle, GUILayout.Width(categoryWidth));
	
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(5);
	}

	void OnEnable () {
		Vector2 min = new Vector2 (400, 300);
		minSize = min;
	}

	[MenuItem ("Window/Debug Panel")]
	public static void GetWindow () {
		EditorWindow.GetWindow<DebugPanelWindow> ("Debug Panel");
	}
}
