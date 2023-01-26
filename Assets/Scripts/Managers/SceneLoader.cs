using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader> {
    private void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(this);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        //load save data
    }

    public void ReloadScene() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        Player.Instance.PlayerInventory.Initialize();
        
    }
    
    public void LoadScene(string scene) {
        var asyncOperation = SceneManager.LoadSceneAsync(scene);
        Player.Instance.PlayerInventory.Initialize();
    }

    public void ColdDeathTransition(Material _coldMat) {
        ReloadScene();
        StartCoroutine(RemoveWhiteness(_coldMat));
        StartCoroutine(RemoveColdSides(_coldMat));
    }

    private IEnumerator RemoveWhiteness(Material _coldMat) {
        float targetWhiteness = 0.9f;
        while (Mathf.Abs(targetWhiteness - _coldMat.GetFloat("_Whiteness")) >= 0.2f) {
            var getWhiteness = _coldMat.GetFloat("_Whiteness");
            _coldMat.SetFloat("_Whiteness", getWhiteness -= 0.2f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        _coldMat.SetFloat("_Whiteness", targetWhiteness);
    }
    
    private IEnumerator RemoveColdSides(Material _coldMat) {
        float targetSides = 22;
        while (Mathf.Abs(targetSides- _coldMat.GetFloat("_Sides")) >= 0.2f) {
            var getSides = _coldMat.GetFloat("_Sides");
            _coldMat.SetFloat("_Sides", getSides += 4.4f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        _coldMat.SetFloat("_Sides", targetSides);
    }
    
    

    public void DeathTransition() {
        
    }
    
    
}

public enum TransitionTag {
 A,
 B,
 C,
 D,
 E
}