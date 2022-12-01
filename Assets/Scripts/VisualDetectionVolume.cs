using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDetectionVolume : MonoBehaviour
{
    [HideInInspector] public HitBoxMaster playerStealthMaster;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") == true)
        {
            playerStealthMaster = other.GetComponent<HitBoxMaster>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            playerStealthMaster = null;
        }
    }
}
