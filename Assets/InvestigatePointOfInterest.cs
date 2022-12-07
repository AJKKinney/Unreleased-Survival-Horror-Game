using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigatePointOfInterest : MonoBehaviour
{
    private NPCNavigator navigator;

    private NPCDetectionMaster detectionMaster;

    private Vector3 pointOfInterest;

    // Update is called once per frame
    void Update()
    {
        navigator.SetTarget(pointOfInterest);
    }


}
