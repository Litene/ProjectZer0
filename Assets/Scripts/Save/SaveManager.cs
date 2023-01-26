using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.Rendering.HighDefinition;

public class SaveManager : Singleton<SaveManager> {
    private SaveFile _saveFile = new SaveFile();
    private const string _saveFileName = "Save.dat";
    private List<ISaveable> _saveables = new List<ISaveable>();

    public void AddListener(ISaveable ISaveableObject) {
        if (!_saveables.Contains(ISaveableObject)) {
            _saveables.Add(ISaveableObject);
        }
    }
    
    public void AddSave(Save save) {
        var saveUID = _saveFile.GetData().FirstOrDefault(uid => uid.UID == save.UID);
            _saveFile.AddData(save);

        if (saveUID == null)
        {
            //_saveFile.Saves.Add(save);
        }
        else {

            var saveIndex = _saveFile.GetData().IndexOf(save);
            Debug.Log(saveIndex);
        }
    }

    [CanBeNull] public Save GetSave(string UID) => _saveFile.GetData().FirstOrDefault(save => save.UID == UID);
    
    /*{
        foreach (var save in _saveFile.Saves) {
            if (save.UID == UID) {
                return save;
            }
        }
    
        return null;
    }*/

    public void SaveGame() {
        _saveables.ForEach(saveable => saveable?.SaveData());
        var path = Path.Combine(Application.persistentDataPath, _saveFileName);
        File.WriteAllText(path, _saveFile.ToJson());
        try {
            
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
}

public interface ISaveable {
    public void LoadData();
    public void SaveData();
    public string UID { get; set; }
}

[System.Serializable] public class SaveFile {
    private List<Save> Saves = new List<Save>();
    public InventoryData InventoryData;

    public void AddData(Save save) {
        Saves.Add(save);
        if (save is InventoryData) {
            InventoryData = (save as InventoryData);
        }
    }

    public List<Save> GetData() {
        return Saves;
    }
    public string ToJson() => JsonUtility.ToJson(this);
}