using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class BakeTexture3dWindow : EditorWindow {
    Material ImageMaterial;
    Vector3Int Resolution;

    bool hasMaterial;
    bool hasResolution;

    [MenuItem ("Tools/Texture Generator/Bake Material to Texture 3D")]
    static void OpenWindow() {
        //create window
        BakeTexture3dWindow window = EditorWindow.GetWindow<BakeTexture3dWindow>();
        window.Show();
        window.CheckInput();
    }

    void OnGUI(){
        EditorGUILayout.HelpBox("Set the material you want to bake as well as the size " + 
                                "of the texture you want to bake to, then press the \"Bake\" button.", MessageType.None);

        using(var check = new EditorGUI.ChangeCheckScope()){
            ImageMaterial = (Material)EditorGUILayout.ObjectField("Material", ImageMaterial, typeof(Material), false);
            Resolution = EditorGUILayout.Vector3IntField("Image Resolution", Resolution);

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
        hasResolution = Resolution.x > 0 && Resolution.y > 0 && Resolution.z > 0;
    }

    void BakeTexture(){
        //get rendertexture to render layers to and texture3d to save values to as well as 2d texture for transferring data
        RenderTexture renderTexture = RenderTexture.GetTemporary(Resolution.x, Resolution.y);
        Texture3D volumeTexture = new Texture3D(Resolution.x, Resolution.y, Resolution.z, TextureFormat.RGB24, false);
        Texture2D tempTexture = new Texture2D(Resolution.x, Resolution.y);

        //prepare for loop
        RenderTexture.active = renderTexture;
        int voxelAmount = Resolution.x * Resolution.y * Resolution.z;
        int slicePixelAmount = Resolution.x * Resolution.y;
        Color32[] colors = new Color32[voxelAmount];

        //loop through slices
        for(int slice=0; slice<Resolution.z; slice++){
            //set z coodinate in shader
            float height = (slice + 0.5f) / Resolution.z;
            ImageMaterial.SetFloat("_Height", height);

            //get shader result
            Graphics.Blit(null, renderTexture, ImageMaterial, 0);
            tempTexture.ReadPixels(new Rect(0, 0, Resolution.x, Resolution.y), 0, 0);
            Color32[] sliceColors = tempTexture.GetPixels32();

            //copy slice to data for 3d texture
            int sliceBaseIndex = slice * slicePixelAmount;
            for(int pixel=0; pixel<slicePixelAmount; pixel++){
                colors[sliceBaseIndex + pixel] = sliceColors[pixel];
            }
        }

        //apply and save 3d texture
        volumeTexture.SetPixels32(colors);
        var filePath = EditorUtilities.GetActiveFolder() + "/" + ImageMaterial.name + ".asset";
        AssetDatabase.CreateAsset(volumeTexture, filePath);
        Debug.Log("'" + ImageMaterial.name + ".mat' baked as '" + filePath + "'.");

        //clean up variables
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(renderTexture);
        //DestroyImmediate(volumeTexture);
        DestroyImmediate(tempTexture);
    }
}