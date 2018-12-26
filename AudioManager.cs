//Script responsible for managing all the audios in the game.
using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

     void Awake()
    {
      foreach (Sound s in sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.clip;
            s.Source.volume = s.volume;
            s.Source.pitch = s.pitch;
        }
    }

    public void play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.Source.Play();
    }
}
