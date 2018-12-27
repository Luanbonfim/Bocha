//Script responsible to set the audio features that will be shown on the inspector to setup all the audios from the Audio Manager.
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;
    
    [HideInInspector]
    public AudioSource Source;
}
