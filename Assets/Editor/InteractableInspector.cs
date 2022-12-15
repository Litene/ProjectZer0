using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Interactable)), CanEditMultipleObjects]
public class InteractableInspector : Editor {

    public SerializedProperty
        state_property,
        keyPressHint_property,
        itemPickup_property,
        textHint_property;

    private void OnEnable() {
        state_property = serializedObject.FindProperty ("itemType");
        textHint_property = serializedObject.FindProperty("hintText");
        keyPressHint_property = serializedObject.FindProperty("keyPressHint");
        itemPickup_property = serializedObject.FindProperty("itemPickup");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(state_property);
        Interactable.InteractableType itemType = (Interactable.InteractableType)state_property.enumValueIndex;

        switch (itemType) {
            case Interactable.InteractableType.HintTextOnly:
                EditorGUILayout.PropertyField(textHint_property, new GUIContent("hintText"));
                break;
            case Interactable.InteractableType.KeyPressHint:
                EditorGUILayout.PropertyField(keyPressHint_property, new GUIContent("keyPressHint"));
                break;
            case Interactable.InteractableType.PickUp:
                EditorGUILayout.PropertyField(itemPickup_property, new GUIContent("itemPickup"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}