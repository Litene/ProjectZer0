using UnityEngine;
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T _instance;
    public static T Instance {
        get {
            _instance ??= FindInScene();
            return _instance ??= GenerateSingleton();
        }
    }
    private static T FindInScene() => FindObjectOfType<T>();
    private static T GenerateSingleton() {
        GameObject gameManagerObject = new GameObject(typeof(T).Name);
        return gameManagerObject.AddComponent<T>();
    }
}

