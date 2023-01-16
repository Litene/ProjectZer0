using UnityEngine;

public class DoorTriggerBehavior : MonoBehaviour {
   private Interactable _interactable;

   private void Awake() {
       _interactable = GetComponentInParent<Interactable>();
   }

   private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            _interactable.LookAt();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            _interactable.LookAway();
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            if (UnityEngine.Input.GetKeyDown(KeyCode.E)) {
                _interactable.TryToOpenDoor();
            }
        }
    }
}
