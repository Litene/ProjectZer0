using UnityEngine;
using UnityEditor;
using System.IO;

// Based on: https://www.ronja-tutorials.com/post/030-baking-shaders/
public class BakeTextureWindow : EditorWindow {
    Material ImageMaterial;
    Vector2Int Resolution;

    bool hasMaterial;
    bool hasResolution;

    [MenuItem ("Tools/Texture Generator/Bake Material to Texture")]
    static void OpenWindow() {
        //create window
        BakeTextureWindow window = EditorWindow.GetWindow<BakeTextureWindow>();
        window.Show();
        window.CheckInput();
    }

    void OnGUI(){
        EditorGUILayout.HelpBox("Set the material you want to bake as well as the size " +
                "of the texture you want to bake to, then press the \"Bake\" button.", MessageType.None);

        using(var check = new EditorGUI.ChangeCheckScope()){
            ImageMaterial = (Material)EditorGUILayout.ObjectField("Material", ImageMaterial, typeof(Material), false);
            Resolution = EditorGUILayout.Vector2IntField("Image Resolution", Resolution);

            if(check.changed){
                CheckInput();
            }
        }

        GUI.enabled = hasMaterial && hasResolution;
        if(GUILayout.Button("Bake")){
            BakeTexture();
        }
        GUI.enabled = true;

        //tell the user what inputs are missing
        if(!hasMaterial){
            EditorGUILayout.HelpBox("Missing a material to bake.", MessageType.Warning);
        }
        if(!hasResolution){
            EditorGUILayout.HelpBox("Image resolution must be greater than zero.", MessageType.Warning);
        }
    }

    void CheckInput(){
        //check which values are entered already
        hasMaterial = ImageMaterial != null;
        hasResolution = Resolution.x > 0 && Resolution.y > 0;
    }

    void BakeTexture(){
        //render material to rendertexture
        var renderTexture = RenderTexture.GetTemporary(Resolution.x, Resolution.y);
        Graphics.Blit(null, renderTexture, ImageMaterial, 0);

        //transfer image from rendertexture to texture
        var texture = new Texture2D(Resolution.x, Resolution.y);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(Vector2.zero, Resolution), 0, 0);

        //save texture to file
        var png = texture.EncodeToPNG();
        var filePath = EditorUtilities.GetActiveFolder() + "/" + ImageMaterial.name + ".png";
        File.WriteAllBytes(filePath, png);
        AssetDatabase.Refresh();
        Debug.Log("'" + ImageMaterial.name + ".mat' baked as '" + filePath + "'.");

        //clean up variables
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(renderTexture);
        DestroyImmediate(texture);
    }
}