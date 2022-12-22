using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WeatherSystem
{
    [RequireComponent(typeof(WeatherController))]
    [ExecuteAlways]
    public class WeatherDirector : MonoBehaviour {
        private WeatherController _weatherController;
        
        private const float TOTAL_TRANSITION_DURATION = 15f; // TODO: Change this back to 60f after testing.

        private void OnEnable() {
            _weatherController = GetComponent<WeatherController>();
        }

        private void Start() {
            StartWeatherState();
        }

        private List<string> WeatherStateNames => WeatherStates.Select(weatherState => weatherState.Name).ToList();
        [Dropdown("WeatherStateNames")] public string DesiredWeatherState;
        public WeatherState GetDesiredWeatherState => NameToWeatherState(DesiredWeatherState);

        public WeatherState[] WeatherStates;

        private WeatherState _target;
        private WeatherState _previous;

        private WeatherState _lastDesiredWeatherState;

        public WeatherState GetNearestWeatherState() {
            var minDeltaIntensity = Mathf.Infinity;
            WeatherState nearestWeatherState = null;
            foreach (var weatherState in WeatherStates) {
                var absoluteDeltaIntensity = Mathf.Abs(weatherState.Intensity - _weatherController.Intensity);
                if (absoluteDeltaIntensity < minDeltaIntensity) {
                    minDeltaIntensity = absoluteDeltaIntensity;
                    nearestWeatherState = weatherState;
                }
            }
            return nearestWeatherState;
        }
        
        private WeatherState NameToWeatherState (string name) {
            return WeatherStates.ToList().FirstOrDefault(state => name == state.Name);
        }

        private void StartWeatherState() {
            var desiredWeatherState = GetDesiredWeatherState;
            _target = _previous = _lastDesiredWeatherState = desiredWeatherState;
        }
        
        private void UpdateWeatherState() {
            var desiredWeatherState = GetDesiredWeatherState;
            if (desiredWeatherState != _lastDesiredWeatherState) {
                SetDesiredWeatherState(desiredWeatherState);
            }
            _lastDesiredWeatherState = desiredWeatherState;
        }
        
        private void SetDesiredWeatherState(WeatherState value) { 
            _previous = _target;
            _target = value;
        }

        private void Update() {
            if (!Application.isPlaying) { return; }
            UpdateWeatherState();
            UpdateIntensity();
        }

        public void ApplyWeatherStateImmediate(WeatherState value) {
            _weatherController.Intensity = value.Intensity;
        }

        private void UpdateIntensity() {
            var transitionDirection = Mathf.Sign(_target.Intensity - _previous.Intensity);
            var transitionRate = Time.deltaTime / TOTAL_TRANSITION_DURATION;
            if (Mathf.Abs(_target.Intensity - _weatherController.Intensity) <= transitionRate) {
                _weatherController.Intensity = _target.Intensity;
                _previous = _target;
                return; 
            }
            _weatherController.Intensity += transitionDirection * transitionRate;
        }
    }
}

[Serializable]
public class WeatherState
{
    public string Name;
    [Range(0,1)] public float Intensity;
}