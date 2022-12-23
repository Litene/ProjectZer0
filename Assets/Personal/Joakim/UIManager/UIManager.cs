using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager> {
    private GameObject _uiManager;
    private GameObject _temporaryObject;
    public GameObject PickUpItemPanel;
    public GameObject ItemInventoryPanel;
    public TextMeshProUGUI PickUpItemText;
    
    //temp raycast
    private Ray ray;
    private RaycastHit raycasthit;

    public string itemName;

    private void Awake() {
        _uiManager ??= new GameObject {
            name = "UI Manager"
        };
    }

    public void ShowItemInventory() {
        ItemInventoryPanel.SetActive(true);
    }

    public void HideItemInventory() {
        ItemInventoryPanel.SetActive(false);
    }

    public void ShowPickUpInfo() {
        PickUpItemText.text = "Press [E] To Pick Up " + itemName; //proper keybound should be displayed
        PickUpItemPanel.SetActive(true);
    }

    public void HidePickUpInfo() {
        PickUpItemPanel.SetActive(false);
    }
    
    private void Update() {
        //Start of cursed raycast. This shouldnt be in UI Manager, but while we dont have a proper controller it makes most sense to have it here.
        ray.origin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        ray.direction = Camera.main.transform.forward;
        if (Physics.Raycast(ray, out raycasthit, 10 )) {
            if (raycasthit.collider.tag == "Interactable") {
                _temporaryObject = raycasthit.collider.gameObject;
                raycasthit.collider.GetComponent<Interactable>().LookAt();
            }
            else if(_temporaryObject != null){
                _temporaryObject.GetComponent<Interactable>().LookAway();
            }
        }
        //end of cursed raycast
    }
}
