using UnityEngine;
using UnityEngine.UI;
using AustenKinney.Essentials;
using AustenKinney.AudioSystem;
using AustenKinney.GameState;
using Lamplight.Input;

namespace Lamplight.UI
{
    /// <summary>
    /// Manages Menu Systems for in-game menus. Holds references to pause menu panels and contains functionality to open and close them.
    /// </summary>
    public class GameMenuManager : Singleton<GameMenuManager>
    {
        [Header("Menus")]
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject exitPanel;

        [Header("Settings UI")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider dialogueVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Slider ambientVolumeSlider;
        [SerializeField] private Toggle subtitlesToggle;

        private AudioManager audioManager;

        private bool menuEnabled;

        #region Initialization

        public override void Init()
        {
            audioManager = AudioManager.Instance;
            InputProvider.playerActions.GameActions.Menu.started += _ => ToggleMenu();
        }

        private void Start()
        {
            GameStateMaster.SetState(GameState.Gameplay);
        }

        #endregion

        #region UI Methods

        public void ToggleMenu()
        {
            if (menuEnabled == false)
            {
                GameStateMaster.SetState(GameState.Paused);
                OpenPauseMenu();
                menuEnabled = true;
            }
            else
            {
                GameStateMaster.SetState(GameState.Gameplay);
                CloseAllMenus();
                menuEnabled = false;
            }
        }

        public void OpenPauseMenu()
        {
            pausePanel.SetActive(true);
        }

        public void ClosePauseMenu()
        {
            pausePanel.SetActive(false);
        }

        public void OpenSettingsMenu()
        {
            settingsPanel.SetActive(true);
        }

        public void CloseSettingsMenu()
        {
            settingsPanel.SetActive(false);
        }

        public void OpenExitMenu()
        {
            exitPanel.SetActive(true);
        }

        public void CloseExitMenu()
        {
            exitPanel.SetActive(false);
        }

        public void CloseAllMenus()
        {
            pausePanel.SetActive(false);
            settingsPanel.SetActive(false);
            exitPanel.SetActive(false);
        }

        public void QuitGame()
        {
            GameStateMaster.SetState(GameState.Quit);
        }

        #endregion

        #region Settings Methods

        public void SetMasterVolume()
        {
            audioManager.Settings.MasterVolume = masterVolumeSlider.value;
        }

        public void SetDialogueVolume()
        {
            audioManager.Settings.DialogueVolume = dialogueVolumeSlider.value;
        }

        public void SetMusicVolume()
        {
            audioManager.Settings.MusicVolume = musicVolumeSlider.value;
        }

        public void SetAmbientVolume()
        {
            audioManager.Settings.AmbientVolume = ambientVolumeSlider.value;
        }

        public void SetSFXVolume()
        {
            audioManager.Settings.SFXVolume = sfxVolumeSlider.value;
        }

        public void ResetToDefaultAudio()
        {
            audioManager.ResetAudioSettings();
            masterVolumeSlider.value = audioManager.Settings.MasterVolume;
            sfxVolumeSlider.value = audioManager.Settings.SFXVolume;
            musicVolumeSlider.value = audioManager.Settings.MusicVolume;
            ambientVolumeSlider.value = audioManager.Settings.AmbientVolume;
            dialogueVolumeSlider.value = audioManager.Settings.DialogueVolume;
            subtitlesToggle.isOn = audioManager.Settings.Subtitles;
        }

        public void SetSubtitles()
        {
            audioManager.SetSubtitlesSetting(subtitlesToggle.isOn);
        }

        #endregion
    }
}
