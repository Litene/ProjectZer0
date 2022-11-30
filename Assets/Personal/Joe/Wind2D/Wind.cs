using System;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private Vector2 _direction;
    
    [SerializeField] private Material _material;
    
    private void Update() {
        SetWindSpeed(_speed);
        SetWindDirection(_direction);
    }

    private void SetWindSpeed(float speed) => _material.SetFloat("_Speed", speed);

    private void SetWindDirection(Vector2 direction) => _material.SetVector("_Direction", direction.normalized);
}
