using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    // Start is called before the first frame update
    void Awake() {
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
        }
    }

    // Find and play the sound by name
    public void Play(string name) {
        // Find the sound in the array
        Sound s = System.Array.Find(sounds, sound => sound.name == name);

        if(s == null){
            // Sound is not found, log error and terminate.
            Debug.LogWarning("Audio clip " + name + " not found!!");
            return;
        }

        // Play the audio by the delay
        s.source.PlayDelayed(s.delay);
    }
}
