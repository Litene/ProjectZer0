using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Vane))]
public class VaneEditor : Editor
{
    float size = 2f;

    protected virtual void OnSceneGUI()
    {
        if (Event.current.type == EventType.Repaint)
        {
            Transform transform = ((Vane)target).transform;
            var position = transform.position - transform.forward * size / 2f;
            Handles.color = Color.white;
            Handles.ArrowHandleCap(
                0,
                position,
                transform.rotation * Quaternion.LookRotation(Vector3.forward),
                size,
                EventType.Repaint
            );
        }
    }
}