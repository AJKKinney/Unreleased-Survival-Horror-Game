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
            audioManager = AudioManager.instance;
            PlaySong(0);
        }

        private void PlaySong(int index)
        {
            audioManager.PlayLoop(songs[index]);
        }
    }
}
