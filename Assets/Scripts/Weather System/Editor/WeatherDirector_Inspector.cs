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

        private void ApplyWeatherStateImmediate() {
            if (Application.isPlaying) { return; }
            _weatherDirector.ApplyWeatherStateImmediate(_weatherDirector.GetDesiredWeatherState);
        }
    }
}
