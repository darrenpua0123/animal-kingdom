using UnityEngine.Audio;
using UnityEngine;
using System;
using Unity.VisualScripting;

public enum SoundName 
{ 
    ButtonPressed,
    MainMenu,
    GameMatch,
    SufficientPurchased,
    InsufficientPurchase,
    ViewCard,
    PlayCard,
    EndTurnButton,
    ActionNotEndable,
    CollectRelicCard
}

public class AudioManagerScript : MonoBehaviour
{
    public static AudioManagerScript instance;

    public Sound[] sounds;

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

        DontDestroyOnLoad(gameObject);

        // Set the sound variable
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.playOnAwake = sound.autoPlay;
            sound.source.loop = sound.loop;
        }
    }

    private void Update()
    {
        // Set the sound variable
        foreach (Sound sound in sounds)
        {
            sound.isPlaying = sound.source.isPlaying;
        }
    }

    public void Play(SoundName name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if (sound == null)
        {
            Debug.LogWarning($"{name} was not found to play.");
            return;
        }

        sound.source.Play();
    }

    public void Stop(SoundName name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if (sound == null)
        {
            Debug.LogWarning($"{name} was not found to stop.");
            return;
        }

        sound.source.Stop();
    }

    public Sound GetSound(SoundName name) 
    {
        Sound sound = Array.Find(instance.sounds, sound => sound.name == SoundName.MainMenu);
        return sound;
    }
}
