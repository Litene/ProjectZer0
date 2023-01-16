using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]public class SoundTrigger : MonoBehaviour { // todo, fix private stuff. and naming. 
    
    
    public enum soundType { // todo: add transform selection for moving things etc.
        localSound,
        worldSound,
        music
    }

    public soundType SoundType;
    [Space]
    [Header("Type in audio name WITHOUT extension. (like .mp3)")]
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
                    SoundManager.Instance.PlaySound(audioName.ToUpper());
                    break;
                case soundType.worldSound:
                    SoundManager.Instance.PlaySound(audioName.ToUpper(), audioPosition);
                    break;
                case soundType.music:
                    SoundManager.Instance.PlayMusic(audioName.ToUpper());
                    break;
            }
        }
    }
}
