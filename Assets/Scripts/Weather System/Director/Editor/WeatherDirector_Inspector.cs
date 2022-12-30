using UnityEditor;
using UnityEngine;
using WeatherSystem.Director;

namespace WeatherSystem.Editor {
    [CustomEditor(typeof(WeatherDirector))]
    public class WeatherDirector_Inspector : UnityEditor.Editor {
        private WeatherDirector _weatherDirector;
        private WeatherState _lastDesiredWeatherState;
        
        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            _weatherDirector = (WeatherDirector)target;
            DrawNearestWeatherStateInspector();
            ApplyWeatherStateImmediate();
        }
        
        private void DrawNearestWeatherStateInspector() {
            if (_weatherDirector.GetCurrentWeatherState() == null) { return; }
            EditorGUILayout.HelpBox("Current weather state: " + _weatherDirector.GetCurrentWeatherState().Name, MessageType.Info);
        }

        private bool HasDesiredWeatherStateChanged() {
            var desiredWeatherState = _weatherDirector.GetDesiredWeatherState;
            var hasDesiredWeatherStateChanged = (desiredWeatherState != _lastDesiredWeatherState);
            _lastDesiredWeatherState = desiredWeatherState;
            return hasDesiredWeatherStateChanged;
        }

        private void ApplyWeatherStateImmediate() {
            if (Application.isPlaying || !HasDesiredWeatherStateChanged()) { return; }
            _weatherDirector.ApplyWeatherStateImmediate(_weatherDirector.GetDesiredWeatherState);
        }
    }
}
