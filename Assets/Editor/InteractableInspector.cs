/*
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Interactable)), CanEditMultipleObjects]
public class InteractableInspector : Editor {
    public SerializedProperty
        state_property,
        keyPressHint_property,
        itemPickup_property,
        textHint_property,
        isLocked_property,
        keyItem_property,
        doorType_property;

    private void OnEnable() {
        state_property = serializedObject.FindProperty ("TypeOfInteractable");
        doorType_property = serializedObject.FindProperty("_DoorType");
        textHint_property = serializedObject.FindProperty("HintText");
        keyPressHint_property = serializedObject.FindProperty("KeyPressHintText");
        itemPickup_property = serializedObject.FindProperty("ItemPickup");
        isLocked_property = serializedObject.FindProperty("IsLocked");
        keyItem_property = serializedObject.FindProperty("KeyItem");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(state_property);
        InteractableType itemType = (InteractableType)state_property.enumValueIndex;
        Interactable.DoorType doorType =
            (Interactable.DoorType)doorType_property.enumValueIndex;

        switch (itemType) {
            case InteractableType.Door:
                EditorGUILayout.PropertyField(keyPressHint_property, new GUIContent("KeyPressHintText"));
                EditorGUILayout.PropertyField(doorType_property);
                EditorGUILayout.PropertyField(isLocked_property, new GUIContent("IsLocked"));
                if (isLocked_property.boolValue) {
                    EditorGUILayout.PropertyField(keyItem_property, new GUIContent("KeyItem"));
                }
                break;
            case InteractableType.PickUp:
                EditorGUILayout.PropertyField(itemPickup_property, new GUIContent("ItemPickup"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
*/
