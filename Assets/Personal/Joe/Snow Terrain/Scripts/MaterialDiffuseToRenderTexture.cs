using UnityEngine;

public class MaterialDiffuseToRenderTexture : MonoBehaviour {
    [SerializeField] private Material _material;
    [SerializeField] private RenderTexture _cumulativeRenderTexture;

    private Vector2Int Resolution => new Vector2Int(_cumulativeRenderTexture.width, _cumulativeRenderTexture.height);
    
    private void Start() {
        ResetCumulativeRenderTexture();
    }
    
    private void ResetCumulativeRenderTexture() {
        Graphics.Blit(Texture2D.blackTexture, _cumulativeRenderTexture);
    }

    private void Update() {
        TransferMaterialDiffuseToRenderTexture();
    }

    private void TransferMaterialDiffuseToRenderTexture() {
        // TODO: Understand why "Graphics.Blit(null, _cumulativeRenderTexture, _material, 0);" alone does not work.
        
        // Get render texture from render material
        var renderTexture = RenderTexture.GetTemporary(Resolution.x, Resolution.y, 0, RenderTextureFormat.R8);
        Graphics.Blit(null, renderTexture, _material, 0);
        
        // Set the cumulative render texture to be the material's output
        Graphics.CopyTexture(renderTexture, _cumulativeRenderTexture);
        
        // Clean up variables
        RenderTexture.ReleaseTemporary(renderTexture);
    }
}
