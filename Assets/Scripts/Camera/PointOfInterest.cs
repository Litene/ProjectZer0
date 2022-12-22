using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour {
    [Range(0, 10)]public int Weight;
    public Renderer Renderer { get; private set; }
    private void Awake() => Renderer = GetComponent<Renderer>();
    
}