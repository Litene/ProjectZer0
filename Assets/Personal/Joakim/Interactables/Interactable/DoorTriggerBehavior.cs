using Interactable.Door;
using UnityEngine;

public class DoorTriggerBehavior : MonoBehaviour {
   private Door _door;

   private void Awake() {
       _door = GetComponentInParent<Door>();
   }

   private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            if (UnityEngine.Input.GetKeyDown(KeyCode.E)) {
                _door.TryToOpenDoor();
            }
        }
    }
}
