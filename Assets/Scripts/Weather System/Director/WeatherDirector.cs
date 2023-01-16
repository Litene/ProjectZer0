using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WeatherSystem.Controllers;

namespace WeatherSystem.Director {
    [ExecuteAlways, RequireComponent(typeof(WeatherController))]
    public class WeatherDirector : MonoBehaviour {
        [SerializeField] private WeatherState[] _weatherStates = Array.Empty<WeatherState>();
        private List<string> WeatherStateNames => _weatherStates.Select(weatherState => weatherState.Name).ToList();
        [Dropdown("WeatherStateNames")] public string DesiredWeatherState;

        private WeatherState _target;
        private WeatherController _weatherController;
        private const float TotalTransitionDuration = 60f;
        
        public WeatherState GetDesiredWeatherState => NameToWeatherState(DesiredWeatherState);
        private WeatherState NameToWeatherState (string name) => _weatherStates.ToList().FirstOrDefault(state => name == state.Name);

        public void ApplyWeatherStateImmediate(WeatherState value) {
            if (value != null) {
                _weatherController.Intensity = value.Intensity;
            }
        }

        private void OnEnable() => _weatherController = GetComponent<WeatherController>();
        
        private void Start() => _target = GetDesiredWeatherState;
        
        private void Update() {
            if (!Application.isPlaying) { return; }
            UpdateIntensity();
        }
        
        private void UpdateIntensity() {
            _target = GetDesiredWeatherState;
            var transitionDirection = Mathf.Sign(_target.Intensity - _weatherController.Intensity);
            var transitionRate = Time.deltaTime / TotalTransitionDuration;
            if (Mathf.Abs(_target.Intensity - _weatherController.Intensity) <= transitionRate) {
                _weatherController.Intensity = _target.Intensity;
                return; 
            }
            _weatherController.Intensity += transitionDirection * transitionRate;
        }
        
        public WeatherState GetCurrentWeatherState() {
            var minDeltaIntensity = Mathf.Infinity;
            WeatherState currentWeatherState = null;
            foreach (var weatherState in _weatherStates) {
                var absoluteDeltaIntensity = Mathf.Abs(weatherState.Intensity - _weatherController.Intensity);
                if (absoluteDeltaIntensity < minDeltaIntensity) {
                    minDeltaIntensity = absoluteDeltaIntensity;
                    currentWeatherState = weatherState;
                }
            }
            return currentWeatherState;
        }
    }
}