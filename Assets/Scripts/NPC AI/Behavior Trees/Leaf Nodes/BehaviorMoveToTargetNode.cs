using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "BehaviorTree/Leaf/Move To Target")]
public class BehaviorMoveToTargetNode : BehaviorTreeNode
{
    [SerializeField] private float stoppingDistance = 2f;

    public override NodeState Tick(AIBlackboard blackboard)
    {
        if (blackboard.Player == null || blackboard.Agent == null)
        {
            return NodeState.Failure;
        }

        blackboard.Agent.SetDestination(blackboard.Player.position);

        bool reachedDestination = Vector3.Distance(blackboard.Enemy.position, blackboard.Player.position) < stoppingDistance;
        
        if(reachedDestination == true)
        {
            return NodeState.Success;
        }
        else
        {
            return NodeState.Running;
        }
    }
}
