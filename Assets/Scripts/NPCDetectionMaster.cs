using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDetectionMaster : MonoBehaviour
{
    [Tooltip("The current detection state of the NPC.")]
    public DetectionState currentDetectionState = DetectionState.Undetected;

    [Header("Detection Volumes")]
    [Tooltip("A trigger collider which represents the area that is considered in the NPC's direct eyesight.")]
    [SerializeField] private VisualDetectionVolume directEyesightTrigger;
    [Tooltip("A trigger collider which represents the area that is considered in the NPC's peripheral eyesight.")]
    [SerializeField] private VisualDetectionVolume peripheralEyesightTrigger;
    [Tooltip("A trigger collider which represents the area that is considered in the NPC's right side, over-the-shoulder eyesight.")]
    [SerializeField] private VisualDetectionVolume rightOTSEyesightTrigger;
    [Tooltip("A trigger collider which represents the area that is considered in the NPC's left side, over-the-shoulder eyesight.")]
    [SerializeField] private VisualDetectionVolume leftOTSEyesightTrigger;


    [Header("Detection Settings")]
    [Tooltip("The speed at which the NPC's awareness will increase in the NPC's direct eyesight. A higher value will cause the NPC to detect the player faster.")]
    [SerializeField] private float directEyesightDetectionSpeed;
    [Tooltip("The speed at which awareness will increase in the NPC's peripheral eyesight. A higher value will cause the NPC to detect the player faster.")]
    [SerializeField] private float peripheralEyesightDetectionSpeed;
    [Tooltip("The speed at which awareness will increase in the NPC's over-the-shoulder eyesight. A higher value will cause the NPC to detect the player faster.")]
    [SerializeField] private float otsEyesightDetectionSpeed;
    [Tooltip("The Threshold at which the NPC's will begin to search suspicious areas.")]
    [SerializeField] private float searchThreshold;
    [Tooltip("The Threshold at which the NPC's will be fully aware of the player's location.")]
    [SerializeField] private float detectedThreshold;
    [Tooltip("The speed at which the NPC's awareness will decrease. A higher value will cause the NPC's awareness to decrease more quickly.")]
    [SerializeField] private float detectionCooldownSpeed;
    [Tooltip("The amount of time the player must be undetected for the awarness of an NPC to begin to lower.")]
    [SerializeField] private float detectedCooldownDelay;
    [Tooltip("Layers which will be ignored by the NPC's detection system. Colliders on layers not included will block the NPC's line of sight.")]
    [SerializeField] private LayerMask ignoreLayers;

    
    /// <summary>
    /// A measure of how aware the NPC is of the player's presence.
    /// </summary>
    private float awareness;
    /// <summary>
    /// A timer which shows how much time is remaining until the awarness of an NPC will begin to lower.
    /// </summary>
    private float delayTimer;


    private void Update()
    {
        CheckForPlayer();
        SetDetectionState();
    }


    /// <summary>
    /// Checks for the player's presence in the NPC's detection volumes, and increases the awareness of the NPC accordingly.
    /// </summary>
    private void CheckForPlayer()
    {
        int numberOfCollidersInLOS = 0;

        if (directEyesightTrigger.playerStealthMaster != null)
        {
            for (int i = 0; i < directEyesightTrigger.playerStealthMaster.hitboxes.Length; i++)
            {
                if(HasLineOfSight(directEyesightTrigger.playerStealthMaster.hitboxes[i]) == true)
                {
                    numberOfCollidersInLOS += 1;
                }
            }

            awareness += directEyesightDetectionSpeed * numberOfCollidersInLOS * Time.deltaTime;
            delayTimer = detectedCooldownDelay;
        }
        else if (peripheralEyesightTrigger.playerStealthMaster != null)
        {
            for (int i = 0; i < peripheralEyesightTrigger.playerStealthMaster.hitboxes.Length; i++)
            {
                if (HasLineOfSight(peripheralEyesightTrigger.playerStealthMaster.hitboxes[i]) == true)
                {
                    numberOfCollidersInLOS += 1;
                }
            }

            awareness += peripheralEyesightDetectionSpeed * numberOfCollidersInLOS * Time.deltaTime;
            delayTimer = detectedCooldownDelay;
        }
        else if (rightOTSEyesightTrigger.playerStealthMaster != null)
        {
            for (int i = 0; i < rightOTSEyesightTrigger.playerStealthMaster.hitboxes.Length; i++)
            {
                if (HasLineOfSight(rightOTSEyesightTrigger.playerStealthMaster.hitboxes[i]) == true)
                {
                    numberOfCollidersInLOS += 1;
                }
            }

            awareness += otsEyesightDetectionSpeed * numberOfCollidersInLOS * Time.deltaTime;
            delayTimer = detectedCooldownDelay;
        }
        else if (leftOTSEyesightTrigger.playerStealthMaster != null)
        {
            for (int i = 0; i < leftOTSEyesightTrigger.playerStealthMaster.hitboxes.Length; i++)
            {
                if (HasLineOfSight(leftOTSEyesightTrigger.playerStealthMaster.hitboxes[i]) == true)
                {
                    numberOfCollidersInLOS += 1;
                }
            }

            awareness += otsEyesightDetectionSpeed * numberOfCollidersInLOS * Time.deltaTime;
            delayTimer = detectedCooldownDelay;
        }

        if (numberOfCollidersInLOS == 0)
        {
            if (awareness > detectedThreshold)
            {
                if (delayTimer > 0)
                {
                    delayTimer -= Time.deltaTime;
                }
                else
                {
                    awareness = detectedThreshold;
                }
            }
            else if (awareness > 0)
            {
                awareness -= detectionCooldownSpeed * Time.deltaTime;
            }
        }
        else
        {
            Debug.Log(numberOfCollidersInLOS);
        }
    }

    /// <summary>
    /// Does a raycast to check if the collider is in the NPCDetectionMaster's line of sight, and returns true if it is.
    /// </summary>
    /// <param name="collider">The collider to be checked for line of sight.</param>
    /// <returns>Bool</returns>
    private bool HasLineOfSight(BoxCollider collider)
    { 
        Vector3 direction = collider.transform.position + (collider.transform.localToWorldMatrix.rotation * collider.center) - transform.position;
        Debug.DrawLine(transform.position, collider.transform.position + (collider.transform.localToWorldMatrix.rotation * collider.center), Color.cyan);

        if (Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, Mathf.Infinity, ~ignoreLayers) == true)
        {

            Debug.Log("Hit " + hitInfo.collider.name);

            if (hitInfo.collider == collider)
            {
                return true;
            }

        }

        return false;
    }


    /// <summary>
    /// Sets the players current detection state based on their current awareness.
    /// </summary>
    private void SetDetectionState()
    {
        if (awareness > detectedThreshold)
        {
            currentDetectionState = DetectionState.Detected;
        }
        else if (awareness > searchThreshold)
        {
            currentDetectionState = DetectionState.Alert;
        }
        else if (awareness <= 0)
        {
            currentDetectionState = DetectionState.Undetected;
        }
    }


    public enum DetectionState
    { 
        Undetected,
        Alert,
        Detected
    }
}
