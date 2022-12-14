using UnityEngine;

namespace AustenKinney.DetectionSystem
{
    public class VisualDetectionVolume : MonoBehaviour
    {
        [SerializeField] private DetectionZone detectionVolumeType;

        private NPCDetectionMaster detectionMaster;

        #region Getters & Setters

        public DetectionZone DetectionVolumeType { get { return detectionVolumeType; } }

        #endregion

        private void Start()
        {
            detectionMaster = GetComponentInParent<NPCDetectionMaster>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<DetectableObject>(out DetectableObject detectable) == true)
            {
                DetectionData data = detectionMaster.GetDetectionData(detectable);

                data.CurrentDetectionVolumes.Add(this);
                data.SetCurrentDetectionZone();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<DetectableObject>(out DetectableObject detectable) == true)
            {
                DetectionData data = detectionMaster.GetDetectionData(detectable);

                data.CurrentDetectionVolumes.Remove(this);
                data.SetCurrentDetectionZone();
            }
        }
    }
}
