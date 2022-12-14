using System.Collections.Generic;
using UnityEngine;
using AustenKinney.Essentials;

namespace AustenKinney.AudioSystem
{
    /// <summary>
    /// Manages audio sources and volume levels in Lamplight.
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {
        private AudioSettingsData settings;
        private AudioDatabase database;
        private List<AudioSource> audioSourcePool = new List<AudioSource>();

        #region Getters & Setters

        /// <summary>
        /// The current audio settings data.
        /// </summary>
        public AudioSettingsData Settings { get { return settings; } }

        /// <summary>
        /// The database of sounds.
        /// </summary>
        public AudioDatabase Database { get { return database; } }

        #endregion

        #region Initialization

        public override void Init()
        {
            database = new AudioDatabase();
            database.LoadData();
            settings = new AudioSettingsData();
        }

        #endregion

        #region Audio Methods

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
            float volume = sound.Gain * settings.MasterVolume * GetVolumeLevelForCategory(sound.Category);
            audioSource.PlayOneShot(sound.Clip, volume);
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
            float volume = sound.Gain * settings.MasterVolume * GetVolumeLevelForCategory(sound.Category);
            audioSource.PlayOneShot(sound.Clip, volume);
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
            float volume = sound.Gain * settings.MasterVolume * GetVolumeLevelForCategory(sound.Category);
            audioSource.PlayOneShot(sound.Clip, volume);
        }

        #endregion

        #region Audio Source Pooling

        /// <summary>
        /// Gets the first available audio source from the audio source pool, or creates a new one if none are available.
        /// </summary>
        /// <returns>AudioSource</returns>
        private AudioSource SetupAudioSource()
        {
            AudioSource sourceAvailable = null;

            for (int i = 0; i < audioSourcePool.Count; i++)
            {
                if (audioSourcePool[i].isPlaying == false)
                {
                    sourceAvailable = audioSourcePool[i];
                }
            }

            if (sourceAvailable == null)
            {
                GameObject gameObject = new GameObject("Audio Source");
                sourceAvailable = gameObject.AddComponent<AudioSource>();
                audioSourcePool.Add(sourceAvailable);
            }

            return sourceAvailable;
        }

        #endregion

        #region Settings Methods

        /// <summary>
        /// Gets the current volume level setting for the given audio category.
        /// </summary>
        /// <returns>float</returns>
        public float GetVolumeLevelForCategory(AudioCategory category)
        {
            if (category == AudioCategory.Dialogue)
            {
                return settings.DialogueVolume;
            }
            else if (category == AudioCategory.Music)
            {
                return settings.MusicVolume;
            }
            else if (category == AudioCategory.SFX)
            {
                return settings.SFXVolume;
            }
            else if (category == AudioCategory.Ambient)
            {
                return settings.AmbientVolume;
            }
            else
            {
                Debug.LogWarning("Playing audio of unknown category: " + category);
                return 0.5f;
            }
        }

        /// <summary>
        /// Sets the volume of all audio assigned to a category.
        /// </summary>
        /// <param name="volume">The volume of the audio which is played in the assigned category</param>
        /// <param name="category">The category of audio whose volume is to be set</param>
        public void SetCategoryVolume(float volume, AudioCategory category)
        {
            Debug.Log("Setting Volume to " + volume);

            if (category == AudioCategory.Dialogue)
            {
                settings.DialogueVolume = volume;
                Debug.Log(category.ToString() + ": " + settings.DialogueVolume);
            }
            else if (category == AudioCategory.Music)
            {
                settings.MusicVolume = volume;
                Debug.Log(category.ToString() + ": " + settings.MusicVolume);
            }
            else if (category == AudioCategory.SFX)
            {
                settings.SFXVolume = volume;
                Debug.Log(category.ToString() + ": " + settings.SFXVolume);
            }
            else if (category == AudioCategory.Ambient)
            {
                settings.AmbientVolume = volume;
                Debug.Log(category.ToString() + ": " + settings.AmbientVolume);
            }
            else
            {
                Debug.LogWarning("Trying to set audio of unknown category: " + category);
            }
        }

        public void SetMasterVolume(float volume)
        {
            settings.MasterVolume = volume;
        }

        /// <summary>
        /// Sets the audio settings to their default settings.
        /// </summary>
        public void ResetAudioSettings()
        {
            settings = new AudioSettingsData();
        }

        /// <summary>
        /// Enables or Disables the subtitles.
        /// </summary>
        /// <param name="subtitlesEnabled">Determines whether subtitles are enabled</param>
        public void SetSubtitlesSetting(bool subtitlesEnabled)
        {
            settings.Subtitles = subtitlesEnabled;
        }

        #endregion
    }
}
