using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowOnlyAttribute))]
public class OnlyShowDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label) {
        string valueString;

        switch (prop.propertyType) {
            case SerializedPropertyType.Integer:
                valueString = prop.intValue.ToString();
                break;
            case SerializedPropertyType.Boolean:
                valueString = prop.boolValue.ToString();
                break;
            case SerializedPropertyType.Float:
                valueString = prop.floatValue.ToString();
                break;
            case SerializedPropertyType.String:
                valueString = prop.stringValue;
                break;
            case SerializedPropertyType.Color:
                valueString = prop.colorValue.ToString();
                break;
            case SerializedPropertyType.ObjectReference:
                try {
                    valueString = prop.objectReferenceValue.ToString();
                }
                catch (System.NullReferenceException) {
                    valueString = "None (GameObject)";
                }
                break;
            case SerializedPropertyType.Enum:
                valueString = prop.enumDisplayNames[prop.enumValueIndex].ToString();
                break;
            default:
                valueString = "Not supported";
                break;
        }

        EditorGUI.LabelField(position, label.text, valueString);
    }
}