using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDetectionVolume : MonoBehaviour
{
    public DetectionZone detectionVolumeType;

    private NPCDetectionMaster detectionMaster;

    private void Start()
    {
        detectionMaster = GetComponentInParent<NPCDetectionMaster>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Push Box"))
        {
            DetectionData data = detectionMaster.GetDetectionData(other.gameObject);

            data.currentDetectionVolumes.Add(this);
            data.SetCurrentDetectionZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Push Box"))
        {
            DetectionData data = detectionMaster.GetDetectionData(other.gameObject);

            data.currentDetectionVolumes.Remove(this);
            data.SetCurrentDetectionZone();
        }
    }
}

public enum DetectionZone
{
    None,
    DirectEyesight,
    PeripheralEyesight,
    OTSEyesight
}
