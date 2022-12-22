using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WeatherSystem
{
    [RequireComponent(typeof(ArcticWeatherController))]
    [ExecuteAlways]
    public class ArcticWeatherDirector : MonoBehaviour {
        //private float GetProgress(float t) => (_arcticWeatherController.Intensity - _previous.Intensity) / (_target.Intensity - _previous.Intensity);

        private ArcticWeatherController _arcticWeatherController;
        
        private const float TOTAL_TRANSITION_DURATION = 15f;

        private void Awake() {
            _arcticWeatherController = GetComponent<ArcticWeatherController>();
        }

        private void Start() {
            _target = _previous = _lastDesiredArcticWeatherState = ArcticWeatherStates[0];
        }

        private List<string> ArcticWeatherStateNames {
            get {
                return ArcticWeatherStates.Select(arcticWeatherState => arcticWeatherState.Name).ToList();
            }
        }
        [Dropdown("ArcticWeatherStateNames")] public string DesiredArcticWeatherState;

        public ArcticWeatherState[] ArcticWeatherStates;

        private ArcticWeatherState _target;
        private ArcticWeatherState _previous;

        private ArcticWeatherState _lastDesiredArcticWeatherState;

        private ArcticWeatherState NameToArcticWeatherState (string name) {
            return ArcticWeatherStates.ToList().FirstOrDefault(state => name == state.Name);
        }

        private void UpdateArcticWeatherState() {
            var desiredArcticWeatherState = NameToArcticWeatherState(DesiredArcticWeatherState);
            if (desiredArcticWeatherState != _lastDesiredArcticWeatherState) {
                SetDesiredArcticWeatherState(desiredArcticWeatherState);
            }
            _lastDesiredArcticWeatherState = desiredArcticWeatherState;
        }
        
        private void SetDesiredArcticWeatherState(ArcticWeatherState value) { 
            _previous = _target;
            _target = value;
        }
        
        private void Update() {
            if (!Application.isPlaying) { return; }
            UpdateArcticWeatherState();
            UpdateIntensity();
        }
        
        private void UpdateIntensity() {
            var transitionDirection = Mathf.Sign(_target.Intensity - _previous.Intensity);
            var transitionRate = Time.deltaTime / TOTAL_TRANSITION_DURATION;
            if (Mathf.Abs(_target.Intensity - _arcticWeatherController.Intensity) <= transitionRate) {
                _arcticWeatherController.Intensity = _target.Intensity;
                _previous = _target;
                return; 
            }
            
            _arcticWeatherController.Intensity += transitionDirection * transitionRate;
        }
    }
}

[Serializable]
public class ArcticWeatherState
{
    public string Name;
    [Range(0,1)] public float Intensity;
}