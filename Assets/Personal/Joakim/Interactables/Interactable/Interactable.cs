using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;

public class Interactable : MonoBehaviour, IInteractable {
    public Item ItemPickup;
    public bool IsPickup;
    public bool IsLocked;
    public string HintText;
    public KeyCode KeyPressHintText;
    public Item KeyItem;
    private TextMeshProUGUI _worldSpaceText;
    private GameObject _thisInteractableCanvas;
    private GameObject _playerRef;
    private GameObject _temp;
    private GameObject _doorTriggerArea;
    private bool _isDoor;

    public enum InteractableType{
        Door,
        PickUp
    }

    public enum DoorType {
        Door,
        Ventilation,
        Window
    }

    public InteractableType TypeOfInteractable;
    public DoorType _DoorType;
    
    private void Awake() {
        _playerRef = GameObject.Find("Player");
        Transform trans = transform;
        Transform canvasTrans = trans.Find("Canvas");
        if (canvasTrans != null) {
            _thisInteractableCanvas = canvasTrans.gameObject;
        }
        _worldSpaceText = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    private void Start() {
        switch (TypeOfInteractable) {
            case InteractableType.Door:
                GenerateTriggerAreaForDoor();
                _worldSpaceText.text = "[" + KeyPressHintText.ToString() + "]";
                _isDoor = true;
                break;
            case InteractableType.PickUp:
                IsPickup = true;
                break;
        }
    }

    public void TryToOpenDoor() {
        switch (_DoorType) {
                case DoorType.Door:
                    if (IsLocked && Inventory.Instance.ItemsInInventory.Contains(KeyItem)) {
                        StartCoroutine(OpenDoor());
                        Inventory.Instance.UseItem(KeyItem);
                    }
                    if (IsLocked && !Inventory.Instance.ItemsInInventory.Contains(KeyItem)) {
                        Debug.Log("key item for this door not present in inventory.");
                        return;
                    }

                    if (!IsLocked) {
                        StartCoroutine(OpenDoor());
                    }
                    break;
                case DoorType.Ventilation:
                    if (IsLocked && Inventory.Instance.ItemsInInventory.Contains(KeyItem)) {
                        StartCoroutine(OpenDoor());
                        Inventory.Instance.UseItem(KeyItem);
                    }
                    if (IsLocked && !Inventory.Instance.ItemsInInventory.Contains(KeyItem)) {
                        Debug.Log("key item for this shaft not present in inventory.");
                        return;
                    }

                    if (!IsLocked) {
                        StartCoroutine(OpenDoor());
                    }
                    break;
                case DoorType.Window:
                    if (IsLocked && Inventory.Instance.ItemsInInventory.Contains(KeyItem)) {
                        StartCoroutine(OpenDoor());
                        Inventory.Instance.UseItem(KeyItem);
                    }
                    if (IsLocked && !Inventory.Instance.ItemsInInventory.Contains(KeyItem)) {
                        Debug.Log("key item for this window not present in inventory.");
                        return;
                    }
                    if (!IsLocked) {
                        StartCoroutine(OpenDoor());
                    }
                    break;
        }
    }

    private void Update() {
        if (IsPickup && ItemPickup.isDisabled) {
            this.gameObject.SetActive(false);
        }

        if (IsPickup) {
            ItemPickup.itemPosition = transform.position;
        }
        
    }

    void GenerateTriggerAreaForDoor() {
        var triggerArea = new GameObject();
        triggerArea.name = "TriggerArea";
        triggerArea.transform.parent = this.transform;
        triggerArea.transform.localScale = new Vector3(10f, 1f, 1f);
        triggerArea.transform.rotation = transform.rotation;
        triggerArea.transform.position = transform.position;
        triggerArea.AddComponent<BoxCollider>();
        triggerArea.GetComponent<BoxCollider>().isTrigger = true;
        triggerArea.AddComponent<DoorTriggerBehavior>();
        _doorTriggerArea = triggerArea;
    }
    
    //open door/window/ventilationShaft Animation, currently placeholder
    IEnumerator OpenDoor() {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(2);
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
    }

    bool PlayerInRange() {
        if (Vector3.Distance(_playerRef.transform.position, transform.position) < 2)
            return true;
        return false;
    }

    //tweens here are looping ! Ill find a better way, soon
    public void LookAt() {
        Sequence showHintWorldCanvas = DOTween.Sequence();
        showHintWorldCanvas.Append(_thisInteractableCanvas.transform.DOScaleY(0.0002982685f, 0.2f)).OnComplete(() => { 
            showHintWorldCanvas.Kill();
        });
    }
    
    public void LookAway() {
        Sequence hideHintWorldCanvas = DOTween.Sequence();
        hideHintWorldCanvas.Append(_thisInteractableCanvas.transform.DOScaleY(0f, 0.2f)).OnComplete(() => { 
            hideHintWorldCanvas.Kill();
        });
    }

    public void KeyPressHint(KeyCode keyCode) {
    }

    public void TextHint(string hintText) {
    }
}
