using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AustenKinney.AudioSystem
{
    public class MusicZoneMaster : MonoBehaviour
    {
        [SerializeField] private SongData song;
        [SerializeField] private List<MusicZone> currentVolumes;
        private bool isPlaying;

        private AudioManager manager;

        #region Getters & Setters

        public SongData Song { get { return song; } }
        public List<MusicZone> CurrentVolumes { get { return currentVolumes; } set { currentVolumes = value; } }

        #endregion

        private void Start()
        {
            manager = AudioManager.instance;
        }

        private void Update()
        {
            if(isPlaying == false && currentVolumes.Count > 0)
            {
                //StartCoroutine(manager.TransitionSong(song, 5));
                isPlaying = true;
            }
            else if(isPlaying == true && currentVolumes.Count == 0)
            {
                //StartCoroutine(manager.FadeOutSong(song, 2.5f));
                isPlaying = false;
            }
        }

        public void AdaptTrack(int trackIndex, int clipIndex)
        {
            StartCoroutine(manager.TransitionTrack(song, trackIndex, clipIndex, 1));
        }

        public void FadeOutTrack(int trackIndex)
        {
            StartCoroutine(manager.FadeOutTrack(song, trackIndex, 1f));
        }
    }
}
