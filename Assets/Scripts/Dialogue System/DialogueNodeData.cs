using UnityEngine;

namespace AustenKinney.Dialogue
{
    [System.Serializable]
    public class DialogueNodeData
    {
        public string NodeGUID;
        public string NodeTitle;
        public string DialogueText;
        public Vector2 Position;
        public NodeType Type;
    }
}
