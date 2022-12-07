using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Contains all the data needed for a sound effect or song.
/// </summary>
[CreateAssetMenu(fileName = "New Sound", menuName = "Create Sound", order = 50)]
public class SoundData : ScriptableObject
{
    [Tooltip("The audio clip which will be played in the game.")]
    public AudioClip audioClip;

    [Tooltip("The adjusted volume of the audioclip.")]
    [Range(0,1)]
    public float gain;

    [Tooltip("The sound's category. This is used to seperate audio for audio mixing settings")]
    public AudioCategory category = AudioCategory.SFX;
}


/// <summary>
/// The different Categories of audio. These are used to seperate the game audio for mixing in the audio settings.
/// </summary>
public enum AudioCategory
{
    Master,
    Dialogue,
    Music,
    SFX,
    Ambient
}
