using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SunSetting", menuName = "Lighting/SunSetting")]
public class SunSetting : ScriptableObject {
   [ShowOnly] public LightState LightState;
   public float DegreesX;
   public float DegreesY;
   private const int NameStartIndex = 0;
   private const int NameEndIndex = 10;
   [Range(1500f, 20000f)][Tooltip("Color temperature in Kelvin")] public float ColorTemperature; 
   private void OnValidate() {
       string shortedString = name.Remove(NameStartIndex, NameEndIndex);
       if (Enum.TryParse(shortedString, out LightState state)) LightState = state;
   }
}