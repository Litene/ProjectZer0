using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour {
    
    
    public enum soundType {
        localSound,
        worldSound,
        music
    }

    public soundType SoundType;
    [Space]
    [Header("Type in audio name WITHOUT extension. (like .mp3)")]
    [Header("Case sensitive.")]
    [Space(25)]
    public string audioName;
    [Space]
    [Header("Tip: Copy empty g-object position.")]
    [Space(25)]
    public Vector3 audioPosition;


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            switch (SoundType) {
                case soundType.localSound:
                    SoundManager.Instance.PlaySound(audioName);
                    break;
                case soundType.worldSound:
                    SoundManager.Instance.PlaySound(audioName, audioPosition);
                    break;
                case soundType.music:
                    SoundManager.Instance.PlayMusic(audioName);
                    break;
            }
        }
    }
}
