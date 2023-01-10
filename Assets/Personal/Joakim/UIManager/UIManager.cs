using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : Singleton<UIManager> {
    private GameObject Canvas;
    private GameObject _pickUpItemPanel;
    private GameObject _itemInventoryPanel;
    private TextMeshProUGUI _pickUpItemText;
    public string itemName;

    private void Awake() { // null reference causes issues
        Canvas = GameObject.Find("Canvas-UI-Overlay");
        _pickUpItemPanel = Canvas.transform.Find("PickUpItemPanel").gameObject;
        _itemInventoryPanel = Canvas.transform.Find("InventoryOverlay").gameObject;
        _pickUpItemText = _pickUpItemPanel.transform.Find("PickUpText").GetComponent<TextMeshProUGUI>();
    }

    public void ShowItemInventory() {
        _itemInventoryPanel.SetActive(true);
    }

    public void HideItemInventory() {
        _itemInventoryPanel.SetActive(false);
    }

    public void ShowPickUpInfo() {
        _pickUpItemText.text = "Press [E] To Pick Up " + itemName; //proper keybound should be displayed
        _pickUpItemPanel.SetActive(true);
    }

    public void HidePickUpInfo() {
        _pickUpItemPanel.SetActive(false);
    }
}
