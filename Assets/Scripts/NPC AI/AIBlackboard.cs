using UnityEngine;
using UnityEngine.AI;

public class AIBlackboard : MonoBehaviour
{
    private Transform enemy;
    private Transform player;
    private NavMeshAgent agent;

    #region Getters & Setters

    public Transform Enemy { get => enemy; set => enemy = value; }
    public Transform Player { get => player; set => player = value; }
    public NavMeshAgent Agent { get => agent; set => agent = value; }

    #endregion
}
