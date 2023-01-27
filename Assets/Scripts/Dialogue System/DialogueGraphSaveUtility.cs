using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AustenKinney.Dialogue
{
    public class DialogueGraphSaveUtility
    {
        private DialogueGraphEditorView editorView;

        public static DialogueGraphSaveUtility GetInstance(DialogueGraphEditorView graphView)
        {
            return new DialogueGraphSaveUtility
            {
                editorView = graphView
            };
        }

        public void SaveGraph(string fileName)
        {

        }

        public void LoadGraph(string fileName)
        {

        }
    }
}
