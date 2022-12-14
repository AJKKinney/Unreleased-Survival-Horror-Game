using UnityEngine;
using AustenKinney.DetectionSystem;
using UnityEngine.UI;


namespace Lamplight.HUD
{
    public class DetectionMeter : MonoBehaviour
    {
        [SerializeField] private Slider alertMeter;
        [SerializeField] private Slider detectionMeter;
        [SerializeField] private NPCDetectionMaster detectionMaster;


        private Camera cam;
        private const float sliderRate = 3f;

        private void Start()
        {
            cam = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            if (detectionMaster.PlayerData == null)
            {
                DisableMeters();
            }
            else
            {
                SetPlayerDetection();
                FaceCamera();
            }

        }

        private void DisableMeters()
        {
            if (alertMeter.gameObject.activeSelf == true)
            {
                alertMeter.gameObject.SetActive(false);
            }
            if (detectionMeter.gameObject.activeSelf == true)
            {
                detectionMeter.gameObject.SetActive(false);
            }
        }

        private void FaceCamera()
        {
            transform.rotation = Quaternion.Euler(cam.transform.rotation.eulerAngles);
        }

        private void SetPlayerDetection()
        {
            float alertValue = 0;
            float detectionValue = 0;



            if (detectionMaster.PlayerData.CurrentState != DetectionState.Detected)
            {
                alertValue = detectionMaster.PlayerData.Awareness / detectionMaster.PlayerData.AlertThreshold;
                detectionValue = (detectionMaster.PlayerData.Awareness - detectionMaster.PlayerData.AlertThreshold) / (detectionMaster.PlayerData.DetectedThreshold - detectionMaster.PlayerData.AlertThreshold);
            }
            else
            {
                detectionValue = detectionMaster.PlayerData.Awareness / detectionMaster.PlayerData.AlertThreshold;
            }

            if (alertValue > 0)
            {
                if (alertMeter.gameObject.activeSelf == false)
                {
                    alertMeter.value = 0;
                    alertMeter.gameObject.SetActive(true);
                }

                alertMeter.value = Mathf.Lerp(alertMeter.value, alertValue, sliderRate * Time.deltaTime);
            }
            else if (alertMeter.gameObject.activeSelf == true)
            {
                alertMeter.gameObject.SetActive(false);
            }


            if (detectionValue > 0)
            {
                if (detectionMeter.gameObject.activeSelf == false)
                {
                    detectionMeter.value = 0;
                    detectionMeter.gameObject.SetActive(true);
                }

                detectionMeter.value = Mathf.Lerp(detectionMeter.value, detectionValue, sliderRate * Time.deltaTime);
            }
            else if (detectionMeter.gameObject.activeSelf == true)
            {
                detectionMeter.gameObject.SetActive(false);
            }
        }
    }
}
