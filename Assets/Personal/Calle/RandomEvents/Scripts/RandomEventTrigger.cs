using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomEventTrigger : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        RandomEventManager randomEventMan = GameObject.Find("RandomEventManager").GetComponent<RandomEventManager>();

        foreach (GameObject eventObj in randomEventMan.relevantObjects)
        {
            float chanceTest = Random.Range(0.00001f, 100);
            if (chanceTest <= eventObj.GetComponent<EventObject>().eventChance) {
                foreach (IRandomEvent rEvent in eventObj.GetComponent<EventObject>().allEvents) {
                    rEvent.DoEvent();
                }
            }
        }
        
    }
}
