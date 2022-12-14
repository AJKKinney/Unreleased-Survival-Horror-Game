using UnityEngine;
using UnityEngine.AI;

namespace AustenKinney.DetectionSystem
{
    public class NoiseMaker
    {
        public static void CreateNoise(Vector3 worldPosition, float travelDistance, float awareness, DetectableObject trackableObject)
        {
            Collider[] colliders = Physics.OverlapSphere(worldPosition, travelDistance);


            foreach (Collider collider in colliders)
            {
                NPCDetectionMaster detectionMaster = collider.GetComponentInChildren<NPCDetectionMaster>();

                if (detectionMaster != null)
                {
                    NavMeshPath path = new NavMeshPath();
                    NavMesh.CalculatePath(worldPosition, collider.transform.position, NavMesh.AllAreas, path);

                    float distance = 0;

                    for (int i = 0; i < path.corners.Length - 1; i++)
                    {
                        distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
                    }

                    if (distance < travelDistance)
                    {
                        detectionMaster.RecieveNoise(awareness * (1 - (distance / travelDistance)), trackableObject);
                    }
                }
            }
        }

        public static void CreateNoise(Vector3 worldPosition, float travelDistance, DetectableObject trackableObject)
        {
            Collider[] colliders = Physics.OverlapSphere(worldPosition, travelDistance);


            foreach (Collider collider in colliders)
            {
                NPCDetectionMaster detectionMaster = collider.GetComponentInChildren<NPCDetectionMaster>();

                if (detectionMaster != null)
                {
                    NavMeshPath path = new NavMeshPath();
                    NavMesh.CalculatePath(worldPosition, collider.transform.position, NavMesh.AllAreas, path);

                    float distance = 0;

                    for (int i = 0; i < path.corners.Length - 1; i++)
                    {
                        distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
                    }

                    if (distance < travelDistance)
                    {
                        detectionMaster.RecieveNoise(9999, trackableObject);
                    }
                }
            }
        }
    }
}
