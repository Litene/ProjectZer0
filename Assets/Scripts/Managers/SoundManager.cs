using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

public class SoundManager : Singleton<SoundManager> { // todo: rename public variables. later to be changed to private. 
    public AudioSource sfxSource, musicSource, ambienceSource;
    public List<AudioClip> sfxClips = new List<AudioClip>();
    public List<AudioClip> musicClips = new List<AudioClip>();
    public List<AudioClip> ambienceClips = new List<AudioClip>();
    public List<string> soundKeys = new List<string>();
    public List<string> musicKeys = new List<string>();
    public List<string> ambienceKeys = new List<string>();
    private readonly Dictionary<string, AudioClip> _keyToAudio = new Dictionary<string, AudioClip>();
    private string _sfxFileDirectory, _musicFileDirectory, _ambienceFileDirectory;
    private string[] _sfxFiles, _musicFiles, _ambienceFiles;
    private GameObject _soundObject;
    private GameObject _soundPool; // probably rename
    private ObjectPool<GameObject> _pool;
    public AudioMixerGroup sfxGroup;

    private void Awake() {
        musicSource = transform.Find("MusicSource").gameObject.GetComponent<AudioSource>();
        sfxSource = transform.Find("SFXSource").gameObject.GetComponent<AudioSource>();
        ambienceSource = transform.Find("AmbienceSource").gameObject.GetComponent<AudioSource>();
        if (_soundPool == null) {
            _soundPool = new GameObject {
                name = "SoundPool"
            };
        }
        
        _pool = new ObjectPool<GameObject>(() => _soundObject = new GameObject(), soundObj => { //todo: can be changed to audiosource instead of gameobject.
            soundObj.SetActive(true);
        }, soundObj => { 
            soundObj.SetActive(false);
        }, soundObj => {
            Destroy(soundObj.gameObject);
        }, false, 10, 30);
        
        _sfxFileDirectory = Application.dataPath + "/Sound/Sfx";
        _musicFileDirectory = Application.dataPath + "/Sound/Music";
        _ambienceFileDirectory = Application.dataPath + "/Sound/Ambience";

        Debug.Log("SoundManager: <color=yellow>Current Dir for SFX: </color>" + _sfxFileDirectory +
                  "<color=yellow>, Current Dir for Music: </color>" + _musicFileDirectory +
                  "<color=yellow>, Current Dir for Ambience: </color>" + _ambienceFileDirectory);

        if (Directory.Exists(_sfxFileDirectory)) {
            _sfxFiles = Directory.GetFiles(_sfxFileDirectory).Where(fileName => !fileName.EndsWith(".meta")).ToArray();
        }

        if (Directory.Exists(_musicFileDirectory)) {
            _musicFiles = Directory.GetFiles(_musicFileDirectory).Where(fileName => !fileName.EndsWith(".meta")).ToArray();
        }
        
        if (Directory.Exists(_ambienceFileDirectory)) {
            _musicFiles = Directory.GetFiles(_ambienceFileDirectory).Where(fileName => !fileName.EndsWith(".meta")).ToArray();
        }

        for (var i = 0; i < _sfxFiles.Length; i++) {
            if (!_sfxFiles[i].EndsWith(".wav")) continue;
            sfxClips.Add(new WWW(_sfxFiles[i]).GetAudioClip(false, true, AudioType.WAV));
            sfxClips[i].name = Path.GetFileName(_sfxFiles[i]);
            MakeKeyOutOf(i, "sfx");
        }

        for (var i = 0; i < _musicFiles.Length; i++) {
            if (!_musicFiles[i].EndsWith(".wav")) continue;
            musicClips.Add(new WWW(_musicFiles[i]).GetAudioClip(false, true, AudioType.WAV));
            musicClips[i].name = Path.GetFileName(_musicFiles[i]);
            MakeKeyOutOf(i, "music");
        }

        for (var i = 0; i < _ambienceFiles.Length; i++) {
            if (!_ambienceFiles[i].EndsWith(".wav")) continue;
            ambienceClips.Add(new WWW(_ambienceFiles[i]).GetAudioClip(false, true, AudioType.WAV));
            ambienceClips[i].name = Path.GetFileName(_ambienceFiles[i]);
            MakeKeyOutOf(i, "ambience");
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
                case "ambience":
                    key = Path.GetFileNameWithoutExtension(_ambienceFiles[audioClipThatNeedsAKey]);
                    ambienceKeys.Add(key);
                    _keyToAudio.Add(key, ambienceClips[audioClipThatNeedsAKey]);
                    break;
            }
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public IEnumerator SpawnSoundObj(string fileName, Vector3 pos, float clipLength) {
         GameObject soundObj = _pool.Get();
         soundObj.name = "SoundAtPos";
         soundObj.transform.SetParent(_soundPool.transform);
         soundObj.transform.position = pos;
         if (!soundObj.TryGetComponent(out AudioSource source)) source = soundObj.AddComponent<AudioSource>();
         source.outputAudioMixerGroup = sfxGroup;
         source.PlayOneShot(_keyToAudio[fileName.ToUpper()]);
         yield return new WaitForSeconds(clipLength);
        _pool.Release(soundObj);
    }

     /// <summary>
    /// Plays sound <para>fileName</para>, exclude file extension.
    /// </summary>
    /// <param name="fileName"></param>
    public void PlaySound(string fileName) {
         sfxSource.outputAudioMixerGroup = sfxGroup;
         sfxSource.PlayOneShot(_keyToAudio[fileName.ToUpper()]);
    }
    
    /// <summary>
    /// Plays sound <para>fileName</para> at <para>pos</para>
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="pos"></param>
    public void PlaySound(string fileName, Vector3 pos) {
        AudioClip ac = _keyToAudio[fileName.ToUpper()];
        float clipLength = ac.length;
        StartCoroutine(SpawnSoundObj(fileName.ToUpper(), pos, clipLength));
    }
    
    /// <summary>
    /// Plays sound <para>fileName</para> at <para>localTransform</para>
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="localTransform"></param>
    public void PlaySound(string fileName, Transform localTransform) { // todo: if it already has a audiosource don't add a new one
        var lAudioSource = localTransform.AddComponent<AudioSource>();
        lAudioSource.outputAudioMixerGroup = sfxGroup;
        lAudioSource.PlayOneShot(_keyToAudio[fileName.ToUpper()]);
    }

    /// <summary>
    /// Plays music <para>fileName</para>
    /// </summary>
    /// <param name="fileName"></param>
    public void PlayMusic(string fileName) {
        AudioClip ac = _keyToAudio[fileName.ToUpper()];
        musicSource.clip = ac;
        musicSource.Play();
    }
    
    
    /// <summary>
    /// Plays ambience <para>fileName</para>
    /// </summary>
    /// <param name="fileName"></param>
    public void PlayAmbience(string fileName) {
        AudioClip ac = _keyToAudio[fileName.ToUpper()];
        ambienceSource.clip = ac;
        ambienceSource.Play();
    }
}