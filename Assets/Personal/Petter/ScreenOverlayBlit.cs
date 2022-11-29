using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[Serializable, VolumeComponentMenu("Screen Overlay")]
public sealed class ScreenOverlayBlit : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    private Material _blitMat;
    private static readonly int InputTexture = Shader.PropertyToID("_InputTexture");
    
    public bool IsActive() => _blitMat != null && this.active;
    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    public override bool visibleInSceneView => false;

    public override void Setup() {
        _blitMat = new Material(Shader.Find("Hidden/Shader/ScreenOverlay"));
        
        //this is for testing
        Shader.SetGlobalFloat("ScreenTransitionStart", -1);
        Shader.SetGlobalFloat("ScreenTransitionSpeed", .5f);
    }

    public override void Cleanup() {
        CoreUtils.Destroy(_blitMat);
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination) {
        if(_blitMat == null) return;

        _blitMat.SetTexture(InputTexture, source);
        HDUtils.DrawFullScreen(cmd, _blitMat, destination);
    }
}

