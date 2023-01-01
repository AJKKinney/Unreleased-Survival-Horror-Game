using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AustenKinney.AudioSystem
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Song", menuName = "Create Song", order = 51)]
    public class SongData : ScriptableObject
    {
        [Tooltip("The audio clip which will be played in the game.")]
        [SerializeField] private List<Track> tracks;

        [Tooltip("The adjusted volume of the audioclip.")]
        [Range(0, 1)]
        [SerializeField] private float gain;

        [Tooltip("The song's category. This is used to seperate audio for audio mixing settings")]
        [SerializeField] private AudioCategory category = AudioCategory.Music;

        #region Getters & Setters

        /// <summary>
        /// The song tracks which will be played simultaneously in the game.
        /// </summary>
        public List<Track> Tracks { get { return tracks; } }

        /// <summary>
        /// The adjusted volume of the song.
        /// </summary>
        public float Gain { get { return gain; } }

        /// <summary>
        /// The sound's category. This is used to seperate audio for audio mixing settings.
        /// </summary>
        public AudioCategory Category { get { return category; } }

        #endregion
    }
}
