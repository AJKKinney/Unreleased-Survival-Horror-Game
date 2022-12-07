using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Holds references to pause menu panels and contains functionality to open and close them.
/// </summary>
public class PauseMenuManager : Singleton<PauseMenuManager>
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

    private void Start()
    {
        audioManager = AudioManager.instance;
    }

    public void OpenPausePanel()
    {
        pausePanel.SetActive(true);
    }

    public void ClosePausePanel()
    {
        pausePanel.SetActive(false);
    }

    public void OpenSettingsPanel()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }

    public void OpenExitPanel()
    {
        exitPanel.SetActive(true);
    }

    public void CloseExitPanel()
    {
        exitPanel.SetActive(false);
    }

    public void CloseAllPausePanels()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        exitPanel.SetActive(false);
    }

    #region Settings Functions

    public void SetMasterVolume()
    {
        audioManager.SetVolume(masterVolumeSlider.value, AudioCategory.Master);
    }

    public void SetDialogueVolume()
    {
        audioManager.SetVolume(dialogueVolumeSlider.value, AudioCategory.Dialogue);
    }

    public void SetMusicVolume()
    {
        audioManager.SetVolume(musicVolumeSlider.value, AudioCategory.Music);
    }

    public void SetAmbientVolume()
    {
        audioManager.SetVolume(ambientVolumeSlider.value, AudioCategory.Ambient);
    }

    public void SetSFXVolume()
    {
        audioManager.SetVolume(sfxVolumeSlider.value, AudioCategory.SFX);
    }

    public void ResetToDefaultAudio()
    {
        audioManager.ResetAudioSettings();
        masterVolumeSlider.value = audioManager.GetVolumeLevelForCategory(AudioCategory.Master);
        sfxVolumeSlider.value = audioManager.GetVolumeLevelForCategory(AudioCategory.SFX);
        musicVolumeSlider.value = audioManager.GetVolumeLevelForCategory(AudioCategory.Music);
        ambientVolumeSlider.value = audioManager.GetVolumeLevelForCategory(AudioCategory.Ambient);
        dialogueVolumeSlider.value = audioManager.GetVolumeLevelForCategory(AudioCategory.Dialogue);
        subtitlesToggle.isOn = audioManager.audioSettings.subtitles;
    }

    public void SetSubtitles()
    {
        audioManager.SetSubtitlesSetting(subtitlesToggle.isOn);
    }

    #endregion
}
