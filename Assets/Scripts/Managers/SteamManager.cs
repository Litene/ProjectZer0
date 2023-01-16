using System;
using System.Text;
using DG.Tweening.Plugins.Core.PathCore;
using JetBrains.Annotations;
using Steamworks;
using System.IO;
using UnityEngine;
using Path = System.IO.Path;

public class SteamManager : Singleton<SteamManager> {
    private const int GameID = 0;
    public bool LocalMode { get; private set; } = false;

    //VersionNumber 

    private void Awake() {
        try {
            Steamworks.SteamClient.Init(GameID);
        }
        catch (System.Exception e) {
            Debug.LogError(e);
            LocalMode = true;
        }
    }

    private void Update() {
        if (LocalMode) return;

        Steamworks.SteamClient.RunCallbacks();
    }
    
    //FileExist
    public bool FileExist() => !LocalMode && SteamRemoteStorage.FileExists(SaveUtility.SaveFileName);


    public string LoadGame() {
        if (FileExist()) {
            byte[] data = SteamRemoteStorage.FileRead(SaveUtility.SaveFileName);
            return Encoding.UTF8.GetString(data);
        }

        return null;
    }


    //LoadGame

    //CleanSteamFiles


    //on quit?
    // private void OnDisable() {
    //     Steamworks.SteamClient.Shutdown();
    // }
}