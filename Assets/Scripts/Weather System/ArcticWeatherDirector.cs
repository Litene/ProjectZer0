using System;
using UnityEngine;
using Utilities;

namespace WeatherSystem
{
    [RequireComponent(typeof(ArcticWeatherController))]
    [ExecuteAlways]
    public class ArcticWeatherDirector : MonoBehaviour {
        // TODO: Look into generating the enum dynamically based on ArcticWeatherStates names.
        
        /*
        [SerializeProperty("ArcticWeatherState")]
        public ArcticWeatherState _arcticWeatherState;
        public ArcticWeatherState ArcticWeatherState {
            get => _arcticWeatherState;
            set {
                _arcticWeatherState = value;
            }
        }
        */
        
        /*
public enum ArcticWeatherState { // TODO: Convert this to a dictionary of state-names to intensity values.
    Clear,
    Light,
    Moderate,
    Heavy
}
*/
        
        //private float GetProgress(float t) => (_arcticWeatherController.Intensity - _previous.Intensity) / (_target.Intensity - _previous.Intensity);

        private ArcticWeatherController _arcticWeatherController;
        
        private const float TOTAL_TRANSITION_DURATION = 60f;

        private void Awake()
        {
            _arcticWeatherController = GetComponent<ArcticWeatherController>();
        }

        private ArcticWeatherState _target;
        private ArcticWeatherState _previous;


        private void Update() {
            if (!Application.isPlaying) { return; }
            UpdateIntensity();
        }

        private void UpdateIntensity()
        {
            var transitionDuration = TOTAL_TRANSITION_DURATION * Mathf.Abs(_target.Intensity - _previous.Intensity);
            var transitionDirection = Mathf.Sign(_target.Intensity - _previous.Intensity);
            if (transitionDuration == 0) { return; }
            _arcticWeatherController.Intensity += transitionDirection * Time.deltaTime / transitionDuration;
        }
        
        public ArcticWeatherState[] ArcticWeatherStates;
    }
}

[Serializable]
public class ArcticWeatherState
{
    public string Name;
    [Range(0,1)] public float Intensity;
}