using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "BehaviorTree/Leaf/Move To Target")]
public class BehaviorMoveToTargetNode : BehaviorTreeNode
{
    [SerializeField] private float stoppingDistance = 2f;

    public override NodeState Tick(AIController controller)
    {
        if (controller.Blackboard.Player == null || controller.AINavmeshAgent == null)
        {
            return NodeState.Failure;
        }

        controller.AINavmeshAgent.SetDestination(controller.Blackboard.Player.position);

        bool reachedDestination = Vector3.Distance(controller.AITransform.position, controller.Blackboard.Player.position) < stoppingDistance;
        
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
