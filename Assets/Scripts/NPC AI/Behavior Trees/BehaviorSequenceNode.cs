using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Composite/Sequence")]
public class BehaviorSequenceNode : BehaviorTreeNode
{
    [SerializeField] private List<BehaviorTreeNode> children = new List<BehaviorTreeNode>();

    public BehaviorSequenceNode(List<BehaviorTreeNode> nodes)
    {
        children = nodes;
    }


    public override NodeState Tick(AIBlackboard blackboard)
    {
        foreach (var child in children)
        {
            NodeState result = child.Tick(blackboard);
            if (result == NodeState.Failure)
            {
                return NodeState.Failure;
            }
            if (result == NodeState.Running)
            {
                return NodeState.Running;
            }
        }
        return NodeState.Success;
    }
}
