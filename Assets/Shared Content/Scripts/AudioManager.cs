using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System;

public class AudioManager : MonoBehaviour {

    public SoundCollection soundCollection;

    public static AudioManager instance;
    public static GameObject instanceGameObject;

    private string musicPlaying;

    private void Awake()
    {
        if (instance == null)
        {
            instanceGameObject = this.gameObject;
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            foreach (Sound s in soundCollection.sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.playOnAwake = false;
            }
        } else {          
           Destroy(this.gameObject);
        }
    }

    public void Play(string name) {
        Sound s = GetSoundByName(name);
        if (s.randomPitch)
        {
            s.source.pitch = UnityEngine.Random.Range(s.randomMin, s.randomMax);
        }

        if (s.loop) {
            if (HasMusicPlaying())
            {
                StopMusic();
            }
            StoreMusic(s.name);
        }

        // Debug.Log("Play Sound " + s.name + ", Loop: " + s.loop);
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = GetSoundByName(name);
        if (s == null)
        {
            Debug.LogWarning("Try to play sound that not exists. sound name: " + name);
            return;
        }        
        s.source.Stop();
        // musicPlaying = null;
    }

    public void StoreMusic(string name) {
        // Debug.Log("Stored Music " + name);
        musicPlaying = name;
    }

    public void StopMusic() {
        if (HasMusicPlaying())
        {
            // Debug.Log("Stop Music " + musicPlaying);
            Stop(musicPlaying);
        }
    }

    bool HasMusicPlaying()
    {
        return musicPlaying != null;
    }

    public bool HasSound(string name) {
        return GetSoundByName(name) != null;
    }

    public Sound GetSoundByName(string name)
    {
        Sound s = Array.Find(soundCollection.sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Try to get sound that not exists. sound name: " + name);
        }     
        return s;
    }
}
