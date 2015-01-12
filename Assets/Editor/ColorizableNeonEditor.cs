using UnityEditor;

[CustomEditor(typeof(ColorizableNeon))]
public class ColorizableNeonEditor : Editor {

	public override void OnInspectorGUI(){
		base.OnInspectorGUI();
		ColorizableNeon colorizableNeon = (ColorizableNeon) target;
		colorizableNeon.Color = EditorGUILayout.ColorField("Neon color", colorizableNeon.Color);
		EditorUtility.SetDirty(target);
	}
}
