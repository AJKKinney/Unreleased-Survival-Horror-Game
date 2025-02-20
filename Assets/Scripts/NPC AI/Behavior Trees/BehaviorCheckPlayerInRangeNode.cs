using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Leaf/Check Player In Range")]
public class BehaviorCheckPlayerInRangeNode : BehaviorTreeNode
{
    [SerializeField] private float detectionRange = 3f;

    public override NodeState Tick(AIController controller)
    {
        if (controller.Blackboard.Player == null)
        {
            return NodeState.Failure;
        }

        float distance = Vector3.Distance(controller.AITransform.position, controller.Blackboard.Player.position);
        bool playerInRange = distance <= detectionRange;

        if (playerInRange)
        {
            return NodeState.Success;
        }
        else
        {
            return NodeState.Failure;
        }
    }
}
