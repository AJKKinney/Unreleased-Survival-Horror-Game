using UnityEngine;

namespace AustenKinney.SoundMaster
{
    [System.Serializable]
    public class AudioSettingsData
    {

        [Header("Audio Settings")]

        private float masterVolume = 1f;
        private float musicVolume = 0.5f;
        private float sfxVolume = 0.5f;
        private float dialogueVolume = 0.5f;
        private float ambientVolume = 0.5f;

        private bool subtitles = false;

        #region Getters & Setters

        public float MasterVolume { get { return masterVolume; } set { masterVolume = value; } }
        public float MusicVolume { get { return musicVolume; } set { musicVolume = value; } }
        public float SFXVolume { get { return sfxVolume; } set { sfxVolume = value; } }
        public float DialogueVolume { get { return dialogueVolume; } set { dialogueVolume = value; } }
        public float AmbientVolume { get { return ambientVolume; } set { ambientVolume = value; } }

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
