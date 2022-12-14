using UnityEngine;
using AustenKinney.AudioSystem;
using AustenKinney.DetectionSystem;


public class Footstepper : MonoBehaviour
{
    [SerializeField] private DetectableObject detectableObject;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;
        detectableObject = transform.root.GetComponent<DetectableObject>();
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

        NoiseMaker.CreateNoise(transform.position, 5, 15, detectableObject);
    }
}
