using UnityEditor.Experimental.GraphView;
using Descent.Common.AI.BehaviourTree.Nodes;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Editor
{
    public class BehaviourTreeNodeView : Node
    {
        public BehaviourTreeNode Node { get; private set; }
        public Port Input { get; private set; }
        public Port Output { get; private set; }

        public BehaviourTreeNodeView(BehaviourTreeNode node)
        {
            Node = node;
            title = Node.Name ?? Node.GetType().Name;

            SetStyles(node);

            Input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            Input.portName = "";
            inputContainer.Add(Input);

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
                // !!! SAVE ASSETS LATER - WHEN YOU PRESS SAVE BUTTON !!!
                AssetDatabase.SaveAssets();
            }
        }
    }
}