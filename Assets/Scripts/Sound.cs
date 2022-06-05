using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {
    // The audio clip file, such as a .mp3 file
    public AudioClip clip;

    // Name of the audio clip
    public string name;

    // The volume of the clip (0-1)
    [Range(0f, 1f)]
    public float volume;

    // The pitch of the clip (0.1 - 3)
    [Range(.1f, 3f)]
    public float pitch;

    // Delay time specified in seconds
    public float delay;

    // If true, the clip loops. If flase, the clip only play once.  
    public bool loop = false;

    [HideInInspector]
    // Audio source
    public AudioSource source;
}
