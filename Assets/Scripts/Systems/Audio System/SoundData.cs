using UnityEngine;

namespace AustenKinney.AudioSystem
{
    /// <summary>
    /// Contains all the data needed for a sound effect or song.
    /// </summary>
    [CreateAssetMenu(fileName = "New Sound", menuName = "Create Sound", order = 50)]
    public class SoundData : ScriptableObject
    {
        [Tooltip("The audio clip which will be played in the game.")]
        [SerializeField] private AudioClip clip;

        [Tooltip("The adjusted volume of the audioclip.")]
        [Range(0, 1)]
        [SerializeField] private float gain;

        [Tooltip("The sound's category. This is used to seperate audio for audio mixing settings")]
        [SerializeField] private AudioCategory category = AudioCategory.SFX;

        #region Getters & Setters

        /// <summary>
        /// The audio clip which will be played in the game.
        /// </summary>
        public AudioClip Clip { get { return clip; } }

        /// <summary>
        /// The adjusted volume of the audioclip.
        /// </summary>
        public float Gain { get { return gain; } }

        /// <summary>
        /// The sound's category. This is used to seperate audio for audio mixing settings.
        /// </summary>
        public AudioCategory Category { get { return category; } }

        #endregion
    }
}
