using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using UnityEngine.SceneManagement;
using System.Linq;

[System.Serializable]
public class Inventory : ISaveable {
    // public List<GameObject> ItemSlots;
    public InventorySO _inventoryData;
    private const int INVENTORY_SIZE = 4;

    public void Initialize() {
        // if (ItemSlots == null) {
        //     ItemSlots = Transform.FindObjectsOfType<GameObject>().ToList();
        // }
        _inventoryData = Resources.Load<InventorySO>("Inventory/InventoryData");
        SaveManager.Instance.AddListener(this);
        LoadData();
        LoadInventory();
    }

    public void LoadInventory() {
        foreach (Item item in _inventoryData.Inventory) {
            AddItem(item);
        }
    }

    public void AddItem(Item item) {
        if (!_inventoryData.Inventory.Contains(item)) {
            _inventoryData.Inventory.Add(item);
        }

        UpdateInventory();
    }

    public void RemoveItem(Item item) {
        _inventoryData.Inventory.Remove(item);
        _inventoryData.Inventory.Remove(item);
        UpdateInventory();
    }

    public void UseItem(Item item) {
        if (item.destroyOnUse) {
            _inventoryData.Inventory.Remove(item);
            _inventoryData.Inventory.Remove(item);
            UpdateInventory();
        }
    }

    public void UpdateInventory() {
        if (_inventoryData.Inventory.Count > 0) {
            int index = 0;
            foreach (var Item in _inventoryData.Inventory) {
                //   ItemSlots[index].GetComponent<Image>().color = Color.white;
                // ItemSlots[index].GetComponent<Image>().overrideSprite = Item.itemSprite;
                index++;
            }

            UIManager.Instance.ShowItemInventory();
        }
        else {
            UIManager.Instance.HideItemInventory();
        }

        SaveData();
    }

    public void LoadData() {
        var iData = SaveManager.Instance.GetSave(UID) as InventoryData;
        //if (iData != null) _inventoryData = iData.InventorySOData;
    }

    public void SaveData() {
        SaveManager.Instance.AddSave(new InventoryData()
            { InventoryNames = ToStringList(_inventoryData.Inventory), UID = this.UID });
    }

    List<string> ToStringList(List<Item> items) {
        List<string> tempList = new List<string>();
        foreach (var VARIABLE in items) {
            tempList.Add(VARIABLE.itemName);
        }

        return tempList;
    }

    public string UID { get; set; } = "Inventory";
}