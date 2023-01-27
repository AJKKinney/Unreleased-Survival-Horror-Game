using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


namespace AustenKinney.Dialogue
{
    public class DialogueNode : Node
    {
        public string DialogueText;
        public string GUID;
        public bool EntryPoint = false;
    }
}
