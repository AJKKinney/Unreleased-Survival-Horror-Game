using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AustenKinney.AudioSystem
{
    public class SongPlayer : MonoBehaviour
    {
        [SerializeField] private List<SongData> songs = new List<SongData>();

        private AudioManager audioManager;

        private void Start()
        {
            audioManager = AudioManager.Instance;
            PlaySong(0);
        }

        private bool transitioned = false;

        private void Update()
        {
            if(Time.timeSinceLevelLoad >= 5 && transitioned == false)
            {
                PlaySong(0);
                transitioned = true;
            }
        }

        private void PlaySong(int index)
        {
            StartCoroutine(audioManager.TransitionSong(songs[index], 5));
        }
    }
}
