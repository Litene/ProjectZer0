using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class T3rdLookTarget : MonoBehaviour {
    [SerializeField, Tooltip("A reference to the player.")] private GameObject player;

    [SerializeField, Range(1f, 20f), Tooltip("A factor which increases the time it takes for the camera to look at the player.")] private float lookTime = 5f;
    private Vector3 _vel;
    
    void Update() {
        Vector3 target = new Vector3(player.transform.position.x, player.transform.position.y + 1.5f, player.transform.position.z);
        
        transform.position = Vector3.SmoothDamp(transform.position, target, ref _vel, lookTime * 10 * Time.deltaTime);
    }
}
