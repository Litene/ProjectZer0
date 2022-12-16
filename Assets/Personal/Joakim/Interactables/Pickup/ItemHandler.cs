using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour{
    public List<GameObject> ItemsInRange = new List<GameObject>();
    
    private void Update() {
        SortListByDistance();
        if (ItemsInRange.Count > 0) {
            UIManager.Instance.ShowPickUpInfo();
            UIManager.Instance.itemName =
                ItemsInRange[0].GetComponent<Interactable>().itemPickup.itemName;
        }
        else {
            UIManager.Instance.HidePickUpInfo();
        }
    }

    void SortListByDistance() {
        ItemsInRange = ItemsInRange.OrderBy(
            x => Vector3.Distance(transform.position, x.transform.position)).ToList();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Pickup") && other.GetComponent<Interactable>().isPickup) {
            var itemToAdd = other.gameObject;
            ItemsInRange.Add(itemToAdd);
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Pickup") && other.GetComponent<Interactable>().isPickup) {
            var itemToRemove = other.gameObject;
            ItemsInRange.Remove(itemToRemove);
        }
    }
}
