using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class MVPOptions : MonoBehaviour {
    public Slider masterVolSlider, sfxVolSlider, musicVolSlider, gammaSlider;
    
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject PostProcessingVolume;
    
    private bool paused;
    
    private LiftGammaGain gamma;
    
    private Volume volume;

    public AudioMixer audioMixer;

    private float _masterVolume;
    private float _musicVolume;
    private float _sfxVolume;



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

    public void AdjustMasterVol() {
        _masterVolume = masterVolSlider.value;
        audioMixer.SetFloat("MasterVolParam", _masterVolume);
    }
    
    public void AdjustMusicVol() {
        _musicVolume = musicVolSlider.value;
        audioMixer.SetFloat("MusicVolParam", _musicVolume);
    }
    
    public void AdjustSfxVol() {
        _sfxVolume = sfxVolSlider.value;
        audioMixer.SetFloat("SfxVolParam", _sfxVolume);
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

    public void Back() {
        optionsMenu.SetActive(false);
    }

}
