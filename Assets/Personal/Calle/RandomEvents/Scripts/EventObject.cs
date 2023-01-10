using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject : MonoBehaviour {
    
    [Tooltip("The chance of this object to fire any event at all, whenever an EventTrigger is being called"), Range(0, 100)]
    public float eventChance;

    public List<IRandomEvent> allEvents = new List<IRandomEvent>();

    private void Start() {
        allEvents.Add(GetComponent<FloatEvent>());
    }
}
