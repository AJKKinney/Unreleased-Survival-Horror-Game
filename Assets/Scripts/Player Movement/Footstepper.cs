using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustenKinney.AudioSystem;

public class Footstepper : MonoBehaviour
{
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            Step();
        }
    }

    private void Step()
    {
        SoundData sfx = audioManager.Database.LookUpRandomSound("Footsteps");
        audioManager.PlaySound(sfx, transform);

        NoiseMaker.CreateNoise(transform.position, 5, 15, transform.root.gameObject);
    }
}
