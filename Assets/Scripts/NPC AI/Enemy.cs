using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private BehaviorTree behaviorTree;
    private AIBlackboard blackboard;

    void Start()
    {
        blackboard = new AIBlackboard
        {
            Enemy = transform,
            Player = GameObject.FindGameObjectWithTag("Player").transform,
            Agent = GetComponent<NavMeshAgent>()
        };
    }

    void Update()
    {
        behaviorTree.Tick(blackboard); // Process behavior tree each frame.
    }
}