using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class Inventory : Singleton<Inventory> {
    public List<Item> ItemsInInventory = new List<Item>();
    public List<GameObject> ItemSlots = new List<GameObject>();

    public void AddItem(Item item) {
        ItemsInInventory.Add(item);
        UpdateInventory();
    }

    public void RemoveItem(Item item) {
        ItemsInInventory.Remove(item);
        UpdateInventory();
    }

    public void UseItem(Item item) {
        if (item.destroyOnUse) {
            ItemsInInventory.Remove(item);
            UpdateInventory();
        }
    }

    public void SelectItem() {
        
    }

    public void UpdateInventory() {
        if (ItemsInInventory.Count > 0) {
            int index = 0;
            foreach (var Item in ItemsInInventory) { 
                ItemSlots[index].GetComponent<Image>().color = Color.white;
              ItemSlots[index].GetComponent<Image>().overrideSprite = Item.itemSprite;
              index++;
            }
            UIManager.Instance.ShowItemInventory();
        }
        else {
            UIManager.Instance.HideItemInventory();
        }
    }
}
