using UnityEngine;

public class ArcticWeatherManager : MonoBehaviour
{
    [Range(0, 1)] private float _intensity; 

    // TODO: Lerp the volume profile's "fog attenuation distance" from 100000 to 1000, based on _intensity.
    // TODO: Lerp snows values, based on _intensity.
}
