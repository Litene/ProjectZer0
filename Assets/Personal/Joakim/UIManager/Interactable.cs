using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Interactable : MonoBehaviour, IInteractable {
    
    public string hintText;
    public KeyCode keyPressHint;
    private TextMeshProUGUI worldSpaceText;
    private GameObject thisInteractableCanvas;

    public enum InteractableType{
        KeyPressHint,
        HintTextOnly
    }

    public InteractableType itemType;
    
    private void Awake() {
        Transform trans = transform;
        Transform canvasTrans = trans.Find("Canvas");
        if (canvasTrans != null) {
            thisInteractableCanvas = canvasTrans.gameObject;
        }
        worldSpaceText = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start() {
        switch (itemType) {
            case InteractableType.KeyPressHint:
                worldSpaceText.text = "[" + keyPressHint.ToString() + "]";
                break;
            case InteractableType.HintTextOnly:
                worldSpaceText.text = hintText;
                break;
        }
    }

    public void LookAt() {
        Sequence _showHintWorldCanvas = DOTween.Sequence();
        _showHintWorldCanvas.Append(thisInteractableCanvas.transform.DOScaleY(0.0002982685f, 0.2f)).OnComplete(() => {
        _showHintWorldCanvas.Kill();
        });
    }
    
    public void LookAway() {
        Sequence _hideHintWorldCanvas = DOTween.Sequence();
        _hideHintWorldCanvas.Append(thisInteractableCanvas.transform.DOScaleY(0f, 0.2f)).OnComplete(() => {
            _hideHintWorldCanvas.Kill();
        });
    }

    public void KeyPressHint(KeyCode keyCode) {
    }

    public void TextHint(string hintText) {
    }
}
