using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioSettingsData
{

    [Header("Audio Settings")]

    public float masterVolume = 1f;
    public float musicVolume = 0.5f;
    public float sfxVolume = 0.5f;
    public float dialogueVolume = 0.5f;
    public float ambientVolume = 0.5f;

    public bool subtitles = false;

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
}
