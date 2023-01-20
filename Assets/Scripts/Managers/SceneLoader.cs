using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader> {
    private void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        //load save data
    }

    public void ReloadScene() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        
    }
    
    public void LoadScene(string scene) {
        var asyncOperation = SceneManager.LoadSceneAsync(scene);
    }

    public void ColdDeathTransition(Material _coldMat) {
        ReloadScene();
        StartCoroutine(RemoveWhiteness(_coldMat));
    }

    private IEnumerator RemoveWhiteness(Material _coldMat) { // nu plinga klockan, sides måste lerpas med liksom
        _coldMat.SetFloat("_Sides", 22);
        float whiteness = 0.9f;
        while (Mathf.Abs(whiteness - _coldMat.GetFloat("_Whiteness")) <= 0.2f) {
            whiteness = _coldMat.GetFloat("_Whiteness");
            _coldMat.SetFloat("_Whiteness", whiteness += 0.2f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        _coldMat.SetFloat("_Whiteness", whiteness);
    }
    
    

    public void DeathTransition() {
        
    }
}