#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowOnlyAttribute))]
public class ShowOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property,
                                    GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        GUI.Box(position, "");
        GUI.enabled = false;
        EditorGUI.PropertyField(position, prop, label);
        GUI.enabled = true;
    }
}
#endif