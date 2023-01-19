using System.Linq;
using System.Collections.Generic;
using Interactable.PickUp;
using UnityEngine;

public class ItemHandler : MonoBehaviour{
    // TODO: Add enum for Indoor / Outdoor (maybe access this from GameManager, which should get set from SceneManager).
    // TODO: Add trigger area (all game objects) (outside) and raycast (first object) (inside) which stores interactable objects.
    // TODO: Access the relevant interactable objects from PickUp, Door, Holdable, etc.
    
    
    
    
    public List<PickUp> PickUpsInRange = new List<PickUp>();
    
    private void Update() {
        SortListByDistance();
        if (PickUpsInRange.Count > 0) {
            UIManager.Instance.ShowPickUpInfo();
            UIManager.Instance.itemName =
                PickUpsInRange[0].ItemPickup.itemName;
        }
        else {
            UIManager.Instance.HidePickUpInfo();
        }

        if (PickUpsInRange.Count > 0 && UnityEngine.Input.GetKeyDown(KeyCode.E)) {
            Inventory.Instance.AddItem(PickUpsInRange[0].ItemPickup);
            PickUpsInRange[0].ItemPickup.isDisabled = true;
            PickUpsInRange.RemoveAt(0);
        }
    }

    void SortListByDistance() {
        PickUpsInRange = PickUpsInRange.OrderBy(
            x => Vector3.Distance(transform.position, x.transform.position)).ToList();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PickUp>()) {
            var pickUpToAdd = other.GetComponent<PickUp>();
            PickUpsInRange.Add(pickUpToAdd);
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<PickUp>()) {
            var pickUpToRemove = other.GetComponent<PickUp>();
            PickUpsInRange.Remove(pickUpToRemove);
        }
    }
}
