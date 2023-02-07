using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AustenKinney.Dialogue
{
    [System.Serializable]
    public class DialogueGraphData : ScriptableObject
    {
        public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
        public List<DialogueNodeData> DialogueNodeData = new List<DialogueNodeData>();
        public List<ExposedProperty> ExposedProperties = new List<ExposedProperty>();
        public List<CommentBlockData> CommentBlockData = new List<CommentBlockData>();
    }
}
