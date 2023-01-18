using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class MVPOptions : MonoBehaviour {
    public Slider gammaSlider;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    private bool paused;
    private LiftGammaGain gamma;
    private Volume volume;
    public GameObject PostProcessingVolume;

    private void Awake() {
        volume = PostProcessingVolume.GetComponent<Volume>();
        volume.profile.TryGet<LiftGammaGain>(out gamma);
    }

    void Update() {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape)) {
            paused = !paused;
        }
        if (paused) {
            pauseMenu.SetActive(true);
        }
        else {
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(false);
        }
    }

    public void AdjustGamma() {
        gamma.gamma.value = new Vector4(1f, 1f, 1f, gammaSlider.value);
    }

    public void Resume() {
        pauseMenu.SetActive(false);
    }

    public void Options() {
        optionsMenu.SetActive(true);
    }

    public void QuitToMenu() {
        
    }

    public void Back() {
        optionsMenu.SetActive(false);
    }

}
