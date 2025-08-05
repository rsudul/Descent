using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Nodes;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using System;
using Descent.AI.BehaviourTree.Variables;

namespace Descent.AI.BehaviourTree.Editor
{
    public class BehaviourTreeGraphView : GraphView, IEdgeConnectorListener
    {
        private BehaviourTreeAsset _treeAsset;

        private readonly Dictionary<BehaviourTreeNode, BehaviourTreeNodeView> _nodeViews = 
            new Dictionary<BehaviourTreeNode, BehaviourTreeNodeView>();

        public IReadOnlyDictionary<BehaviourTreeNode, BehaviourTreeNodeView> NodeViews => _nodeViews;
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
            this.AddManipulator(new EdgeConnector<Edge>(this));

            GridBackground grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            graphViewChanged += OnGraphViewChanged;

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

            LoadExistingTree();
        }

        private void LoadExistingTree()
        {
            if (_treeAsset == null)
            {
                return;
            }

            _treeAsset.SyncAllNodes();

            if (_treeAsset.Root == null)
            {
                CreateRootNode();
            }

            CreateNodeRecursive(_treeAsset.Root, null);

            LoadOrphanedNodes();

            LoadValueConnections();
        }

        private void LoadOrphanedNodes()
        {
            foreach (BehaviourTreeNode node in _treeAsset.AllNodes)
            {
                if (_nodeViews.ContainsKey(node))
                {
                    continue;
                }

                BehaviourTreeNodeView nodeView = new BehaviourTreeNodeView(node);
                nodeView.userData = node.GUID;
                AddElement(nodeView);

                _nodeViews[node] = nodeView;
                nodeView.SetPosition(new Rect(node.Position, new Vector2(200, 150)));
            }
        }

        private void LoadValueConnections()
        {
            foreach (ValueConnection connection in _treeAsset.ValueConnections)
            {
                BehaviourTreeNodeView sourceView = _nodeViews.Values.FirstOrDefault(nv => nv.Node.GUID == connection.SourceNodeGUID);
                BehaviourTreeNodeView targetView = _nodeViews.Values.FirstOrDefault(nv => nv.Node.GUID == connection.TargetNodeGUID);

                if (sourceView == null || targetView == null)
                {
                    continue;
                }

                Port sourcePort = sourceView.outputContainer.Children().OfType<Port>()
                    .FirstOrDefault(p => p.portName == connection.SourcePinName);
                Port targetPort = targetView.inputContainer.Children().OfType<Port>()
                    .FirstOrDefault(p => p.portName == connection.TargetPinName);

                if (sourcePort != null && targetPort != null)
                {
                    Edge edge = sourcePort.ConnectTo(targetPort);
                    AddElement(edge);
                }
            }
        }

        // this is just for loading existing tree asset
        private void CreateRootNode()
        {
            _treeAsset.Root = ScriptableObject.CreateInstance<BehaviourTreeSelectorNode>();
            _treeAsset.Root.Name = "Root";
            _treeAsset.Root.ForceGenerateGuid();

            float horizontalCenter = contentViewContainer.layout.size.x * 0.5f;
            _treeAsset.Root.Position = new Vector2(horizontalCenter, 100.0f);
            AssetDatabase.AddObjectToAsset(_treeAsset.Root, _treeAsset);
        }

        // same as above
        private void CreateNodeRecursive(BehaviourTreeNode node, BehaviourTreeNodeView parentView)
        {
            BehaviourTreeNodeView nodeView = new BehaviourTreeNodeView(node);
            nodeView.userData = node.GUID;
            AddElement(nodeView);
            
            _nodeViews[node] = nodeView;

            nodeView.SetPosition(new Rect(node.Position, new Vector2(200, 150)));

            if (parentView != null)
            {
                Edge edge = parentView.Output.ConnectTo(nodeView.Input);
                AddElement(edge);
            }

            if (node is BehaviourTreeCompositeNode composite)
            {
                for (int i = 0; i < composite.Children.Count; i++)
                {
                    CreateNodeRecursive(composite.Children[i], nodeView);
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

        // this actually adds a new node
        private void AddNodeToGraph(BehaviourTreeNode node, Vector2 position)
        {
            CreateNodeView(node, position);

            if (_treeAsset.Root is BehaviourTreeCompositeNode rootComposite
                && rootComposite.Children.Count == 0)
            {
                ConnectNodes(rootComposite, node);
            }

            MarkDirty();
        }

        private BehaviourTreeNodeView CreateNodeView(BehaviourTreeNode node, Vector2 position)
        {
            node.ForceGenerateGuid();

            BehaviourTreeNodeView nodeView = new BehaviourTreeNodeView(node);
            nodeView.userData = node.GUID;
            AddElement(nodeView);
            nodeView.SetPosition(new Rect(position, new Vector2(200.0f, 150.0f)));
            _nodeViews[node] = nodeView;
            AssetDatabase.AddObjectToAsset(node, _treeAsset);

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

            EditorUtility.SetDirty(node);

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
                List<Edge> validEdges = new List<Edge>();

                foreach (Edge edge in change.edgesToCreate)
                {
                    ValuePinView outPin = edge.output as ValuePinView;
                    ValuePinView inPin = edge.input as ValuePinView;

                    if (outPin != null && inPin != null)
                    {
                        if (outPin.Definition.Type != inPin.Definition.Type)
                        {
                            continue;
                        }
                    }

                    validEdges.Add(edge);
                }

                change.edgesToCreate = validEdges;

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
            foreach (GraphElement element in change.elementsToRemove)
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

            nodeView.CleanUp();

            if (nodeView.Node.Parent is BehaviourTreeCompositeNode parentComposite)
            {
                parentComposite.RemoveChild(nodeView.Node);
                AssetDatabase.RemoveObjectFromAsset(nodeView.Node);
                MarkDirty();
            }
        }

        private void RemoveEdge(Edge edge)
        {
            if (edge.output is Port outPort && edge.input is Port inPort)
            {
                ValueConnection valueConnection = new ValueConnection
                {
                    SourceNodeGUID = (string)outPort.node.userData,
                    SourcePinName = outPort.portName,
                    TargetNodeGUID = (string)inPort.node.userData,
                    TargetPinName = inPort.portName
                };
                _treeAsset.RemoveValueConnection(valueConnection);
                EditorUtility.SetDirty(_treeAsset);
            }

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

                bool isValuePinConnection = !string.IsNullOrEmpty(edge.output.portName) ||
                                            !string.IsNullOrEmpty(edge.input.portName);

                if (isValuePinConnection)
                {
                    ValueConnection valueConnection = new ValueConnection
                    {
                        SourceNodeGUID = parentView.Node.GUID,
                        SourcePinName = edge.output.portName,
                        TargetNodeGUID = childView.Node.GUID,
                        TargetPinName = edge.input.portName
                    };
                    _treeAsset.AddValueConnection(valueConnection);
                    EditorUtility.SetDirty(_treeAsset);
                    return;
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
            AssetDatabase.RemoveObjectFromAsset(nodeView.Node);

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
            List<GraphElement> elements = graphElements.ToList();
            foreach (GraphElement element in elements)
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

        public void RefreshNodeStatuses()
        {
            foreach (var nodePair in _nodeViews)
            {
                BehaviourTreeNode node = nodePair.Key;
                BehaviourTreeNodeView nodeView = nodePair.Value;
                nodeView.SetStatus(node.Status);
            }
        }

        public void CreateVariableNode(string variableGuid, VariableType variableType, Vector2 position)
        {
            var node = ScriptableObject.CreateInstance<BehaviourTreeGetVariableNode>();

            VariableDefinition variableDefinition = _treeAsset.VariableContainer.GetByGUID(variableGuid);
            node.Name = variableDefinition != null ? variableDefinition.Name : "Get Variable";
            node.VariableGUID = variableGuid;
            node.VariableType = variableType;
            node.Position = position;

            AddNodeToGraph(node, position);
        }

        public void OnDrop(GraphView graphView, Edge edge)
        {

        }

        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {

        }
    }
}