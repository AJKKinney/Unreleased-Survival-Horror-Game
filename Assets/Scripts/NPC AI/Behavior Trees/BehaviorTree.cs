using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Tree")]
public class BehaviorTree : ScriptableObject
{
    [SerializeField] private BehaviorTreeNode rootNode;

    public BehaviorTreeNode.NodeState Tick(AIController controller)
    {
        return rootNode.Tick(controller);
    }
}
