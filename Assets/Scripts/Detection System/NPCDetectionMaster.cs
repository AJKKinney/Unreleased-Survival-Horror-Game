using System.Collections.Generic;
using UnityEngine;

namespace AustenKinney.DetectionSystem
{
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
        [Tooltip("The max distance from which an npc may detect objects.")]
        [SerializeField] private float maxDetectionDistance;
        [Tooltip("Layers which will be ignored by the NPC's detection system. Colliders on layers not included will block the NPC's line of sight.")]
        [SerializeField] private LayerMask ignoreLayers;


        /// <summary>
        /// A list of the objects which the detection master is tracking
        /// </summary>
        private List<DetectionData> trackedObjects = new List<DetectionData>();
        private DetectionData playerData = null;

        #region Getters & Setters

        public DetectionData PlayerData { get { return playerData; } }

        #endregion


        private void Update()
        {
            CheckVision();
        }

        private void CheckVision()
        {
            for (int i = 0; i < trackedObjects.Count; i++)
            {
                float distance = Vector3.Distance(trackedObjects[i].TrackedObject.transform.position, transform.position);

                if (distance < maxDetectionDistance)
                {
                    float distanceModifier = 1 - (distance / maxDetectionDistance);

                    int numberOfCollidersInLOS = 0;

                    if (trackedObjects[i].CurrentZone != DetectionZone.None)
                    {
                        for (int z = 0; z < trackedObjects[i].Hitboxes.Length; z++)
                        {
                            if (HasLineOfSight(trackedObjects[i].Hitboxes[z]) == true)
                            {
                                numberOfCollidersInLOS += 1;
                            }
                        }

                        if (numberOfCollidersInLOS > 0)
                        {
                            if (trackedObjects[i].Awareness < trackedObjects[i].DetectedThreshold)
                            {
                                if (trackedObjects[i].CurrentZone == DetectionZone.DirectEyesight)
                                {
                                    trackedObjects[i].IncrementAwareness(directEyesightDetectionSpeed * numberOfCollidersInLOS * distanceModifier * Time.deltaTime);
                                }
                                else if (trackedObjects[i].CurrentZone == DetectionZone.PeripheralEyesight)
                                {
                                    trackedObjects[i].IncrementAwareness(peripheralEyesightDetectionSpeed * numberOfCollidersInLOS * distanceModifier * Time.deltaTime);
                                }
                                else if (trackedObjects[i].CurrentZone == DetectionZone.OTSEyesight)
                                {
                                    trackedObjects[i].IncrementAwareness(otsEyesightDetectionSpeed * numberOfCollidersInLOS * distanceModifier * Time.deltaTime);
                                }
                            }

                            trackedObjects[i].DelayTimer = detectedCooldownDelay;
                            trackedObjects[i].SetDetectionState();
                        }
                    }


                    if (trackedObjects[i].DelayTimer > 0)
                    {
                        trackedObjects[i].DelayTimer -= Time.deltaTime;
                    }
                    else if (trackedObjects[i].Awareness > 0)
                    {
                        trackedObjects[i].IncrementAwareness(-detectionCooldownSpeed * Time.deltaTime);
                    }
                    else if (trackedObjects[i].Awareness < 0 && trackedObjects[i].CurrentDetectionVolumes.Count == 0)
                    {
                        if (trackedObjects[i] == playerData)
                        {
                            playerData = null;
                        }

                        trackedObjects.RemoveAt(i);
                    }
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


        public DetectionData GetDetectionData(DetectableObject key)
        {
            DetectionData detectionData;

            for (int i = 0; i < trackedObjects.Count; i++)
            {
                if (trackedObjects[i].TrackedObject == key)
                {
                    detectionData = trackedObjects[i];
                    return detectionData;
                }
            }

            detectionData = new DetectionData(key);
            trackedObjects.Add(detectionData);
            
            if(key.gameObject.CompareTag("Player"))
            {
                playerData = detectionData;
            }

            return detectionData;
        }

        public void RecieveNoise(float increaseAwareness, DetectableObject objectToTrack)
        {
            DetectionData data = GetDetectionData(objectToTrack);

            data.DelayTimer = detectedCooldownDelay;

            if (data.Awareness >= data.AlertThreshold || data.CurrentState == DetectionState.Detected)
            {
                return;
            }

            if(increaseAwareness + data.Awareness > data.AlertThreshold)
            {
                increaseAwareness = data.AlertThreshold - data.Awareness;
            }

            data.IncrementAwareness(increaseAwareness);

            data.SetDetectionState();

            Debug.Log(transform.parent.name + " heard a noise, and their awareness increased by " + increaseAwareness.ToString("F1") + "\n Awareness at " + data.Awareness);
        }
    }
}
