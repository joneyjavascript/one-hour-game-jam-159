using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    public bool loop;

    [Range(0, 1f)]
    public float volume;

    [Range(0, 1f)]
    public float pitch;
    public bool randomPitch;
    public float randomMin;
    public float randomMax;

    [HideInInspector]
    public AudioSource source;

}
