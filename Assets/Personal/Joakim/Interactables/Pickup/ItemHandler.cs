using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour{
    public List<GameObject> itemsInRange = new List<GameObject>();
    
    private void Update() {
        SortListByDistance();
        if (itemsInRange.Count > 0) {
            UIManager.Instance.ShowPickUpInfo();
            UIManager.Instance.itemName =
                itemsInRange[0].GetComponent<Interactable>().itemPickup.itemName;
        }
        else {
            UIManager.Instance.HidePickUpInfo();
        }
    }

    void SortListByDistance() {
        itemsInRange = itemsInRange.OrderBy(
            x => Vector3.Distance(transform.position, x.transform.position)).ToList();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Pickup") && other.GetComponent<Interactable>().isPickup) {
            var itemToAdd = other.gameObject;
            itemsInRange.Add(itemToAdd);
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Pickup") && other.GetComponent<Interactable>().isPickup) {
            var itemToRemove = other.gameObject;
            itemsInRange.Remove(itemToRemove);
        }
    }
}
