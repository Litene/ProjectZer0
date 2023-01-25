using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class Inventory : Singleton<Inventory> {
    public List<Item> ItemsInInventory = new List<Item>();
    public List<GameObject> ItemSlots = new List<GameObject>();
    public InventorySO _inventoryData;

    private void Awake() {
        _inventoryData = Resources.Load<InventorySO>("Inventory/InventoryData");
    }

    private void Start() {
        if (_inventoryData.Inventory.Count >= 0) {
            foreach (Item item in _inventoryData.Inventory) {
                AddItem(item);
            }
        }
    }

    public void LoadInventory() {
        foreach (Item item in _inventoryData.Inventory) {
            AddItem(item);
        }
    }

    public void AddItem(Item item) {
        if (!ItemsInInventory.Contains(item)) {
            ItemsInInventory.Add(item);
            if (!_inventoryData.Inventory.Contains(item)) {
                _inventoryData.Inventory.Add(item);
            }
        }
        UpdateInventory();
    }

    public void RemoveItem(Item item) {
        ItemsInInventory.Remove(item);
        _inventoryData.Inventory.Remove(item);
        UpdateInventory();
    }

    public void UseItem(Item item) {
        if (item.destroyOnUse) {
            ItemsInInventory.Remove(item);
            _inventoryData.Inventory.Remove(item);
            UpdateInventory();
        }
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
