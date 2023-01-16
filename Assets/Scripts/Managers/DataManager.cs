using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using Steamworks;

public class DataManager : MonoBehaviour
{
    
    //STUFF THAT NEEDS TO BE STORED IN A SAVE
//    Current Scene, PlayerPos, PlayerRotation, PlayerHealth, Inventory, day cycle, Quest State

    public bool SaveGame(string data) {
        var fullPath = Path.Combine(Application.persistentDataPath, SaveUtility.SaveFileName);

        try {
            File.WriteAllText(fullPath, data);
            return SteamRemoteStorage.FileWrite(SaveUtility.SaveFileName, File.ReadAllBytes(fullPath));
        }
        catch (Exception e) {
            Debug.LogError("Save fail : " + e);
            return false;
        }
    }
}
