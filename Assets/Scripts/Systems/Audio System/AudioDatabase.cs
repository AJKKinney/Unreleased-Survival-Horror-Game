using System.Collections.Generic;
using UnityEngine;
using System.IO;
using AustenKinney.Essentials;

namespace AustenKinney.AudioSystem
{
    /// <summary>
    /// Stores all the audio data into a sounds dictionary, so that it can be accessed at runtime.
    /// </summary>
    public class AudioDatabase
    {
        private Dictionary<string, SoundData[]> sounds = new Dictionary<string, SoundData[]>();

        private const string audioDirectory = "Audio";
        private const string rootPath = "Assets/Resources/";

        #region Getters & Setters

        /// <summary>
        /// The sounds stored in the audio database.
        /// </summary>
        public Dictionary<string, SoundData[]> Sounds { get { return sounds; } }

        #endregion

        #region Setup Database

        /// <summary>
        /// Loads the audio data from the correct folder, and organizes it within a Dictionary to be accessed later.
        /// </summary>
        public void LoadData()
        {
            string currentDirectory = "";

            DirectoryInfo[] directories = IOFunctionality.GetDirectories(rootPath + audioDirectory);

            foreach (var folder in directories)
            {
                currentDirectory = folder.Name;
                SoundData[] newSounds = Resources.LoadAll<SoundData>(audioDirectory + "/" + currentDirectory);
                sounds.Add(currentDirectory, newSounds);
            }
        }

        #endregion

        #region Search Methods

        /// <summary>
        /// Gets a sound from the audio database.
        /// </summary>
        /// <param name="directory">The group of sounds to be searched.</param>
        /// <param name="name">The name of the sound within the group</param>
        /// <returns></returns>
        public SoundData LookUpSound(string directory, string name)
        {
            foreach (SoundData sound in sounds[directory])
            {
                if (sound.name == name)
                {
                    return sound;
                }
            }

            Debug.LogWarning("Trying to find a sound that could not be found: \n" + rootPath + audioDirectory + "/" + directory + "/" + name + ".asset.");
            return null;
        }


        /// <summary>
        /// Gets a sound from the audio database.
        /// </summary>
        /// <param name="group">The group of sounds to be searched.</param>
        /// <param name="index">The index at which the sound is located within its group.</param>
        /// <returns>SoundData</returns>
        public SoundData LookUpSound(string group, int index)
        {
            if (sounds[group][index] != null)
            {
                return sounds[group][index];
            }

            Debug.LogWarning("Trying to find a sound that could not be found: \n" + rootPath + audioDirectory + "/" + group + "/*\n" + "At index: " + index);
            return null;
        }


        /// <summary>
        /// Gets a random sound from the audio database.
        /// </summary>
        /// <param name="group">The group of sounds to be selected from. Groups are created by adding folders to the Audio folder in Resources.</param>
        /// <returns>SoundData</returns>
        public SoundData LookUpRandomSound(string group)
        {
            int soundsAvailable = 0;

            if (sounds[group][0] != null)
            {
                soundsAvailable = sounds[group].Length;
                int index = Random.Range(0, soundsAvailable);
                return sounds[group][index];
            }

            Debug.LogWarning("Trying to find a random sound that could not be found: \n" + rootPath + audioDirectory + "/" + group + "/*.asset");
            return null;
        }

        #endregion
    }
}
