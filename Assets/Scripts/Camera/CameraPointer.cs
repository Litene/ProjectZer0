using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraPointer : MonoBehaviour {

    [SerializeField] private List<PointOfInterest> _pointOfInterests = new List<PointOfInterest>();
    public Player player;
    private void OnTriggerEnter(Collider other) {
        if (_pointOfInterests.Count == 0) return;
        
        if (other.TryGetComponent<Player>(out player)) {
            _pointOfInterests.ForEach(point => player.AddToList(point));
        }
    }

    private void OnTriggerExit(Collider other) {
        if (_pointOfInterests.Count == 0) return;
        
        if (other.TryGetComponent<Player>(out player)) {
            _pointOfInterests.ForEach(point => player.RemoveFromList(point));
        }
    }
}