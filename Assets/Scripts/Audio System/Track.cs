using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AustenKinney.AudioSystem
{
    [System.Serializable]
    public class Track
    {
        [SerializeField] private AudioClip[] clips;
        private bool muted = true;

        #region Getters & Setters

        public AudioClip[] Clips { get { return clips; } }
        public bool Muted { get { return muted; } set { muted = value; } }

        #endregion

        public AudioClip SetCurrentClip(int index)
        {
            if(clips.Length == 0)
            {
                Debug.LogWarning(this.ToString() + " is missing audioclips.");
                return null;
            }

            if (index > clips.Length - 1)
            {
                index = 0;
            }
            else if(index < 0)
            {
                index = clips.Length - 1;
            }

            return clips[index];
        }
    }
}
