using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages audio sources and volume levels in Lamplight.
/// </summary>
public class AudioManager : Singleton<AudioManager>
{
    [HideInInspector] public AudioDatabase audioDatabase;

    [HideInInspector] public AudioSettingsData audioSettings;

    private List<AudioSource> audioSourcePool = new List<AudioSource>();

    private void Start()
    {
        audioDatabase = new AudioDatabase();
        audioDatabase.LoadData();
        audioSettings = new AudioSettingsData();
    }


    /// <summary>
    /// Plays a sound effect in 2D space.
    /// </summary>
    /// <param name="sound">The sound effect to be played.</param>
    public void PlaySound(SoundData sound)
    {
        AudioSource audioSource = SetupAudioSource();
        audioSource.transform.position = Camera.main.transform.position;
        audioSource.transform.parent = Camera.main.transform;
        audioSource.spatialBlend = 0;
        float volume = sound.gain * audioSettings.masterVolume * GetVolumeLevelForCategory(sound.category);
        audioSource.PlayOneShot(sound.audioClip, volume);
    }


    /// <summary>
    /// Plays a sound effect in 3D space.
    /// </summary>
    /// <param name="sound">The sound effect to be played.</param>
    /// <param name="position">The position in world space where the sfx is played.</param>
    public void PlaySound(SoundData sound, Vector3 position)
    {
        AudioSource audioSource = SetupAudioSource();
        audioSource.transform.position = position;
        audioSource.transform.parent = null;
        audioSource.spatialBlend = 1;
        float volume = sound.gain * audioSettings.masterVolume * GetVolumeLevelForCategory(sound.category);
        audioSource.PlayOneShot(sound.audioClip, volume);
    }

    /// <summary>
    /// Plays a sound effect in 3D space.
    /// </summary>
    /// <param name="sound">The sound to be played.</param>
    /// <param name="parent">The object which the audiosoure will be a child to.</param>
    public void PlaySound(SoundData sound, Transform parent)
    {
        AudioSource audioSource = SetupAudioSource();
        audioSource.transform.position = parent.position;
        audioSource.transform.parent = parent;
        audioSource.spatialBlend = 1;
        float volume = sound.gain * audioSettings.masterVolume * GetVolumeLevelForCategory(sound.category);
        audioSource.PlayOneShot(sound.audioClip, volume);
    }


    /// <summary>
    /// Gets the first available audio source from the audio source pool, or creates a new one if none are available.
    /// </summary>
    /// <returns>AudioSource</returns>
    private AudioSource SetupAudioSource()
    {
        AudioSource sourceAvailable = null;

        for(int i = 0; i < audioSourcePool.Count; i++)
        {
            if(audioSourcePool[i].isPlaying == false)
            {
                sourceAvailable = audioSourcePool[i];
            }
        }

        if(sourceAvailable == null)
        {
            GameObject gameObject = new GameObject("Audio Source");
            sourceAvailable = gameObject.AddComponent<AudioSource>();
            audioSourcePool.Add(sourceAvailable);
        }

        return sourceAvailable;
    }


    /// <summary>
    /// Gets the current volume level setting for the given audio category.
    /// </summary>
    /// <returns>float</returns>
    public float GetVolumeLevelForCategory(AudioCategory category)
    {
        if(category == AudioCategory.Master)
        {
            return audioSettings.masterVolume;
        }
        if(category == AudioCategory.Dialogue)
        {
            return audioSettings.dialogueVolume;
        }
        else if(category == AudioCategory.Music)
        {
            return audioSettings.musicVolume;
        }
        else if(category == AudioCategory.SFX)
        {
            return audioSettings.sfxVolume;
        }
        else if(category == AudioCategory.Ambient)
        {
            return audioSettings.ambientVolume;
        }
        else
        {
            Debug.LogWarning("Playing audio of unknown category: " + category);
            return 0.5f;
        }
    }

    public void SetVolume(float volume, AudioCategory category)
    {
        Debug.Log("Setting Volume to " + volume);

        if(category == AudioCategory.Master)
        {
            audioSettings.masterVolume = volume;
            Debug.Log(category.ToString() + ": " + audioSettings.masterVolume);
        }
        else if (category == AudioCategory.Dialogue)
        {
            audioSettings.dialogueVolume = volume;
            Debug.Log(category.ToString() + ": " + audioSettings.dialogueVolume);
        }
        else if (category == AudioCategory.Music)
        {
            audioSettings.musicVolume = volume;
            Debug.Log(category.ToString() + ": " + audioSettings.musicVolume);
        }
        else if (category == AudioCategory.SFX)
        {
            audioSettings.sfxVolume = volume;
            Debug.Log(category.ToString() + ": " + audioSettings.sfxVolume);
        }
        else if (category == AudioCategory.Ambient)
        {
            audioSettings.ambientVolume = volume;
            Debug.Log(category.ToString() + ": " + audioSettings.ambientVolume);
        }
        else
        {
            Debug.LogWarning("Trying to set audio of unknown category: " + category);
        }
    }

    public void ResetAudioSettings()
    {
        audioSettings = new AudioSettingsData();
    }

    public void SetSubtitlesSetting(bool subtitlesEnabled)
    {
        audioSettings.subtitles = subtitlesEnabled;
    }
}
