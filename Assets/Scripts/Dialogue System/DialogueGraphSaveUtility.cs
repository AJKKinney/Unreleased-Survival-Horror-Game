using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace AustenKinney.Dialogue
{
    public class DialogueGraphSaveUtility
    {
        private List<Edge> Edges => editorView.edges.ToList();
        private List<DialogueNode> Nodes => editorView.Nodes;

        private List<Group> CommentBlocks => editorView.graphElements.ToList().Where(x => x is Group).Cast<Group>().ToList();

        private DialogueGraphData graphData;

        private DialogueGraphEditorView editorView;


        public static DialogueGraphSaveUtility GetInstance(DialogueGraphEditorView graphView)
        {
            return new DialogueGraphSaveUtility
            {
                editorView = graphView
            };
        }

        #region Saving

        public void SaveGraph(string fileName, string path = null)
        {
            if(string.IsNullOrEmpty(path) == true)
            {
                path = $"Assets/Resources/Dialogue/{fileName}.asset";
            }

            var dialogueContainerObject = ScriptableObject.CreateInstance<DialogueGraphData>();
            if (!SaveNodes(fileName, dialogueContainerObject)) return;
            SaveExposedProperties(dialogueContainerObject);
            SaveCommentBlocks(dialogueContainerObject);

            UnityEngine.Object loadedAsset = AssetDatabase.LoadAssetAtPath(path, typeof(DialogueGraphData));

            if (loadedAsset == null || !AssetDatabase.Contains(loadedAsset))
            {
                AssetDatabase.CreateAsset(dialogueContainerObject, path);
            }
            else
            {
                DialogueGraphData container = loadedAsset as DialogueGraphData;
                container.NodeLinks = dialogueContainerObject.NodeLinks;
                container.DialogueNodeData = dialogueContainerObject.DialogueNodeData;
                container.ExposedProperties = dialogueContainerObject.ExposedProperties;
                container.CommentBlockData = dialogueContainerObject.CommentBlockData;
                EditorUtility.SetDirty(container);
            }

            AssetDatabase.SaveAssets();
        }

        private bool SaveNodes(string fileName, DialogueGraphData dialogueContainerObject)
        {
            foreach (var perNode in Nodes)
            {
                Debug.Log(perNode.Type);
            }

                if (!Edges.Any()) return false;
            var connectedSockets = Edges.Where(x => x.input.node != null).ToArray();
            for (var i = 0; i < connectedSockets.Count(); i++)
            {
                var outputNode = (connectedSockets[i].output.node as DialogueNode);
                var inputNode = (connectedSockets[i].input.node as DialogueNode);
                dialogueContainerObject.NodeLinks.Add(new NodeLinkData
                {
                    BaseNodeGUID = outputNode.GUID,
                    PortName = connectedSockets[i].output.portName,
                    TargetNodeGUID = inputNode.GUID,
                });

                Debug.Log("Node " + outputNode.GUID);
            }

            foreach (var node in Nodes.Where(node => node.Type != NodeType.Entry))
            {
                Debug.Log(node.title);
                Debug.Log(node.GUID);

                dialogueContainerObject.DialogueNodeData.Add(new DialogueNodeData
                {
                    NodeGUID = node.GUID,
                    NodeTitle = node.title,
                    DialogueText = node.DialogueText,
                    Position = node.GetPosition().position,
                    Type = node.Type
                });
            }

            return true;
        }

        private void SaveExposedProperties(DialogueGraphData dialogueContainer)
        {
            dialogueContainer.ExposedProperties.Clear();
            dialogueContainer.ExposedProperties.AddRange(editorView.ExposedProperties);
        }

        private void SaveCommentBlocks(DialogueGraphData dialogueContainer)
        {
            foreach (var block in CommentBlocks)
            {
                var nodes = block.containedElements.Where(x => x is DialogueNode).Cast<DialogueNode>().Select(x => x.GUID).ToList();

                dialogueContainer.CommentBlockData.Add(new CommentBlockData
                {
                    ChildNodes = nodes,
                    Title = block.title,
                    Position = block.GetPosition().position
                });
            }
        }

        #endregion

        #region Loading

        public void LoadGraph(string fileName, string path = null)
        {
            if(string.IsNullOrEmpty(path))
            {
                path = $"Assets/Resources/Dialogue/{fileName}.asset";
            }

            graphData = AssetDatabase.LoadAssetAtPath(path, typeof(DialogueGraphData)) as DialogueGraphData;
            if(graphData == null)
            {
                EditorUtility.DisplayDialog("File Not Found", "Target Narrative Data does not exist!", "OK");
                return;
            }

            ClearGraph();
            GenerateDialogueNodes();
            ConnectDialogueNodes();
            AddExposedProperties();
            GenerateCommentBlocks();
        }

        /// <summary>
        /// Set Entry point GUID then Get All Nodes, remove all and their edges. Leave only the entrypoint node. (Remove its edge too)
        /// </summary>
        private void ClearGraph()
        {
            Nodes.Find(x => x.Type == NodeType.Entry).GUID = graphData.NodeLinks[0].BaseNodeGUID;
            foreach (var perNode in Nodes)
            {
                Debug.Log(perNode.Type);
                if (perNode.Type != NodeType.Entry)
                {
                    Debug.Log("Continue");
                    Edges.Where(x => x.input.node == perNode).ToList().ForEach(edge => editorView.RemoveElement(edge));
                    editorView.RemoveElement(perNode);
                }
            }
        }

        /// <summary>
        /// Create All serialized nodes and assign their guid and dialogue text to them
        /// </summary>
        private void GenerateDialogueNodes()
        {
            foreach (var perNode in graphData.DialogueNodeData)
            {
                var tempNode = editorView.CreateNode(perNode.NodeTitle,  Vector2.zero, perNode.Type, perNode.DialogueText);
                tempNode.GUID = perNode.NodeGUID;
                editorView.AddElement(tempNode);

                var nodePorts = graphData.NodeLinks.Where(x => x.BaseNodeGUID == perNode.NodeGUID).ToList();
                if (tempNode.Type == NodeType.Choice)
                {
                    nodePorts.ForEach(x => editorView.AddChoicePort(tempNode, x.PortName));
                }
                else
                {
                    nodePorts.ForEach(x => editorView.AddOutputPort(tempNode, x.PortName));
                }
            }
        }

        private void ConnectDialogueNodes()
        {
            for (var i = 0; i < Nodes.Count; i++)
            {
                var k = i; //Prevent access to modified closure
                var connections = graphData.NodeLinks.Where(x => x.BaseNodeGUID == Nodes[k].GUID).ToList();
                for (var j = 0; j < connections.Count(); j++)
                {
                    var targetNodeGUID = connections[j].TargetNodeGUID;
                    Debug.Log(targetNodeGUID);
                    for (var d = 0; d < connections.Count(); d++)
                    {
                        Debug.Log(Nodes[d].GUID.ToString());
                    }
                    var targetNode = Nodes.First(x => x.GUID == targetNodeGUID);
                    LinkNodesTogether(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                    targetNode.SetPosition(new Rect(
                        graphData.DialogueNodeData.First(x => x.NodeGUID == targetNodeGUID).Position,
                        editorView.DefaultNodeSize));
                }
            }
        }

        private void LinkNodesTogether(Port outputSocket, Port inputSocket)
        {
            var tempEdge = new Edge()
            {
                output = outputSocket,
                input = inputSocket
            };
            tempEdge?.input.Connect(tempEdge);
            tempEdge?.output.Connect(tempEdge);
            editorView.Add(tempEdge);
        }

        private void AddExposedProperties()
        {
            editorView.ClearBlackBoardAndExposedProperties();
            foreach (var exposedProperty in graphData.ExposedProperties)
            {
                editorView.AddPropertyToBlackBoard(exposedProperty);
            }
        }

        private void GenerateCommentBlocks()
        {
            foreach (var commentBlock in CommentBlocks)
            {
                editorView.RemoveElement(commentBlock);
            }

            foreach (var commentBlockData in graphData.CommentBlockData)
            {
                var block = editorView.CreateCommentBlock(new Rect(commentBlockData.Position, editorView.DefaultCommentBlockSize),
                     commentBlockData);
                block.AddElements(Nodes.Where(x => commentBlockData.ChildNodes.Contains(x.GUID)));
            }
        }
        #endregion
    }
}
