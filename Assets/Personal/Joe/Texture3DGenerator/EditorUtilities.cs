using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class EditorUtilities
{
    /// <summary>
    /// Creates an asset in the editor's active project folder.
    /// </summary>
    /// <param name="asset">Object from which to create the asset.</param>
    /// <param name="fileName">File name of the asset.</param>
    /// <typeparam name="T">UnityEngine.Object</typeparam>
    public static void CreateAssetInActiveFolder<T> (T asset, string fileName) where T : Object {
        var getActiveFolderPath = typeof(ProjectWindowUtil).GetMethod(
            "GetActiveFolderPath",
            BindingFlags.Static | BindingFlags.NonPublic);
        var folderPath = (string)getActiveFolderPath.Invoke(null, null);
        var pathName = folderPath + "/" + fileName + ".asset";
        ProjectWindowUtil.CreateAsset(asset, pathName);
    }
}