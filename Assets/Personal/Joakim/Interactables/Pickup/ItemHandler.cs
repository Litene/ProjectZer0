using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour{
    public List<Item> ItemsInRange = new List<Item>();
    
    private void Update() {
        SortListByDistance();
        if (ItemsInRange.Count > 0) {
            UIManager.Instance.ShowPickUpInfo();
            UIManager.Instance.itemName =
                ItemsInRange[0].itemName;
        }
        else {
            UIManager.Instance.HidePickUpInfo();
        }

        if (ItemsInRange.Count > 0 && UnityEngine.Input.GetKeyDown(KeyCode.E)) {
            Inventory.Instance.AddItem(ItemsInRange[0]);
            ItemsInRange[0].isDisabled = true;
            ItemsInRange.RemoveAt(0);
        }
    }

    void SortListByDistance() {
        ItemsInRange = ItemsInRange.OrderBy(
            x => Vector3.Distance(transform.position, x.itemPosition)).ToList();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Pickup") && other.GetComponent<Interactable>().isPickup) {
            var itemToAdd = other.GetComponent<Interactable>().itemPickup;
            ItemsInRange.Add(itemToAdd);
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Pickup") && other.GetComponent<Interactable>().isPickup) {
            var itemToRemove = other.GetComponent<Interactable>().itemPickup;
            ItemsInRange.Remove(itemToRemove);
        }
    }
}
