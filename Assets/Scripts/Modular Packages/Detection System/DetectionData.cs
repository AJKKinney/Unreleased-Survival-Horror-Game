using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AustenKinney.AI.DetectionSystem
{
    [System.Serializable]
    public class DetectionData
    {
        private GameObject trackedObject;
        private float awareness;
        private float delayTimer;
        private BoxCollider[] hitboxes;
        private DetectionState currentState;
        private DetectionZone currentZone;
        private List<VisualDetectionVolume> currentDetectionVolumes = new List<VisualDetectionVolume>();

        private const float detectedThreshold = 100;
        private const float alertThreshold = 50;

        #region Getters & Setters

        public GameObject TrackedObject { get { return trackedObject; } }
        public BoxCollider[] Hitboxes { get { return hitboxes; } }
        public float Awareness { get { return awareness; } set { awareness = value; } }
        public float DelayTimer { get { return delayTimer; } set { delayTimer = value; } }
        public DetectionState CurrentState { get { return currentState; } }
        public DetectionZone CurrentZone { get { return currentZone; } }
        public List<VisualDetectionVolume> CurrentDetectionVolumes { get { return currentDetectionVolumes; } }

        public float DetectedThreshold { get { return detectedThreshold; } }
        public float AlertThreshold { get { return alertThreshold; } }

        #endregion

        public DetectionData(GameObject detectableActor)
        {
            trackedObject = detectableActor;
            hitboxes = trackedObject.GetComponentsInChildren<BoxCollider>();
            awareness = 0;
            currentState = DetectionState.Undetected;
        }

        /// <summary>
        /// Sets the objects current detection state based on the detector's current awareness.
        /// </summary>
        public void SetDetectionState()
        {
            if (awareness >= detectedThreshold)
            {
                currentState = DetectionState.Detected;
            }
            else if (awareness >= alertThreshold)
            {
                currentState = DetectionState.Alert;
            }
            else if (awareness <= 0)
            {
                currentState = DetectionState.Undetected;
            }
        }

        public void AdjustAwareness(float awarenessChange)
        {
            awareness += awarenessChange;
            SetDetectionState();
        }

        public void SetCurrentDetectionZone()
        {
            if (currentDetectionVolumes.Count == 0)
            {
                currentZone = DetectionZone.None;
            }
            else
            {
                for (int i = 0; i < currentDetectionVolumes.Count; i++)
                {
                    if (currentDetectionVolumes[i].detectionVolumeType == DetectionZone.DirectEyesight)
                    {
                        currentZone = DetectionZone.DirectEyesight;
                    }
                    else if (currentZone != DetectionZone.DirectEyesight && currentDetectionVolumes[i].detectionVolumeType == DetectionZone.PeripheralEyesight)
                    {
                        currentZone = DetectionZone.PeripheralEyesight;
                    }
                    else if (currentZone != DetectionZone.DirectEyesight && currentZone != DetectionZone.PeripheralEyesight && currentDetectionVolumes[i].detectionVolumeType == DetectionZone.OTSEyesight)
                    {
                        currentZone = DetectionZone.OTSEyesight;
                    }

                }
            }
        }
    }
}
