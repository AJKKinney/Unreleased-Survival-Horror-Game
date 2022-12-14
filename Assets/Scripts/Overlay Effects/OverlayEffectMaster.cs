using UnityEngine;
using AustenKinney.DetectionSystem;

namespace Lamplight.HUD
{
    public class OverlayEffectMaster : MonoBehaviour
    {
        [Header("Stealth Overlays")]

        [SerializeField] private FlashOverlayEffect alertedFlash;
        [SerializeField] private FlashOverlayEffect detectedFlash;
        [SerializeField] private DetectableObject player;

        private bool playerDetected;
        private int observers;

        private void Start()
        {
            player.OnAlert += Alerted;
            player.OnDetected += Detected;
            player.OnUndetected += Undetected;
        }

        private void Alerted()
        {
            if (playerDetected == false)
            {
                alertedFlash.Activate();
                observers += 1;
            }
        }

        private void Detected()
        {
            if (playerDetected == false)
            {
                detectedFlash.Activate();
                playerDetected = true;
            }
        }

        private void Undetected()
        {
            observers -= 1;

            if (playerDetected == true && observers == 0)
            {
                playerDetected = false;
            }
        }
    }
}
