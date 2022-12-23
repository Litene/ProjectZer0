using System;
using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Interactable : MonoBehaviour, IInteractable {
    
    public string HintText;
    public KeyCode KeyPressHintText;
    public Item itemPickup;
    private TextMeshProUGUI _worldSpaceText;
    private GameObject _thisInteractableCanvas;
    public bool isPickup, isKeyInteractable;
    private GameObject PlayerRef;
    private GameObject temp;

    public enum InteractableType{
        KeyPressHint,
        HintTextOnly,
        PickUp
    }

    public enum KeyPressInteractions {
        Door,
        Ventilation,
        Window
    }

    public InteractableType TypeOfInteractable;
    public KeyPressInteractions KeyPressInteractionType;
    
    private void Awake() {
        PlayerRef = GameObject.Find("Player");
        Transform trans = transform;
        Transform canvasTrans = trans.Find("Canvas");
        if (canvasTrans != null) {
            _thisInteractableCanvas = canvasTrans.gameObject;
        }
        _worldSpaceText = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    private void Start() {
        switch (TypeOfInteractable) {
            case InteractableType.KeyPressHint:
                _worldSpaceText.text = "[" + KeyPressHintText.ToString() + "]";
                isKeyInteractable = true;
                break;
            case InteractableType.HintTextOnly:
                _worldSpaceText.text = HintText;
                break;
            case InteractableType.PickUp:
                isPickup = true;
                break;
        }
    }

    private void Update() {
        if (isKeyInteractable && PlayerInRange() && UnityEngine.Input.GetKeyDown(KeyPressHintText)) {
            switch (KeyPressInteractionType) {
                case KeyPressInteractions.Door:
                    StartCoroutine(OpenDoor());
                    break;
                case KeyPressInteractions.Ventilation:
                    StartCoroutine(OpenDoor());
                    break;
                case KeyPressInteractions.Window:
                    StartCoroutine(OpenDoor());
                    break;
            }
        }

        if (isPickup && itemPickup.isDisabled) {
            this.gameObject.SetActive(false);
        }

        if (isPickup) {
            itemPickup.itemPosition = transform.position;
        }
        
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
        if (Vector3.Distance(PlayerRef.transform.position, transform.position) < 2)
            return true;
        return false;
    }

    //tweens here are looping ! Ill find a better way eventually
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
