using UnityEngine;

public class AudioManager : Singleton<AudioManager> 
{
    [SerializeField] private SoundEffect[] soundEffects;

    private void Awake() {
        foreach (var s in soundEffects) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.playOnAwake = false;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.clip = s.clip;
        }
    }

    public void PlaySound(string name) {
        SoundEffect s = System.Array.Find(soundEffects, sound => sound.name == name);
        if (s != null) {
            s.source.Play();
        }
        else {
            Debug.LogWarning($"SoundEffect with name {name} not found!");
        }

    }

    public void PlaySound(string name, bool randomPitch) {
        SoundEffect s = System.Array.Find(soundEffects, sound => sound.name == name);
        if (s != null) {
            if (randomPitch)
                s.source.pitch = Random.Range(s.pitchMin, s.pitchMax);
            s.source.Play();
        }
        else {
            Debug.LogWarning($"SoundEffect with name {name} not found!");
        }
    }


}

[System.Serializable]
public class SoundEffect 
{
    [HideInInspector] public AudioSource source; 
    public string name; 
    public AudioClip clip;
    [Range(0, 1)] public float volume = 1f;
    [Range (0.2f, 2f)] public float pitch = 1f;   
    public float pitchMin = 0.8f; 
    public float pitchMax = 1.2f; 
}

