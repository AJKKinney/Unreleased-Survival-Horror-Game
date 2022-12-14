using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AustenKinney.AI.DetectionSystem
{
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

                data.CurrentDetectionVolumes.Add(this);
                data.SetCurrentDetectionZone();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Push Box"))
            {
                DetectionData data = detectionMaster.GetDetectionData(other.gameObject);

                data.CurrentDetectionVolumes.Remove(this);
                data.SetCurrentDetectionZone();
            }
        }
    }
}
