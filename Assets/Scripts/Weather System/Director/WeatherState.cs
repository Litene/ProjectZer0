using System;
using UnityEngine;

namespace WeatherSystem.Director {
    [Serializable]
    public class WeatherState {
        public string Name;
        [Range(0,1)] public float Intensity;
    }
}