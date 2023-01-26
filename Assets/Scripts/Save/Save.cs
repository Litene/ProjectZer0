using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Save {
    public string UID;
}

[System.Serializable] public class InventoryData : Save {
    public List<string> InventoryNames;
}

[System.Serializable]
public class MissionData : Save {
    public string MissionDataStuff;
}

[System.Serializable]
public class SceneData : Save {
    public string SceneName;
    public TransitionTag Destination;
}