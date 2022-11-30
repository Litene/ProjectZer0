using UnityEngine;
using UnityEditor;

public abstract class Texture3DGenerator {
    [MenuItem("Texture 3D Generator/Create Example")]
    private static void CreateExampleTexture3D() => CreateTexture3D(Texture3DMode.Example);
    
    [MenuItem("Texture 3D Generator/Create Tileable Perlin Noise")]
    private static void CreateTileablePerlinNoiseTexture3D() => CreateTexture3D(Texture3DMode.TileablePerlinNoise);
    
    private static void CreateTexture3D(Texture3DMode texture3DMode) {
        // Configure the texture
        const int size = 32;
        const TextureFormat format = TextureFormat.RGBA32;
        const TextureWrapMode wrapMode = TextureWrapMode.Clamp;

        // Create the texture and apply the configuration
        var texture = new Texture3D(size, size, size, format, false) {
            wrapMode = wrapMode
        };

        // Create a 3-dimensional array to store color data
        var colors = new Color[size * size * size];

        SetColors(ref colors, size, texture3DMode);

        // Copy the color values to the texture
        texture.SetPixels(colors);

        // Apply the changes to the texture and upload the updated texture to the GPU
        texture.Apply();        

        // Save the texture to your Unity Project
        EditorUtilities.CreateAssetInActiveFolder(texture, "Example3DTexture");
    }

    private static void SetColors(ref Color[] colors, int size, Texture3DMode texture3DMode) {
        switch (texture3DMode)
        {
            case Texture3DMode.Example:
                SetColorsExample(ref colors, size);
                break;
            case Texture3DMode.TileablePerlinNoise:
                SetColorsTileablePerlinNoise(ref colors, size);
                break;
        }
    }

    private static void SetColorsExample(ref Color[] colors, int size)
    {
        // Populate the array so that the x, y, and z values of the texture will map to red, blue, and green colors
        var inverseResolution = 1.0f / (size - 1.0f);
        for (var z = 0; z < size; z++) {
            var zOffset = z * size * size;
            for (var y = 0; y < size; y++) {
                var yOffset = y * size;
                for (var x = 0; x < size; x++) {
                    colors[x + yOffset + zOffset] = new Color(x * inverseResolution,
                        y * inverseResolution, z * inverseResolution, 1.0f);
                }
            }
        }
    }
    
    private static void SetColorsTileablePerlinNoise(ref Color[] colors, int size)
    {
        // Populate the array so that the x, y, and z values of the texture will map to red, blue, and green colors
        var inverseResolution = 1.0f / (size - 1.0f);
        for (var z = 0; z < size; z++) {
            var zOffset = z * size * size;
            for (var y = 0; y < size; y++) {
                var yOffset = y * size;
                for (var x = 0; x < size; x++) {
                    colors[x + yOffset + zOffset] = new Color(x * inverseResolution,
                        y * inverseResolution, z * inverseResolution, 1.0f);
                }
            }
        }
    }

    private enum Texture3DMode {
        Example,
        TileablePerlinNoise
    }
}