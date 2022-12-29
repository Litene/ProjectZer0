using System;
using UnityEngine;

public class CumulativeRenderTexture : MonoBehaviour {
    [SerializeField] private RenderTexture _renderTexture;
    [SerializeField] private RenderTexture _cumulativeRenderTexture;

    private Texture2D _currentTexture;
    private Texture2D _cumulativeTexture;
    
    private int Width => _renderTexture.width;
    private int Height => _renderTexture.height;

    private void Start() {
        var colors = new Color[Width*Height];
        Array.Fill(colors, Color.black);
        
        _currentTexture = new Texture2D(Width, Height, TextureFormat.ARGB32, false);
        _currentTexture.SetPixels(colors);
        _currentTexture.Apply(false);
        
        _cumulativeTexture = new Texture2D(Width, Height, TextureFormat.ARGB32, false);
        _cumulativeTexture.SetPixels(colors);
        _cumulativeTexture.Apply(false);
    }

    private void Update() {
        // 1) Read render texture to texture
        Graphics.CopyTexture(_renderTexture, _currentTexture);
        
        // 2) Add current texture to existing texture
        var colors = _cumulativeTexture.GetPixels();
        for (var x = 0; x < _currentTexture.width; x++) {
            for (var y = 0; y < _currentTexture.height; y++) {
                var index = (y * _currentTexture.width) + x;
                //colors[index] += _currentTexture.GetPixel(x, y);
                colors[index] = _currentTexture.GetPixel(x, y);
            }
        }
        _cumulativeTexture.SetPixels(colors);
        _cumulativeTexture.Apply(false); // TODO: Calling this after CopyTexture has undefined results...

        // 3) Apply local cumulative texture to an asset
        Graphics.CopyTexture(_cumulativeTexture, _cumulativeRenderTexture);
    }
}