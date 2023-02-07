using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustenKinney.Essentials;
using AustenKinney.Dialogue;

namespace Lamplight.Dialogue
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        private DialogueGraphData currentDialogue;
        private DialogueNodeData currentNode;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void StartDialogue(DialogueGraphData dialogue)
        {
            currentDialogue = dialogue;
            currentNode = dialogue.DialogueNodeData[0];
        }

        public void LoadNextNode(int outputPort = 0)
        {
          
        }
    }
}
