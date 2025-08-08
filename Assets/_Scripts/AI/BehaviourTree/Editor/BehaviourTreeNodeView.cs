using UnityEditor.Experimental.GraphView;
using Descent.AI.BehaviourTree.Nodes;
using UnityEngine.UIElements;
using UnityEngine;
using Descent.AI.BehaviourTree.Core;
using UnityEditor;

namespace Descent.AI.BehaviourTree.Editor
{
    public class BehaviourTreeNodeView : Node
    {
        public BehaviourTreeNode Node { get; private set; }
        public Port Input { get; private set; }
        public Port Output { get; private set; }

        public BehaviourTreeNodeView(BehaviourTreeNode node, bool isRoot = false)
        {
            Node = node;
            title = Node.Name ?? Node.GetType().Name;

            Node.OnPropertyChanged += OnNodePropertyChanged;

            SetStyles(node);

            if (!isRoot)
            {
                Input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(BehaviourTreeNode));
                Input.portName = "";
                inputContainer.Add(Input);
            }

            if (node is BehaviourTreeCompositeNode)
            {
                Output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
                Output.portName = "";
                outputContainer.Add(Output);
            }

            RegisterCallback<GeometryChangedEvent>(OnMoved);

            RefreshExpandedState();
            RefreshPorts();
        }

        public void CleanUp()
        {
            Node.OnPropertyChanged -= OnNodePropertyChanged;
        }

        private void SetStyles(BehaviourTreeNode node)
        {
            AddToClassList("node");

            if (node is BehaviourTreeSelectorNode)
            {
                AddToClassList("selector");
            }
            else if (node is BehaviourTreeSequenceNode)
            {
                AddToClassList("sequence");
            }
            else if (node is BehaviourTreeActionNode)
            {
                AddToClassList("action");
            }
            else if (node is BehaviourTreeConditionNode)
            {
                AddToClassList("condition");
            }
        }

        public void UpdateTitle()
        {
            title = Node.Name ?? Node.GetType().Name;
            MarkDirtyRepaint();
        }

        private void OnMoved(GeometryChangedEvent evt)
        {
            Vector2 newPos = GetPosition().position;
            if (Node.Position != newPos)
            {
                Node.Position = newPos;
                EditorUtility.SetDirty(Node);
                AssetDatabase.SaveAssets();
            }
        }

        public void SetStatus(BehaviourTreeStatus status)
        {
            switch (status)
            {
                case BehaviourTreeStatus.Running:
                    titleContainer.style.backgroundColor = new Color(0.4f, 0.6f, 1.0f, 0.9f);
                    break;

                case BehaviourTreeStatus.Success:
                    titleContainer.style.backgroundColor = new Color(0.4f, 1.0f, 0.4f, 0.8f);
                    break;

                case BehaviourTreeStatus.Failure:
                    titleContainer.style.backgroundColor = new Color(1.0f, 0.4f, 0.4f, 0.8f);
                    break;

                default:
                    titleContainer.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.2f);
                    break;
            }
        }

        private void OnNodePropertyChanged(object sender, string propertyName)
        {
            if (propertyName == nameof(BehaviourTreeNode.Name))
            {
                UpdateTitle();
            }

            EditorUtility.SetDirty(Node);
            AssetDatabase.SaveAssets();
        }
    }
}