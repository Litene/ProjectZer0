using System;
using UnityEngine;

public class CumulativeTexture : MonoBehaviour {
    [SerializeField] private Material _material;
    [SerializeField] private RenderTexture _cumulativeRenderTexture;

    private Vector2Int Resolution => new Vector2Int(_cumulativeRenderTexture.width, _cumulativeRenderTexture.height);

    /*
    private RenderTexture GetRenderTexture(Material material) {
        // Render material to render texture
        var renderTexture = RenderTexture.GetTemporary(Resolution.x, Resolution.y);
        Graphics.Blit(null, renderTexture, _material, 0);
        return renderTexture;
    }
    */

    private void OnEnable()
    {
        ResetCumulativeRenderTexture();
    }

    private void OnDisable()
    {
        ResetCumulativeRenderTexture();
    }

    private void ResetCumulativeRenderTexture() {
        Graphics.Blit(Texture2D.blackTexture, _cumulativeRenderTexture);
    }

    private void Update() {
        // Get render texture from render material
        var renderTexture = RenderTexture.GetTemporary(Resolution.x, Resolution.y);
        Graphics.Blit(null, renderTexture, _material, 0);
        
        // Set the cumulative render texture to be the material's output
        Graphics.CopyTexture(renderTexture, _cumulativeRenderTexture);
        
        // Clean up variables
        RenderTexture.ReleaseTemporary(renderTexture);
    }
}