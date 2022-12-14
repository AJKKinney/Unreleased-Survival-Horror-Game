using UnityEngine;

namespace AustenKinney.DetectionSystem
{
    public class DetectableObject : MonoBehaviour
    {
        [SerializeField] private BoxCollider[] detectableColliders;

        public BoxCollider[] DetectableColliders { get { return detectableColliders; } }

        public delegate void Alert();
        public event Alert OnAlert = delegate { };

        public delegate void Detected();
        public event Detected OnDetected = delegate { };

        public delegate void Undetected();
        public event Undetected OnUndetected = delegate { };

        public void CallOnAlert()
        {
            OnAlert();
            Debug.Log("alerted");
        }

        public void CallOnDetected()
        {
            OnDetected();
            Debug.Log("detected");
        }

        public void CallOnUndetected()
        {
            OnUndetected();
            Debug.Log("undetected");
        }
    }
}
