using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CustomTagAttribute))]
public class CustomTagAttDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        //string valueStr;
        if (prop.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, prop.name + " must be a String Type");
            return;
        }


        string[] tags = (attribute as CustomTagAttribute).tags;

        if(tags == null || tags.Length == 0)
        {
            EditorGUI.LabelField(position, prop.name + " must have tag elements defined in its CustomTagAttr");
            return;
        }

        EditorGUI.LabelField(position, prop.name);

        if (string.IsNullOrEmpty(prop.stringValue)) prop.stringValue = tags[0];

        //find current index
        int currIndex = -1;
        for (int i = 0; i < tags.Length; i++)
        {
            if(tags[i] == prop.stringValue)
            {
                currIndex = i;
            }
        }

        currIndex = EditorGUI.Popup(new Rect(position.x + (position.width * .5f), position.y, position.width * .5f, position.height), currIndex, tags);

        if (currIndex >= tags.Length) currIndex = 0;
        prop.stringValue = tags[currIndex];
    }
}