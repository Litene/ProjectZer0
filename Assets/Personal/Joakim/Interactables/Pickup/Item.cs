using UnityEngine;

[CreateAssetMenu(fileName = "ItemPickup", menuName = "ItemPickup/Item", order = 1)]
[System.Serializable] public class Item : ScriptableObject {
    public enum ItemType {
        keyItem,
        journalItem,
        consumable
    }
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public bool destroyOnUse = false;
    public bool isDisabled;
    public override string ToString() {
        return itemName;
    }
}
