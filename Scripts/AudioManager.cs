using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] soundEffects;
    int num = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);

        foreach (Sound s in soundEffects)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        PlaySFX("Music");
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(soundEffects, sound => sound.name == name);
        s.source.Play();
    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(soundEffects, sound => sound.name == name);
        s.source.Stop();
    }

    // -----------------------------------------------------------------------------

    /*
    public Audio[] soundEffects;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);

        foreach (Audio a in soundEffects)
        {
            a.source = gameObject.AddComponent<AudioSource>();
            a.source.clip = a.clip;

            a.source.volume = a.volume;
            a.source.pitch = a.pitch;
        }
    }

    public void PlaySFX(string soundName)
    {
        
        for (int i = 0; i < soundEffects.Length; i++)
        {
            if (soundEffects[i].name == soundName)
            {
                soundEffects[i].Play();
                return; 
            }
        }
        

        Debug.Log(soundName + " not in list.");
    }
    */
}
