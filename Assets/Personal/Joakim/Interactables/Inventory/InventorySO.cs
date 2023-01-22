using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "Inventory", order = 1)]
public class InventorySO : ScriptableObject {
    public List<Item> Inventory = new List<Item>();
}
