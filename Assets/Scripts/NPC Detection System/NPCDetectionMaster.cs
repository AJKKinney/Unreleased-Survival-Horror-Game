using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls an NPC's detection state and detection systems.
/// </summary>
public class NPCDetectionMaster : MonoBehaviour
{
    [Header("Detection Settings")]
    [Tooltip("The speed at which the NPC's awareness will increase in the NPC's direct eyesight. A higher value will cause the NPC to detect the player faster.")]
    [SerializeField] private float directEyesightDetectionSpeed;
    [Tooltip("The speed at which awareness will increase in the NPC's peripheral eyesight. A higher value will cause the NPC to detect the player faster.")]
    [SerializeField] private float peripheralEyesightDetectionSpeed;
    [Tooltip("The speed at which awareness will increase in the NPC's over-the-shoulder eyesight. A higher value will cause the NPC to detect the player faster.")]
    [SerializeField] private float otsEyesightDetectionSpeed;
    [Tooltip("The speed at which the NPC's awareness will decrease. A higher value will cause the NPC's awareness to decrease more quickly.")]
    [SerializeField] private float detectionCooldownSpeed;
    [Tooltip("The amount of time the player must be undetected for the awarness of an NPC to begin to lower.")]
    [SerializeField] private float detectedCooldownDelay;
    [Tooltip("Layers which will be ignored by the NPC's detection system. Colliders on layers not included will block the NPC's line of sight.")]
    [SerializeField] private LayerMask ignoreLayers;


    /// <summary>
    /// A list of the objects which the detection master is tracking
    /// </summary>
    public List<DetectionData> trackedObjects = new List<DetectionData>();


    private void Update()
    {
        CheckVision();
    }

    private void CheckVision()
    {
        for (int i = 0; i < trackedObjects.Count; i ++)
        {
            int numberOfCollidersInLOS = 0;

            if (trackedObjects[i].detectionZone != DetectionZone.None)
            {
                for (int z = 0; z < trackedObjects[i].hitboxMaster.hitboxes.Length; z++)
                {
                    if (HasLineOfSight(trackedObjects[i].hitboxMaster.hitboxes[z]) == true)
                    {
                        numberOfCollidersInLOS += 1;
                    }
                }

                if (numberOfCollidersInLOS > 0)
                {
                    if (trackedObjects[i].awareness < trackedObjects[i].detectedThreshold)
                    {
                        if (trackedObjects[i].detectionZone == DetectionZone.DirectEyesight)
                        {
                            trackedObjects[i].awareness += directEyesightDetectionSpeed * numberOfCollidersInLOS * Time.deltaTime;
                        }
                        else if(trackedObjects[i].detectionZone == DetectionZone.PeripheralEyesight)
                        {
                            trackedObjects[i].awareness += peripheralEyesightDetectionSpeed * numberOfCollidersInLOS * Time.deltaTime;
                        }
                        else if(trackedObjects[i].detectionZone == DetectionZone.OTSEyesight)
                        {
                            trackedObjects[i].awareness += otsEyesightDetectionSpeed * numberOfCollidersInLOS * Time.deltaTime;
                        }
                    }

                    trackedObjects[i].delayTimer = detectedCooldownDelay;
                    trackedObjects[i].SetDetectionState();
                }
            }


            if (trackedObjects[i].delayTimer > 0)
            {
                trackedObjects[i].delayTimer -= Time.deltaTime;
            }
            else if(trackedObjects[i].awareness > 0)
            {
                trackedObjects[i].awareness -= detectionCooldownSpeed * Time.deltaTime;
            }
            else if (trackedObjects[i].awareness < 0 && trackedObjects[i].currentDetectionVolumes.Count == 0)
            {
                trackedObjects.RemoveAt(i);
            }
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


        if (Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, Mathf.Infinity, ~ignoreLayers) == true)
        {
            if (hitInfo.collider == collider)
            {
                Debug.DrawLine(transform.position, collider.transform.position + (collider.transform.localToWorldMatrix.rotation * collider.center), Color.cyan);
                return true;
            }
        }

        Debug.DrawLine(transform.position, collider.transform.position + (collider.transform.localToWorldMatrix.rotation * collider.center), Color.red);
        return false;
    }


    public enum DetectionState
    { 
        Undetected,
        Alert,
        Detected
    }

    public DetectionData GetDetectionData(GameObject key)
    {
        DetectionData detectionData;

        for (int i = 0; i < trackedObjects.Count; i++)
        {
            if(trackedObjects[i].trackedObject == key)
            {
                detectionData = trackedObjects[i];
                return detectionData;
            }
        }

        detectionData = new DetectionData(key);
        trackedObjects.Add(detectionData);
        return detectionData;
    }

    public void RecieveNoise(float increaseAwareness, GameObject objectToTrack)
    {
        DetectionData data = GetDetectionData(objectToTrack);

        data.delayTimer = detectedCooldownDelay;

        if(data.awareness >= data.alertThreshold)
        {
            return;
        }

        if(increaseAwareness > data.alertThreshold - data.awareness)
        {
            data.awareness = data.alertThreshold;
        }
        else
        {
            data.awareness += increaseAwareness;
        }

        data.SetDetectionState();

        Debug.Log(name + " heard a noise, and their awareness increased by " + increaseAwareness.ToString("F1") + "\n Awareness at " + data.awareness);
    }
}
