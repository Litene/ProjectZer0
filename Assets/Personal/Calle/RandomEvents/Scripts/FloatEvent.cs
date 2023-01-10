using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatEvent : MonoBehaviour, IRandomEvent {
    
    [Tooltip("The relative chance of this specific event to fire, compared to other event-weights in the pool of possible ones.")]
    public float weight;
    
    public float Weight {
        get => weight;

        set => weight = value;
    }

    [SerializeField, Tooltip("The amount of seconds before the object falls back down.")] private float floatTime;
    [SerializeField, Tooltip("The speed at which the object floats upwards.")] private float floatSpeed;
    [SerializeField, Tooltip("The height that the object will reach when floating.")] private float floatHeight;

    private Vector3 targetPos;
    private bool shouldFloat = false;
    
    public void DoEvent() {
        GetComponent<Rigidbody>().isKinematic = true;

        targetPos = new Vector3(transform.position.x, transform.position.y + floatHeight, transform.position.z);

        shouldFloat = true;
        
        StartCoroutine(DelayFall());
    }

    private void Update() {
        if (shouldFloat) {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * floatSpeed);
        }
    }

    private IEnumerator DelayFall() {
        yield return new WaitForSeconds(floatTime);

        shouldFloat = false;
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
