using System;
using UnityEditor.PackageManager;
using UnityEngine;

public class UIManager : Singleton<UIManager> {
    private GameObject _uiManager;

    private Ray ray;
    private RaycastHit raycasthit;

    private GameObject temporaryObject;

    private void Awake() { 
        if (_uiManager == null) {
            _uiManager = new GameObject {
                name = "UI Manager"
            };
        }
    }
    
    private void Update() {
        //Start of raycast. This shouldnt be in UI Manager, but while we dont have a proper controller it makes most sense to have it here.
        ray.origin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        ray.direction = Camera.main.transform.forward;
        if (Physics.Raycast(ray, out raycasthit, 100f)) {
            if (raycasthit.collider.tag == "Interactable") {
                temporaryObject = raycasthit.collider.gameObject;
                raycasthit.collider.GetComponent<Interactable>().LookAt();
            }
            else if(temporaryObject != null){
                temporaryObject.GetComponent<Interactable>().LookAway();
            }
        }
        //end of raycast
    }

}
