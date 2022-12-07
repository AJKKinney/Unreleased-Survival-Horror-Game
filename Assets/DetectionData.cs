using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DetectionData
{
    public GameObject trackedObject;
    public HitBoxMaster hitboxMaster;
    public Vector3 position;
    public float awareness;
    public DetectionState detectionState;
    public float delayTimer;
    public DetectionZone detectionZone;
    public List<VisualDetectionVolume> currentDetectionVolumes = new List<VisualDetectionVolume>();


    public float detectedThreshold = 100;
    public float alertThreshold = 50;

    public DetectionData(GameObject detectableActor)
    {
        trackedObject = detectableActor;
        hitboxMaster = trackedObject.GetComponent<HitBoxMaster>();
        position = trackedObject.transform.position;
        awareness = 0;
        detectionState = DetectionState.Undetected;
    }

    /// <summary>
    /// Sets the objects current detection state based on the detector's current awareness.
    /// </summary>
    public void SetDetectionState()
    {
        if (awareness >= detectedThreshold)
        {
            detectionState = DetectionState.Detected;
        }
        else if (awareness >= alertThreshold)
        {
            detectionState = DetectionState.Alert;
        }
        else if (awareness <= 0)
        {
            detectionState = DetectionState.Undetected;
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
            detectionZone = DetectionZone.None;
        }
        else
        {
            for (int i = 0; i < currentDetectionVolumes.Count; i++)
            {
                if (currentDetectionVolumes[i].detectionVolumeType == DetectionZone.DirectEyesight)
                {
                    detectionZone = DetectionZone.DirectEyesight;
                }
                else if (detectionZone != DetectionZone.DirectEyesight && currentDetectionVolumes[i].detectionVolumeType == DetectionZone.PeripheralEyesight)
                {
                    detectionZone = DetectionZone.PeripheralEyesight;
                }
                else if(detectionZone != DetectionZone.DirectEyesight && detectionZone != DetectionZone.PeripheralEyesight && currentDetectionVolumes[i].detectionVolumeType == DetectionZone.OTSEyesight)
                {
                    detectionZone = DetectionZone.OTSEyesight;
                }    

            }
        }
    }
}

public enum DetectionState
{
    Undetected,
    Alert,
    Detected
}
