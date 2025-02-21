using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AustenKinney.AudioSystem
{
    public class MusicZoneMaster : MonoBehaviour
    {
        [SerializeField] private SongData song;

        private bool isPlaying;
        private List<MusicZone> currentVolumes = new List<MusicZone>();
        private AudioManager manager;

        #region Getters & Setters

        public SongData Song { get { return song; } }
        public List<MusicZone> CurrentVolumes { get { return currentVolumes; } set { currentVolumes = value; } }

        #endregion

        private void Start()
        {
            manager = AudioManager.Instance;
        }

        private void Update()
        {
            if(isPlaying == false && currentVolumes.Count > 0)
            {
                manager.SetUpSong(song);
                isPlaying = true;
            }
            else if(isPlaying == true && currentVolumes.Count == 0)
            {
                isPlaying = false;
            }
        }

        public void AdaptTrack(int trackIndex, int clipIndex, float transitionLength)
        {
            StartCoroutine(manager.TransitionTrack(song, trackIndex, clipIndex, transitionLength));
        }

        public void FadeOutTrack(int trackIndex, float transitionLength)
        {
            StartCoroutine(manager.FadeOutTrack(song, trackIndex, transitionLength));
        }
    }
}
