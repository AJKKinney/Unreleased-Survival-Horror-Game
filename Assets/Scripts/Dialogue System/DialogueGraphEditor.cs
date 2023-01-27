using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AustenKinney.Dialogue
{
    public class DialogueGraphEditor : EditorWindow
    {
        private DialogueGraphEditorView editorView;

        private string fileName = "New Dialogue Graph";

        [MenuItem("Tools/Dialogue/Dialogue Graph")]
        public static void CreateGraphViewWindow()
        {
            var window = GetWindow<DialogueGraphEditor>();
            window.titleContent = new GUIContent("Dialogue Graph");
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            GenerateMiniMap();
            GenerateBlackBoard();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(editorView);
        }

        private void ConstructGraphView()
        {
            editorView = new DialogueGraphEditorView(this)
            {
                name = "Dialogue Graph",
            };
            editorView.StretchToParentSize();
            rootVisualElement.Add(editorView);
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();

            toolbar.Add(new ToolbarSpacer());
            Label fileNameLabel = new Label(fileName);
            fileNameLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            fileNameLabel.style.minWidth = 180;
            toolbar.Add(fileNameLabel);


            toolbar.Add(new ToolbarSpacer());

            ToolbarMenu fileDropdown = new ToolbarMenu();
            fileDropdown.text = "File";
            fileDropdown.style.minWidth = 80;

            fileDropdown.menu.AppendAction("Save", a => RequestDataOperation(true), a => DropdownMenuAction.Status.Normal);
            fileDropdown.menu.AppendAction("Save As...", a => RequestDataOperation(true), a => DropdownMenuAction.Status.Normal);
            fileDropdown.menu.AppendAction("Load Data", a => RequestDataOperation(false), a => DropdownMenuAction.Status.Normal);
            toolbar.Add(fileDropdown);

            toolbar.Add(new Button(() => editorView.CreateNewDialogueNode("Dialogue Node", Vector2.zero)) {text = "New Node",});
            rootVisualElement.Add(toolbar);
        }

        private void RequestDataOperation(bool save)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var saveUtility = DialogueGraphSaveUtility.GetInstance(editorView);
                if (save)
                    saveUtility.SaveGraph(fileName);
                else
                    saveUtility.LoadGraph(fileName);
            }
            else
            {
                EditorUtility.DisplayDialog("Invalid File name", "Please Enter a valid filename", "OK");
            }
        }

        private void GenerateMiniMap()
        {
            var miniMap = new MiniMap { anchored = true };
            var cords = editorView.contentViewContainer.WorldToLocal(new Vector2(this.maxSize.x - 10, 30));
            miniMap.SetPosition(new Rect(cords.x, cords.y, 200, 140));
            editorView.Add(miniMap);
        }

        private void GenerateBlackBoard()
        {
            var blackboard = new Blackboard(editorView);
            blackboard.Add(new BlackboardSection { title = "Exposed Variables" });
            blackboard.addItemRequested = _blackboard =>
            {
                editorView.AddPropertyToBlackBoard(ExposedProperty.CreateInstance(), false);
            };
            blackboard.editTextRequested = (_blackboard, element, newValue) =>
            {
                var oldPropertyName = ((BlackboardField)element).text;
                if (editorView.ExposedProperties.Any(x => x.PropertyName == newValue))
                {
                    EditorUtility.DisplayDialog("Error", "This property name already exists, please chose another one.",
                        "OK");
                    return;
                }

                var targetIndex = editorView.ExposedProperties.FindIndex(x => x.PropertyName == oldPropertyName);
                editorView.ExposedProperties[targetIndex].PropertyName = newValue;
                ((BlackboardField)element).text = newValue;
            };
            blackboard.SetPosition(new Rect(10, 30, 200, 300));
            editorView.Add(blackboard);
            editorView.Blackboard = blackboard;
        }

    }
}
