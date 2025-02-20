using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [SerializeField] private BehaviorTree behaviorTree;
    [SerializeField] private AIBlackboard blackboard;
    private Transform aiTransform;
    private NavMeshAgent aiNavmeshAgent;

    #region Getters & Setters

    public BehaviorTree BehaviorTree { get => behaviorTree; set => behaviorTree = value; }
    public AIBlackboard Blackboard { get => blackboard; set => blackboard = value; }
    public Transform AITransform { get => aiTransform; set => aiTransform = value; }
    public NavMeshAgent AINavmeshAgent { get => aiNavmeshAgent; set => aiNavmeshAgent = value; }

    #endregion

    private void Start()
    {
        AITransform = transform;
        aiNavmeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        BehaviorTree.Tick(this); // Process behavior tree each frame.
    }
}