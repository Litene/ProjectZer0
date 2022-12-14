using UnityEngine;

[CreateAssetMenu(fileName = "ItemPickup", menuName = "ItemPickup/Item", order = 1)]
public class Item : ScriptableObject {
    
    //item types here are just examples, feel free to change/add
    public enum itemType {
        keyItem,
        journalItem,
        consumable
    }
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
}
