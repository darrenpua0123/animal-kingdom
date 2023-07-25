using UnityEngine;

[System.Serializable]
public class Sound 
{
    public SoundName name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    
    [Range(.1f, 3)]
    public float pitch;

    public bool autoPlay;
    public bool loop;

    [HideInInspector] public AudioSource source;
    [HideInInspector] public bool isPlaying;
}