using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Descent.Common.AI.BehaviourTree.Core;
using Descent.Common.AI.BehaviourTree.Nodes;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Descent.Common.AI.BehaviourTree.Editor
{
    public class BehaviourTreeGraphView : GraphView
    {
        private BehaviourTreeAsset _treeAsset;

        private readonly Dictionary<BehaviourTreeNode, BehaviourTreeNodeView> _nodeViews = 
            new Dictionary<BehaviourTreeNode, BehaviourTreeNodeView>();

        public bool IsDirty { get; private set; } = false;

        public event EventHandler<BehaviourTreeNode> OnNodeSelected;
        public event EventHandler<BehaviourTreeNodeView> OnNodeDeleted;

        public BehaviourTreeGraphView(EditorWindow editorWindow)
        {
            styleSheets.Add(Resources.Load<StyleSheet>("BehaviourTreeGraphViewStyles"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            GridBackground grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            graphViewChanged = OnGraphViewChanged;

            graphViewChanged += (change) => change;

            RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt.keyCode == KeyCode.Delete || evt.keyCode == KeyCode.Backspace)
                {
                    if (selection.FirstOrDefault() is BehaviourTreeNodeView nodeView)
                    {
                        DeleteNode(nodeView);
                    }
                }
            });

            RegisterCallback<MouseUpEvent>(evt =>
            {
                BehaviourTreeNodeView selectedNodeView = selection.OfType<BehaviourTreeNodeView>().FirstOrDefault();
                OnNodeSelected?.Invoke(this, selectedNodeView?.Node);
            });
        }

        public void PopulateView(BehaviourTreeAsset tree)
        {
            ClearGraph();

            if (tree == null)
            {
                return;
            }

            _treeAsset = tree;

            if (_treeAsset.Root == null)
            {
                _treeAsset.Root = new BehaviourTreeSelectorNode { Name = "Root" };
                MarkDirty();
            }

            CreateNodeRecursive(_treeAsset.Root, null, new Vector2(100, 200));
        }

        private void CreateNodeRecursive(BehaviourTreeNode node, BehaviourTreeNodeView parentView, Vector2 position)
        {
            BehaviourTreeNodeView nodeView = new BehaviourTreeNodeView(node);
            AddElement(nodeView);
            _nodeViews[node] = nodeView;

            nodeView.SetPosition(new Rect(position, new Vector2(200, 150)));

            if (parentView != null)
            {
                var edge = parentView.Output.ConnectTo(nodeView.Input);
                AddElement(edge);
            }

            if (node is BehaviourTreeCompositeNode composite)
            {
                float yOffset = 200.0f;
                for (int i = 0; i < composite.Children.Count; i++)
                {
                    Vector2 childPos = position + new Vector2(300.0f, i + yOffset);
                    CreateNodeRecursive(composite.Children[i], nodeView, childPos);
                }
            }
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            Vector2 mousePosition = evt.localMousePosition;

            foreach (NodeCreationMenuItem item in BehaviourTreeNodeRegistry.NodeTypes)
            {
                evt.menu.AppendAction($"Add/{item.DisplayName}", action =>
                {
                    BehaviourTreeNode node = item.CreateFunc();
                    AddNodeToGraph(node, mousePosition);
                });
            }

            base.BuildContextualMenu(evt);
        }

        private void AddNodeToGraph(BehaviourTreeNode node, Vector2 position)
        {
            CreateNodeView(node, position);

            if (_treeAsset.Root is BehaviourTreeCompositeNode rootComposite)
            {
                ConnectNodes(rootComposite, node);
            }

            MarkDirty();
        }

        private BehaviourTreeNodeView CreateNodeView(BehaviourTreeNode node, Vector2 position)
        {
            BehaviourTreeNodeView nodeView = new BehaviourTreeNodeView(node);
            AddElement(nodeView);
            nodeView.SetPosition(new Rect(position, new Vector2(200.0f, 150.0f)));
            _nodeViews[node] = nodeView;

            nodeView.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button == 0 && evt.clickCount == 1)
                {
                    OnNodeSelected?.Invoke(this, node);
                }
            });

            nodeView.AddManipulator(new ContextualMenuManipulator(evt =>
            {
                evt.menu.AppendAction("Delete Node", _ => DeleteNode(nodeView));
            }));

            return nodeView;
        }

        private void ConnectNodes(BehaviourTreeNode parent, BehaviourTreeNode child)
        {
            if (parent is BehaviourTreeCompositeNode composite)
            {
                if (composite.Children.Contains(child))
                {
                    return;
                }

                composite.AddChild(child);
                child.Parent = parent;

                BehaviourTreeNodeView parentView = GetNodeView(parent);
                BehaviourTreeNodeView childView = GetNodeView(child);

                if (parentView?.Output != null && childView?.Input != null)
                {
                    Edge edge = parentView.Output.ConnectTo(childView.Input);
                    AddElement(edge);
                }

                MarkDirty();
            }
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange change)
        {
            if (change.edgesToCreate != null)
            {
                CreateEdges(change);
            }

            if (change.elementsToRemove != null)
            {
                RemoveElements(change);
            }

            return change;
        }

        private void RemoveElements(GraphViewChange change)
        {
            foreach (var element in change.elementsToRemove)
            {
                if (element is Edge edge)
                {
                    RemoveEdge(edge);
                }

                if (element is BehaviourTreeNodeView nodeView)
                {
                    RemoveNodeView(nodeView);
                }
            }
        }

        private void RemoveNodeView(BehaviourTreeNodeView nodeView)
        {
            if (_nodeViews.ContainsKey(nodeView.Node))
            {
                _nodeViews.Remove(nodeView.Node);
            }

            if (nodeView.Node.Parent is BehaviourTreeCompositeNode parentComposite)
            {
                parentComposite.RemoveChild(nodeView.Node);
                MarkDirty();
            }
        }

        private void RemoveEdge(Edge edge)
        {
            BehaviourTreeNodeView parentView = edge.output.node as BehaviourTreeNodeView;
            BehaviourTreeNodeView childView = edge.input.node as BehaviourTreeNodeView;

            if (parentView?.Node is BehaviourTreeCompositeNode composite && childView?.Node != null)
            {
                if (composite.Children.Contains(childView.Node))
                {
                    composite.RemoveChild(childView.Node);
                    childView.Node.Parent = null;
                    MarkDirty();
                }
            }
        }

        private void CreateEdges(GraphViewChange change)
        {
            foreach (Edge edge in change.edgesToCreate)
            {
                BehaviourTreeNodeView parentView = edge.output.node as BehaviourTreeNodeView;
                BehaviourTreeNodeView childView = edge.input.node as BehaviourTreeNodeView;

                if (parentView == null || childView == null)
                {
                    continue;
                }

                BehaviourTreeNode parentNode = parentView.Node;
                BehaviourTreeNode childNode = childView.Node;

                if (parentNode is BehaviourTreeCompositeNode composite)
                {
                    if (!composite.Children.Contains(childNode))
                    {
                        composite.AddChild(childNode);
                        childNode.Parent = parentNode;
                        MarkDirty();
                    }
                }
                else
                {
                    Debug.LogWarning($"Cannot connect children to non-composite node: {parentNode.GetType().Name}");
                }
            }
        }

        private BehaviourTreeNodeView GetNodeView(BehaviourTreeNode node)
        {
            if (_nodeViews.ContainsKey(node))
            {
                return _nodeViews[node];
            }

            return null;
        }

        private void DisconnectEdge(Edge edge)
        {
            edge.output.Disconnect(edge);
            edge.input.Disconnect(edge);
            RemoveElement(edge);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
                endPort.direction != startPort.direction &&
                endPort.node != startPort.node
            ).ToList();
        }

        public void RefreshNodeTitle(BehaviourTreeNode node)
        {
            if (_nodeViews.TryGetValue(node, out BehaviourTreeNodeView nodeView))
            {
                nodeView.UpdateTitle();
            }
        }

        public void DeleteNode(BehaviourTreeNodeView nodeView)
        {
            if (nodeView == null || nodeView.Node == null)
            {
                return;
            }

            BehaviourTreeNode node = nodeView.Node;

            Undo.RegisterCompleteObjectUndo(_treeAsset, "Delete Node");

            if (node.Parent is BehaviourTreeCompositeNode parentComposite)
            {
                parentComposite.RemoveChild(node);
            }

            foreach (Edge edge in edges.ToList())
            {
                if (edge.input?.node == nodeView || edge.output?.node == nodeView)
                {
                    RemoveElement(edge);
                }
            }

            RemoveElement(nodeView);

            _nodeViews.Remove(node);

            if (_treeAsset.Root == node)
            {
                _treeAsset.Root = null;
            }

            OnNodeDeleted?.Invoke(this, nodeView);

            MarkDirty();
        }

        private void ClearGraph()
        {
            var elements = graphElements.ToList();
            foreach (var element in elements)
            {
                RemoveElement(element);
            }

            _nodeViews.Clear();
        }

        public void MarkDirty()
        {
            IsDirty = true;
        }

        public void ClearDirtyFlag()
        {
            IsDirty = false;
        }
    }
}