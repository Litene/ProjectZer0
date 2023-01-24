using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SoundTrigger)), CanEditMultipleObjects]
public class SoundTriggerInspector : Editor {
    

    public SerializedProperty
        state_property,
        audioName_property,
        audioPosition_property;

    private void OnEnable() {
        state_property = serializedObject.FindProperty("SoundType");
        audioName_property = serializedObject.FindProperty("audioName");
        audioPosition_property = serializedObject.FindProperty("audioPosition");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(state_property);
        SoundTrigger.soundType SoundType = (SoundTrigger.soundType)state_property.enumValueIndex;

        switch (SoundType) {
            case SoundTrigger.soundType.localSound:
                EditorGUILayout.PropertyField(audioName_property, new GUIContent("audioName"));
                break;
            case SoundTrigger.soundType.worldSound:
                EditorGUILayout.PropertyField(audioName_property, new GUIContent("audioName"));
                EditorGUILayout.PropertyField(audioPosition_property, new GUIContent("audioPosition"));
                break;
            case SoundTrigger.soundType.music:
                EditorGUILayout.PropertyField(audioName_property, new GUIContent("audioName"));
                break;
            case SoundTrigger.soundType.ambience:
                EditorGUILayout.PropertyField(audioName_property, new GUIContent("audioName"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
