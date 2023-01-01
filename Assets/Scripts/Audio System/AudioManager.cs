using System.Collections.Generic;
using System.Collections;
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

        private List<AudioSource> sfxSourcePool = new List<AudioSource>();
        private List<AudioSource> musicSourcePool = new List<AudioSource>();
        private List<AudioSource> ambientSourcePool = new List<AudioSource>();
        private List<AudioSource> dialogueSourcePool = new List<AudioSource>();

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

        #region Audio Methods & Coroutines

        /// <summary>
        /// Plays a sound effect in 2D space.
        /// </summary>
        /// <param name="sound">The sound to be played.</param>
        public void PlaySound(SoundData sound)
        {
            AudioSource audioSource = SetupAudioSource(GetAudioPool(sound.Category));
            audioSource.transform.position = Camera.main.transform.position;
            audioSource.transform.parent = Camera.main.transform;
            float volume = sound.Gain * settings.MasterVolume * GetVolumeLevelForCategory(sound.Category);
            audioSource.PlayOneShot(sound.Clip, volume);
        }


        /// <summary>
        /// Plays a sound effect in 3D space.
        /// </summary>
        /// <param name="sound">The sound to be played.</param>
        /// <param name="position">The position in world space where the sfx is played.</param>
        public void PlaySound(SoundData sound, Vector3 position)
        {
            AudioSource audioSource = SetupAudioSource(GetAudioPool(sound.Category));
            audioSource.transform.parent = null;
            audioSource.transform.position = position;
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
            AudioSource audioSource = SetupAudioSource(GetAudioPool(sound.Category));
            audioSource.transform.position = parent.position;
            audioSource.transform.parent = parent;
            audioSource.spatialBlend = 1;
            float volume = sound.Gain * settings.MasterVolume * GetVolumeLevelForCategory(sound.Category);
            audioSource.PlayOneShot(sound.Clip, volume);
        }

        /// <summary>
        /// Plays a looping sound in 2D space.
        /// </summary>
        /// <param name="sound">The sound to be played.</param>
        public void PlayLoop(SoundData sound, bool setVolume = true)
        {
            AudioSource audioSource = SetupAudioSource(GetAudioPool(sound.Category));
            audioSource.transform.position = Camera.main.transform.position;
            audioSource.transform.parent = Camera.main.transform;
            audioSource.loop = true;
            float volume = sound.Gain * settings.MasterVolume * GetVolumeLevelForCategory(sound.Category);

            if (setVolume == true)
            {
                audioSource.volume = volume;
            }

            audioSource.clip = sound.Clip;
            audioSource.Play();
        }

        /// <summary>
        /// Plays a looping song in 2D space.
        /// </summary>
        /// <param name="song">The song loop to be played</param>
        /// <param name="setVolume">Determines whether the volume should be set to the level set in the songData.</param>
        public void PlayLoop(SongData song, bool setVolume = true)
        {
            for (int i = 0; i < song.Tracks.Count; i++)
            {
                AudioSource audioSource = SetupAudioSource(GetAudioPool(song.Category));
                audioSource.transform.position = Camera.main.transform.position;
                audioSource.transform.parent = Camera.main.transform;
                audioSource.loop = true;
                float volume = song.Gain * settings.MasterVolume * GetVolumeLevelForCategory(song.Category);

                if (setVolume == true)
                {
                    audioSource.volume = volume;
                }


                audioSource.clip = song.Tracks[i].Clips[0];
                audioSource.Play();

                if (i != 0)
                {
                    SyncSources(song.Category);
                }
            }
        }

        /// <summary>
        /// Fades out the current playing song, and fades in the new song. This is the recommended way of playing songs.
        /// </summary>
        /// <param name="song">The song to be played.</param>
        /// <param name="transitionLength">How long in seconds it will take to transition fully.</param>
        public IEnumerator TransitionSong(SongData song, float transitionLength)
        {
            Debug.Log("Transitioning songs...");

            yield return StartCoroutine(FadeOutSong(song, transitionLength/2));

            yield return StartCoroutine(FadeInSong(song, transitionLength / 2));
            Debug.Log("Song transition completed. Now playing: " + song.name);
        }

        public IEnumerator FadeOutSong(SongData song, float transitionLength)
        {
            List<AudioSource> pool = GetAudioPool(song.Category);

            if (pool.Count > 0 && pool[0].isPlaying == true)
            {
                for (int i = 0; i < pool.Count; i++)
                {
                    StartCoroutine(Fade(pool[i], pool[i].volume, 0, transitionLength / 2));
                }

                yield return new WaitForSeconds(transitionLength / 2);

                for (int i = 0; i < pool.Count; i++)
                {
                    pool[i].Stop();
                }
            }
        }

        public IEnumerator FadeOutTrack(SongData song, int track, float transitionLength)
        {

            List<AudioSource> pool = GetAudioPool(song.Category);

            if (pool.Count > 0 && pool[track].isPlaying == true)
            {
                StartCoroutine(Fade(pool[track], pool[track].volume, 0, transitionLength / 2));


                yield return new WaitForSeconds(transitionLength / 2);

                pool[track].Stop();
            }
        }

        public IEnumerator FadeInSong(SongData song, float transitionLength)
        {
            List<AudioSource> pool = GetAudioPool(song.Category);

            float volume = song.Gain * settings.MasterVolume * GetVolumeLevelForCategory(song.Category);

            PlayLoop(song, false);

            for (int i = 0; i < pool.Count; i++)
            {
                pool[i].volume = 0;
                StartCoroutine(Fade(pool[i], pool[i].volume, volume, transitionLength / 2));
            }

            yield return new WaitForSeconds(transitionLength / 2);
        }

        public IEnumerator FadeInTrack(SongData song, int track, float transitionLength)
        {
            List<AudioSource> pool = GetAudioPool(song.Category);

            SetupAudioSource(pool);

            float volume = song.Gain * settings.MasterVolume * GetVolumeLevelForCategory(song.Category);

            pool[track].volume = 0;
            StartCoroutine(Fade(pool[track], pool[track].volume, volume, transitionLength / 2));

            yield return new WaitForSeconds(transitionLength / 2);
        }

        /// <summary>
        /// Fades out the current playing track clip, and fades in the new track clip. This is the recommended way of changing clips for an adaptive soundtrack.
        /// </summary>
        /// <param name="song">The song to be played.</param>
        /// <param name="track">The index of the track to transition</param>
        /// <param name="clip">The index of the clip of the song on the given track to be played</param>
        /// <param name="transitionLength">How long in seconds it will take to transition fully.</param>
        public IEnumerator TransitionTrack(SongData song, int track, int clip, float transitionLength)
        {
            yield return StartCoroutine(FadeOutTrack(song, track, transitionLength / 2));

            yield return StartCoroutine(FadeInTrack(song, track, transitionLength / 2));
        }

        /// <summary>
        /// Syncs all the sources within an audio source pool of a given category with the 1st audio source
        /// </summary>
        /// <param name="sourcePool">The audio source pool which is to be synced</param>
        public void SyncSources(AudioCategory category)
        {
            List<AudioSource> sourcePool = GetAudioPool(category);

            for(int i = 0; i < sourcePool.Count; i++)
            {
                if(i > 0)
                {
                    sourcePool[i].time = sourcePool[0].time;
                }
            }
        }


        /// <summary>
        /// Fades the volume of the given audio source to the target vvolume
        /// </summary>
        /// <param name="source">The audio source which is to be faded out.</param>
        /// <param name="targetVolume">The volume level which the audio source's volume will be set to.</param>
        /// <param name="fadeTime">The amount of time it takes for the volume level to fade to the target volume</param>
        /// <param name="timer">The current value for the timer. Set to 0 when starting a fade.</param>
        /// <returns></returns>
        public IEnumerator Fade(AudioSource source, float startVolume, float targetVolume, float fadeTime, float timer = 0)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;

            //Debug.Log("Timer: " + timer + ". Transition: " + (timer / fadeTime));

            source.volume = Mathf.Lerp(startVolume, targetVolume, timer / fadeTime);

            if(source.volume != targetVolume)
            {
                yield return Fade(source, startVolume, targetVolume, fadeTime, timer);
            }
            else
            {
                yield return null;
            }
        }

        #endregion

        #region Audio Source Pooling

        /// <summary>
        /// Gets the first available audio source from the correct audio source pool, or creates a new one if none are available.
        /// </summary>
        /// <param name="category">The category of sounds which the audio source will play.</param>
        /// <returns>AudioSource</returns>
        private AudioSource SetupAudioSource(List<AudioSource> sourcePool)
        {
            AudioSource sourceAvailable = null;

            for (int i = 0; i < sourcePool.Count; i++)
            {
                if (sourcePool[i].isPlaying == false)
                {
                    sourceAvailable = sourcePool[i];
                }
            }

            if (sourceAvailable == null)
            {
                GameObject gameObject = new GameObject("Audio Source");
                sourceAvailable = gameObject.AddComponent<AudioSource>();
                sourcePool.Add(sourceAvailable);
            }
           
            return sourceAvailable;
        }

        private List<AudioSource> GetAudioPool(AudioCategory category)
        {
            if(category == AudioCategory.SFX)
            {
                return sfxSourcePool;
            }
            else if(category == AudioCategory.Music)
            {
                return musicSourcePool;
            }
            else if(category == AudioCategory.Dialogue)
            {
                return dialogueSourcePool;
            }
            else if(category == AudioCategory.Ambient)
            {
                return ambientSourcePool;
            }
            else
            {
                Debug.LogWarning("The Audio Pool has not been implemented yet " + category.ToString());
                return null;
            }
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

        #region Deprecated Methods

        /// <summary>
        /// Sets the volume of all audio assigned to a category. Use the setter instead.
        /// </summary>
        /// <param name="volume">The volume of the audio which is played in the assigned category</param>
        /// <param name="category">The category of audio whose volume is to be set</param>
        [System.Obsolete("deprecated")]
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

        [System.Obsolete("deprecated")]
        public void SetMasterVolume(float volume)
        {
            settings.MasterVolume = volume;
        }

        #endregion
    }
}
