using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "Inventory", order = 1)]
[System.Serializable] public class InventorySO : ScriptableObject {
    public List<Item> Inventory = new List<Item>();
}
