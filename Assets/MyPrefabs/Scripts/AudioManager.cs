using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;


    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;
            s.source.volume = s.Volume;
            s.source.pitch = s.Pitch;
        }
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds,sound =>sound.Name == name);
        s.source.Play();
    }
    







}
