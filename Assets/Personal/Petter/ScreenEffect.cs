using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScreenEffect
{
    private static readonly int ScreenTransitionStart = Shader.PropertyToID("ScreenTransitionStart");
    private static readonly int ScreenTransitionSpeed = Shader.PropertyToID("ScreenTransitionSpeed");

    public static void StartTransitionIn(float seconds = 1)
    {
        Shader.SetGlobalFloat(ScreenTransitionStart, Time.time);
        Shader.SetGlobalFloat(ScreenTransitionSpeed, 1/seconds);
    }

    public static void StartTransitionOut(float seconds = 1)
    {
        Shader.SetGlobalFloat(ScreenTransitionStart, Time.time);
        Shader.SetGlobalFloat(ScreenTransitionSpeed, -(1/seconds));
    }

    public static void TransitionReset()
    {
        Shader.SetGlobalFloat(ScreenTransitionStart, -1);
        Shader.SetGlobalFloat(ScreenTransitionSpeed, 0);
    }
}
