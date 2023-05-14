using UnityEngine;
using AustenKinney.AudioSystem;
using AustenKinney.DetectionSystem;
using Lamplight.Input;


public class PlayerFootstepper : MonoBehaviour
{
    private DetectableObject detectableObject;
    private AudioManager audioManager;
    private InventoryManager inventoryManager;
    private PlayerController playerController;

    #region Initialization

    private void Start()
    {
        audioManager = AudioManager.instance;
        inventoryManager = InventoryManager.instance;
        detectableObject = transform.root.GetComponent<DetectableObject>();
        playerController = transform.root.GetComponent<PlayerController>();
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            Step();
        }
    }

    /// <summary>
    /// Creates a footstep noise for the detection system and plays the appropriate SFX.
    /// </summary>
    private void Step()
    {
        SoundData sfx = audioManager.Database.LookUpRandomSound("Footsteps");
        audioManager.PlaySound(sfx, transform);

        float travelDistance = 10 * CalculateStrength();
        float awarenessIncrement = 30 * CalculateStrength();

        NoiseMaker.CreateNoise(transform.position, travelDistance, awarenessIncrement, detectableObject);
    }

    /// <summary>
    /// Calculates the noise strength modifier for footsteps. The more weight that the player carries the louder the footsteps will be and the further they will travel.
    /// </summary>
    /// <returns>float</returns>
    private float CalculateStrength()
    {
        float maxBaseStrength = 0.8f;
        float minBaseStrength = 0.4f;

        float noiseStrenghth = minBaseStrength;
        if (inventoryManager.CarriedWeight > 0)
        {
            float encumberance = 1 - (inventoryManager.CarriedWeight / inventoryManager.MaxCarryWeight);
            noiseStrenghth = Mathf.Lerp(maxBaseStrength, minBaseStrength, encumberance);
        }

        if (InputProvider.playerActions.GameActions.Sprint.IsPressed())
        {
            noiseStrenghth += 0.2f;
        }
        else if (playerController.IsCrouched == true)
        {
            noiseStrenghth -= 0.3f;
        }

        return noiseStrenghth;
    }
}
