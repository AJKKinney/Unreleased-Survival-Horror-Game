using UnityEngine;

public abstract class BehaviorTreeNode : ScriptableObject
{
    public enum NodeState { Running, Success, Failure }
    protected NodeState state;

    public abstract NodeState Tick(AIController controller);
}
