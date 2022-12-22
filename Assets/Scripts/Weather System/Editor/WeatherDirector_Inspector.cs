using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;

namespace WeatherSystem.Editor
{
    [CustomEditor(typeof(WeatherDirector))]
    public class WeatherDirector_Inspector : UnityEditor.Editor {
        private WeatherDirector _weatherDirector;
        
        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            _weatherDirector = (WeatherDirector)target;
            DrawNearestWeatherStateInspector();
            ApplyWeatherStateImmediate();
        }
        
        private void DrawNearestWeatherStateInspector() {
            if (_weatherDirector.GetNearestWeatherState() == null) { return; }
            EditorGUILayout.HelpBox("Nearest Weather State: " + _weatherDirector.GetNearestWeatherState().Name, MessageType.Info);
        }

        private WeatherState _lastDesiredWeatherState;
        
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
