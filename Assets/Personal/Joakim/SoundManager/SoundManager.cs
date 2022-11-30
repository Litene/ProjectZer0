using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class SoundManager : Singleton<SoundManager> {
    public AudioSource audioSource;
    public List<AudioClip> sfxClips = new List<AudioClip>();
    public List<AudioClip> musicClips = new List<AudioClip>();
    public List<string> soundKeys = new List<string>();
    public List<string> musicKeys = new List<string>();
    private readonly Dictionary<string, AudioClip> _keyToAudio = new Dictionary<string, AudioClip>();
    private string _sfxFileDirectory;
    private string _musicFileDirectory;
    private string[] _sfxFiles;
    private string[] _musicFiles;
    private GameObject _soundObject;
    private GameObject _soundPool;
    private ObjectPool<GameObject> _pool;

    private void Awake() {
        if (_soundPool == null) {
            _soundPool = new GameObject {
                name = "SoundPool"
            };
        }
        
        _pool = new ObjectPool<GameObject>(() => _soundObject = new GameObject(), soundObj => {
            soundObj.SetActive(true);
        }, soundObj => { 
            soundObj.SetActive(false);
        }, soundObj => {
            Destroy(soundObj.gameObject);
        }, false, 10, 30);
        
        _sfxFileDirectory = Application.dataPath + "/Personal/Joakim/SFX";
        _musicFileDirectory = Application.dataPath + "/Personal/Joakim/Music";

        Debug.Log("SoundManager: <color=yellow>Current Dir for SFX: </color>" + _sfxFileDirectory +
                  "<color=yellow>, Current Dir for Music: </color>" + _musicFileDirectory);

        if (Directory.Exists(Application.dataPath + "/Personal/Joakim/SFX")) {
            _sfxFiles = Directory.GetFiles(_sfxFileDirectory).Where(fileName => !fileName.EndsWith(".meta")).ToArray();
            Debug.Log("SoundManager: <color=green>SFX Directory Found!</color>");
        }
        else {
            Debug.Log("SoundManager: <color=red>SFX Directory not Found!</color>");
        }

        if (Directory.Exists(Application.dataPath + "/Personal/Joakim/Music")) {
            _musicFiles = Directory.GetFiles(_musicFileDirectory).Where(fileName => !fileName.EndsWith(".meta")).ToArray();
            Debug.Log("SoundManager: <color=green>Music Directory Found!</color>");
        }
        else {
            Debug.Log("SoundManager: <color=red>Music Directory not Found!</color>");
        }

        for (var i = 0; i < _sfxFiles.Length; i++) {
            if (!_sfxFiles[i].EndsWith(".wav")) continue;
            sfxClips.Add(new WWW(_sfxFiles[i]).GetAudioClip(false, true, AudioType.WAV));
            sfxClips[i].name = Path.GetFileName(_sfxFiles[i]);
            MakeKeyOutOf(i, "sfx");
            Debug.Log("SoundManager: Successfully Loaded <color=green>" + i + "/" + (_sfxFiles.Length - 1) +
                      "</color> SFX files");
        }

        for (var i = 0; i < _musicFiles.Length; i++) {
            if (!_musicFiles[i].EndsWith(".wav")) continue;
            musicClips.Add(new WWW(_musicFiles[i]).GetAudioClip(false, true, AudioType.WAV));
            musicClips[i].name = Path.GetFileName(_musicFiles[i]);
            MakeKeyOutOf(i, "music");
            Debug.Log("SoundManager: Successfully Loaded <color=green>" + i + "/" + (_musicFiles.Length - 1) +
                      "</color> Music files");
        }

        void MakeKeyOutOf(int audioClipThatNeedsAKey, string soundType) {
            string key;
            switch (soundType) {
                case "sfx":
                    key = Path.GetFileNameWithoutExtension(_sfxFiles[audioClipThatNeedsAKey]);
                    soundKeys.Add(key);
                    _keyToAudio.Add(key, sfxClips[audioClipThatNeedsAKey]);
                    break;
                case "music":
                    key = Path.GetFileNameWithoutExtension(_musicFiles[audioClipThatNeedsAKey]);
                    musicKeys.Add(key);
                    _keyToAudio.Add(key, musicClips[audioClipThatNeedsAKey]);
                    break;
            }
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator SpawnSoundObj(string fileName, Vector3 pos, float clipLength) {
         GameObject soundObj = _pool.Get();
         soundObj.name = "SoundAtPos";
         soundObj.transform.SetParent(_soundPool.transform);
         soundObj.transform.position = pos;
         if (soundObj.GetComponent<AudioSource>() == null) {
             soundObj.AddComponent<AudioSource>();
         }
         else {
             soundObj.GetComponent<AudioSource>();
         }
         var sAudioSource = soundObj.GetComponent<AudioSource>();
         sAudioSource.PlayOneShot(_keyToAudio[fileName]);
         Debug.Log("SoundManager: Played sound: '<color=green>"+fileName+"</color>' at <color=yellow>"+pos+"</color>");
         yield return new WaitForSeconds(clipLength);
        _pool.Release(soundObj);

     }

     /// <summary>
    /// Plays sound <para>fileName</para>, exclude file extension.
    /// </summary>
    /// <param name="fileName"></param>
    public void PlaySound(string fileName) {
         audioSource.PlayOneShot(_keyToAudio[fileName]);
    }
    
    /// <summary>
    /// Plays sound <para>fileName</para> at <para>pos</para>
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="pos"></param>
    public void PlaySound(string fileName, Vector3 pos) {
        AudioClip ac = _keyToAudio[fileName];
        float clipLength = ac.length;
        StartCoroutine(SpawnSoundObj(fileName, pos, clipLength));
    }
    
    /// <summary>
    /// Plays sound <para>fileName</para> at <para>localTransform</para>
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="localTransform"></param>
    public void PlaySound(string fileName, Transform localTransform) {
        var lAudioSource = localTransform.AddComponent<AudioSource>();
        lAudioSource.PlayOneShot(_keyToAudio[fileName]);
    }

    private void Update() {
        if (UnityEngine.Input.GetKeyDown(KeyCode.T)) {
            PlaySound("Sound1", new Vector3(4, 4, 4));
        }
    }
}