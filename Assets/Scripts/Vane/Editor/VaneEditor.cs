using UnityEditor;
using UnityEngine;

namespace Vane.Editor {
    [CustomEditor(typeof(Vane))]
    public sealed class VaneEditor : UnityEditor.Editor {
        private const float SIZE = 2f;

        private void OnSceneGUI() {
            if (Event.current.type != EventType.Repaint) return;
            var transform = ((Vane)target).transform;
            var position = transform.position - transform.forward * SIZE / 2f;
            Handles.color = Color.black;
            Handles.ArrowHandleCap(
                0,
                position,
                transform.rotation * Quaternion.LookRotation(Vector3.forward),
                SIZE,
                EventType.Repaint
            );
        }
    }
}