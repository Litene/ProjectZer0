using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : Singleton<Player> {
    
    [SerializeField] private List<PointOfInterest> _pointsOfInterests = new List<PointOfInterest>();
    [SerializeField]private List<PointOfInterest> _renderedPoints = new List<PointOfInterest>();



    public float GetWeight() {
        float weight = 0;
        foreach (var point in _renderedPoints) weight += point.Weight;
        
        weight /= _renderedPoints.Count;
        weight *= 0.1f;
        
        return _renderedPoints.Count == 0 ? 0.5f : weight;
    }

    private void Update() {
        if (_pointsOfInterests.Count == 0) {
            _renderedPoints.Clear();
            return;
        }
        
        foreach (var point in _pointsOfInterests) {
            if (!_renderedPoints.Contains(point) && point.Renderer.isVisible) {
                _renderedPoints.Add(point);
            }
            else if (_renderedPoints.Contains(point) && !point.Renderer.isVisible) {
                _renderedPoints.Remove(point);
            }
        }
    }

    public void AddToList(PointOfInterest point) {
        _pointsOfInterests.Add(point);

        if (_pointsOfInterests.Count == 1) return;
        _pointsOfInterests = _pointsOfInterests.OrderByDescending(currentPoint => currentPoint.Weight).ToList();
    }

    public void RemoveFromList(PointOfInterest point) {
        _pointsOfInterests.Remove(point);

        if (_pointsOfInterests.Count <= 1) return;
        _pointsOfInterests = _pointsOfInterests.OrderByDescending(currentPoint => currentPoint.Weight).ToList();
    }
}