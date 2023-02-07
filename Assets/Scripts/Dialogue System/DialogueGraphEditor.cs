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

        private Label fileNameLabel;

        [MenuItem("Tools/Dialogue/Dialogue Graph")]
        public static void CreateGraphViewWindow()
        {
            var window = GetWindow<DialogueGraphEditor>();
            window.titleContent = new GUIContent("Dialogue Graph");
        }

        #region Enable & Disable

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            GenerateMiniMap();
            GenerateBlackBoard();
        }

        private void OnDisable()
        {
            for (int i = rootVisualElement.childCount; i > 0; i--)
            {
                rootVisualElement.RemoveAt(i - 1);
            }
        }

        #endregion

        #region Editor Window

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

            //Name label
            fileNameLabel = new Label(fileName);
            fileNameLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            fileNameLabel.style.minWidth = 180;
            toolbar.Add(fileNameLabel);

            //file dropdown
            ToolbarMenu fileDropdown = new ToolbarMenu();
            fileDropdown.text = "File";
            fileDropdown.style.minWidth = 80;
            fileDropdown.menu.AppendAction("New Graph", a => NewFile(), a => DropdownMenuAction.Status.Normal);
            fileDropdown.menu.AppendAction("Save", a => RequestDataOperation(true), a => DropdownMenuAction.Status.Normal);
            fileDropdown.menu.AppendAction("Save As...", a => SaveAs(), a => DropdownMenuAction.Status.Normal);
            fileDropdown.menu.AppendAction("Load Data", a => Load(), a => DropdownMenuAction.Status.Normal);
            toolbar.Add(fileDropdown);

            //node dropdown
            ToolbarMenu nodeDropdown = new ToolbarMenu();
            nodeDropdown.text = "Node";
            nodeDropdown.style.minWidth = 80;
            nodeDropdown.menu.AppendAction("Dialogue Node", a => editorView.CreateNewDialogueNode("Dialogue Node", Vector2.zero, NodeType.Dialogue), a => DropdownMenuAction.Status.Normal);
            nodeDropdown.menu.AppendAction("Choice Node", a => editorView.CreateNewDialogueNode("Choice Node", Vector2.zero, NodeType.Choice), a => DropdownMenuAction.Status.Normal);
            nodeDropdown.menu.AppendAction("Test Property Node", a => editorView.CreateNewDialogueNode("Test Property Node", Vector2.zero, NodeType.TestProperty), a => DropdownMenuAction.Status.Normal);
            nodeDropdown.menu.AppendAction("Set Property Node", a => editorView.CreateNewDialogueNode("Set Property Node", Vector2.zero, NodeType.SetProperty), a => DropdownMenuAction.Status.Normal);
            toolbar.Add(nodeDropdown);

            rootVisualElement.Add(toolbar);
        }

        #endregion

        #region Saving & Loading

        private void NewFile()
        {
            for (int i = rootVisualElement.childCount; i > 0; i--)
            {
                rootVisualElement.RemoveAt(i - 1);
            }

            fileName = "New Dialogue Graph";

            ConstructGraphView();
            GenerateToolbar();
            GenerateMiniMap();
            GenerateBlackBoard();   
        }

        private void SaveAs()
        {
            string savePath = EditorUtility.SaveFilePanelInProject("Save As...", fileName + ".asset", "asset", "Select a location to save the dialogue data.");
            fileName = savePath.Replace("Assets/Resources/Dialogue/", "");
            fileName = fileName.Replace(".asset", "");
            Debug.Log(fileName);
            RequestDataOperation(true, savePath);

        }

        private void Load()
        {
            string loadPath = EditorUtility.OpenFilePanel("Load", Application.dataPath + "/Resources/Dialogue/" + fileName + ".asset", "asset");
            loadPath = loadPath.Replace(Application.dataPath, "Assets");
            fileName = loadPath.Replace("Assets/Resources/Dialogue/", "");
            fileName = fileName.Replace(".asset", "");
            Debug.Log(fileName);
            RequestDataOperation(false, loadPath);
        }

        private void RequestDataOperation(bool save, string path = null)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var saveUtility = DialogueGraphSaveUtility.GetInstance(editorView);
                if (save == true && string.IsNullOrEmpty(path) == true)
                    saveUtility.SaveGraph(fileName);
                else if (save == true && string.IsNullOrEmpty(path) == false)
                    saveUtility.SaveGraph(fileName, path);
                else if (save == false && string.IsNullOrEmpty(path) == true)
                    saveUtility.LoadGraph(fileName);
                else
                    saveUtility.LoadGraph(fileName, path);

                fileNameLabel.text = fileName;
            }
            else
            {
                EditorUtility.DisplayDialog("Invalid File name", "Please Enter a valid filename", "OK");
            }
        }

        #endregion

        #region Mini-Map

        private void GenerateMiniMap()
        {
            var miniMap = new MiniMap { anchored = true };
            var cords = editorView.contentViewContainer.WorldToLocal(new Vector2(this.maxSize.x - 10, 30));
            miniMap.SetPosition(new Rect(cords.x, cords.y, 200, 140));
            editorView.Add(miniMap);
        }

        #endregion

        #region Blackboard

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

        #endregion
    }
}
