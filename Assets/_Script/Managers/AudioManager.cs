using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundList
{
    FootstepSound,
    MonsterSound,
    ButtonSound,
    DoorSound,
    CardSound,
    MapSound,
    UISound,
    FlashlightSound,
    ProjectileTrapSound,
    NoteSound,
    WinSound,
    LoseSound,
    CardAbilitySound

}
[Serializable]
public class SoundEntry
{
    public string name;
    public SoundList soundType;

    [Tooltip("SoundClips")]
    public List<AudioClip> clips = new();

    [Range(0f, 1f)]
    public float volume = 1f;
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<SoundEntry> soundEntries = new();

    private Dictionary<SoundList, SoundEntry> soundDict = new();

    public static AudioManager Instance { get; private set; }

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (var entry in soundEntries)
        {
            if (!soundDict.ContainsKey(entry.soundType))
                soundDict.Add(entry.soundType, entry);
        }
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }
    public void PlaySound(SoundList soundType)
    {
        if (soundDict.TryGetValue(soundType, out SoundEntry entry))
        {
            if (entry.clips.Count == 0) return;

            AudioClip randomClip = entry.clips[UnityEngine.Random.Range(0, entry.clips.Count)];
            _audioSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
            _audioSource.PlayOneShot(randomClip, entry.volume);
        }
    }
    public void PlaySound(SoundList soundType, float spatialVolume, AudioSource audioSource)
    {
        if (soundDict.TryGetValue(soundType, out SoundEntry entry))
        {
            if (entry.clips.Count == 0) return;

            AudioClip randomClip = entry.clips[UnityEngine.Random.Range(0, entry.clips.Count)];
            audioSource.clip = randomClip;
            audioSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
            audioSource.spatialBlend = spatialVolume;
            audioSource.PlayOneShot(randomClip, entry.volume);
        }
    }
}