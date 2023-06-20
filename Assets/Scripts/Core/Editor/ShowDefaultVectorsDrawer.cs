using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowDefaultVectorsAttribute))]
public class ShowDefaultVectorsDrawer : PropertyDrawer
{
    readonly string[] vector3DefaultDir = new string[]
    {
        "one",
        "zero",
        "up",
        "down",
        "right",
        "left",
        "forward",
        "back",
    };
    readonly string[] vector2DefaultDir = new string[]
    {
        "one",
        "zero",
        "up",
        "down",
        "right",
        "left",
    };

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        if (prop.propertyType == SerializedPropertyType.Vector3)
        {
            EditorGUI.LabelField(position, prop.name);
            int currentIndex = VectorToIndex(prop.vector3Value);
            currentIndex = EditorGUI.Popup(new Rect(position.x + (position.width * .5f), position.y, position.width * .5f, position.height), currentIndex, vector3DefaultDir);
            prop.vector3Value = IndexToVector(currentIndex);

        }
        else if (prop.propertyType == SerializedPropertyType.Vector2)
        {
            EditorGUI.LabelField(position, prop.name);
            int currentIndex = VectorToIndex(prop.vector2Value);
            currentIndex = EditorGUI.Popup(new Rect(position.x + (position.width * .5f), position.y, position.width * .5f, position.height), currentIndex, vector2DefaultDir);
            prop.vector2Value = IndexToVector(currentIndex);
        }
        else
        {
            EditorGUI.LabelField(position, prop.name + " must be a String Type");
            return;
        }
    }

    private int VectorToIndex(Vector3 vector)
    {
        if (vector == Vector3.zero) return 0;
        else if (vector == Vector3.one) return 1;
        else if (vector == Vector3.up) return 2;
        else if (vector == Vector3.down) return 3;
        else if (vector == Vector3.right) return 4;
        else if (vector == Vector3.left) return 5;
        else if (vector == Vector3.forward) return 6;
        else return 7;
    }
    private Vector3 IndexToVector(int vector)
    {
        if (vector == 0) return Vector3.zero;
        else if (vector == 1) return Vector3.one;
        else if (vector == 2) return Vector3.up;
        else if (vector == 3) return Vector3.down;
        else if (vector == 4) return Vector3.right;
        else if (vector == 5) return Vector3.left;
        else if (vector == 6) return Vector3.forward;
        else return Vector3.back;
    }
}
