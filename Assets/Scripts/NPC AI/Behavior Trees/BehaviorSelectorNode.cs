using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Composite/Selector")]
public class BehaviorSelectorNode : BehaviorTreeNode
{
    [SerializeField] private List<BehaviorTreeNode> children = new List<BehaviorTreeNode>();

    public BehaviorSelectorNode(List<BehaviorTreeNode> nodes)
    {
        children = nodes;
    }

    public override NodeState Tick(AIBlackboard blackboard)
    {
        foreach (var child in children)
        {
            NodeState result = child.Tick(blackboard);
            if (result == NodeState.Success)
            {
                return NodeState.Success;
            }
            if (result == NodeState.Running)
            {
                return NodeState.Running;
            }
        }
        return NodeState.Failure;
    }
}
