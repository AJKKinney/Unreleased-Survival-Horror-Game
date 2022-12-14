using UnityEngine;

namespace AustenKinney.AudioSystem
{
    [System.Serializable]
    public class AudioSettingsData
    {

        [Header("Audio Settings")]

        [Tooltip("The master volume level")]
        private float masterVolume = 1f;
        [Tooltip("The music volume level")]
        private float musicVolume = 0.5f;
        [Tooltip("The sound effects volume level")]
        private float sfxVolume = 0.5f;
        [Tooltip("The dialogue volume level")]
        private float dialogueVolume = 0.5f;
        [Tooltip("The ambient volume level")]
        private float ambientVolume = 0.5f;

        [Tooltip("Determines whether subtitles are enabled")]
        private bool subtitles = false;

        #region Getters & Setters

        /// <summary>
        /// The master volume level
        /// </summary>
        public float MasterVolume { get { return masterVolume; } set { masterVolume = value; } }
        /// <summary>
        /// The music volume level
        /// </summary>
        public float MusicVolume { get { return musicVolume; } set { musicVolume = value; } }
        /// <summary>
        /// The sfx volume level
        /// </summary>
        public float SFXVolume { get { return sfxVolume; } set { sfxVolume = value; } }
        /// <summary>
        /// The dialogue volume level
        /// </summary>
        public float DialogueVolume { get { return dialogueVolume; } set { dialogueVolume = value; } }
        /// <summary>
        /// The ambient volume level
        /// </summary>
        public float AmbientVolume { get { return ambientVolume; } set { ambientVolume = value; } }

        /// <summary>
        /// Determines whether subtitles are enabled
        /// </summary>
        public bool Subtitles { get { return subtitles; } set { subtitles = value; } }

        #endregion

        #region Constructors
        public AudioSettingsData()
        {

        }

        public AudioSettingsData(float setMasterVolume, float setMusicVolume, float setSFXVolume, float setDialogueVolume, float setAmbientVolume, bool setSubtitles)
        {
            masterVolume = setMasterVolume;
            musicVolume = setMusicVolume;
            sfxVolume = setSFXVolume;
            dialogueVolume = setDialogueVolume;
            ambientVolume = setDialogueVolume;
            subtitles = setSubtitles;
        }
        #endregion
    }
}
