using UnityEngine;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Nodes;
using Descent.Common.Attributes.AI;
using Descent.AI.BehaviourTree.Requests;
using Descent.AI.BehaviourTree.Events.Arguments;
using System;
using System.Collections.Generic;

namespace Descent.AI.BehaviourTree.Core
{
    [RequireComponent(typeof(BehaviourTreeActionRequestDispatcher))]
    public class BehaviourTreeRunner : MonoBehaviour
    {
        private BehaviourTreeActionRequestDispatcher _dispatcher;

        private BehaviourTreeNode _rootNodeInstance;
        private BehaviourTreeContextRegistry _contextRegistry;
        private List<(IBehaviourTreeContextProvider provider, Type contextType)> _providerEntries;

        private float _tickTimer = 0.0f;

        public bool PauseTree = false;

        public event EventHandler<EventArgs> OnTreeStarted;
        public event EventHandler<EventArgs> OnTreeStopped;
        public event EventHandler<EventArgs> OnTreeReset;
        public event EventHandler<BehaviourTreeAsset> OnTreeAssetChanged;
        public event EventHandler<TickEventArgs> OnTreeTick;

        [SerializeField]
        private BehaviourTreeAsset _treeAsset;
        [SerializeField]
        private float _tickInterval = 0.1f;
        [SerializeField]
        private bool _debug = false;

        private void Start()
        {
            _dispatcher = GetComponent<BehaviourTreeActionRequestDispatcher>();
            _contextRegistry = new BehaviourTreeContextRegistry(_treeAsset.VariableContainer);

            InitializeProviders();
            RefreshContexts();

            ResetAndRebuildTree();
        }

        private void OnDisable()
        {
            OnTreeStopped?.Invoke(this, EventArgs.Empty);
        }

        private void OnDestroy()
        {
            ResetAndRebuildTree();
        }

        private void Update()
        {
            if (_rootNodeInstance == null || _contextRegistry?.Count == 0 || PauseTree)
            {
                return;
            }

            _tickTimer += Time.deltaTime;
            if (_tickTimer >= _tickInterval)
            {
                RefreshContexts();

                BehaviourTreeStatus status = _rootNodeInstance.Tick(_contextRegistry);
                LogNodeStatus(status);
                OnTreeTick?.Invoke(this, new TickEventArgs(_tickTimer, status));
                _tickTimer = 0.0f;
            }
        }

        private void InitializeProviders()
        {
            _providerEntries = new List<(IBehaviourTreeContextProvider provider, Type contextType)>();

            foreach (Component component in GetComponents<MonoBehaviour>())
            {
                if (component is not IBehaviourTreeContextProvider provider)
                {
                    continue;
                }

                var attrs = component.GetType().GetCustomAttributes(typeof(BehaviourTreeContextProviderAttribute), true);

                foreach (BehaviourTreeContextProviderAttribute attr in attrs)
                {
                    _providerEntries.Add((provider, attr.ContextType));
                }
            }
        }

        private void RefreshContexts()
        {
            foreach (var (provider, contextType) in _providerEntries)
            {
                BehaviourTreeContext context = provider.GetBehaviourTreeContext(contextType, gameObject);
                if (context == null)
                {
                    continue;
                }

                _contextRegistry.RegisterContext(contextType, context);
            }
        }

        private void BuildContextRegistry()
        {
            _contextRegistry = new BehaviourTreeContextRegistry(_treeAsset.VariableContainer);

            foreach (Component component in GetComponents<MonoBehaviour>())
            {
                if (component is not IBehaviourTreeContextProvider provider)
                {
                    continue;
                }

                var attrs = component.GetType().GetCustomAttributes(typeof(BehaviourTreeContextProviderAttribute), true);

                if (attrs.Length == 0)
                {
                    continue;
                }

                foreach (BehaviourTreeContextProviderAttribute attr in attrs)
                {
                    BehaviourTreeContext context = provider.GetBehaviourTreeContext(attr.ContextType, gameObject);
                    if (context == null || _contextRegistry.GetContext(attr.ContextType) != null)
                    {
                        continue;
                    }

                    _contextRegistry.RegisterContext(attr.ContextType, context);
                }
            }
        }

        public void ResetTree()
        {
            if (_rootNodeInstance != null)
            {
                ResetNodeRecursive(_rootNodeInstance);
            }
            OnTreeReset?.Invoke(this, EventArgs.Empty);
        }

        private void ResetNodeRecursive(BehaviourTreeNode node)
        {
            if (node == null)
            {
                return;
            }

            node.ResetNode();

            if (node is not BehaviourTreeCompositeNode compositeNode)
            {
                return;
            }

            foreach (BehaviourTreeNode child in compositeNode.Children)
            {
                ResetNodeRecursive(child);
            }
        }

        public bool HasTreeAsset(BehaviourTreeAsset asset)
        {
            return _treeAsset == asset;
        }

        private void InjectDispatcherToActions(BehaviourTreeNode node, BehaviourTreeActionRequestDispatcher dispatcher)
        {
            if (node is BehaviourTreeActionNode actionNode && actionNode.Action != null)
            {
                actionNode.Action.InjectDispatcher(dispatcher);
            }

            if (node is BehaviourTreeCompositeNode compositeNode)
            {
                foreach (BehaviourTreeNode child in compositeNode.Children)
                {
                    InjectDispatcherToActions(child, dispatcher);
                }
            }
            else if (node is BehaviourTreeRepeatUntilFailureNode repeatUntilFailureNode)
            {
                if (repeatUntilFailureNode.Child != null)
                {
                    InjectDispatcherToActions(repeatUntilFailureNode.Child, dispatcher);
                }
            }
            else if (node is BehaviourTreeRepeatWhileConditionNode repeatWhileConditionNode)
            {
                foreach (BehaviourTreeNode child in repeatWhileConditionNode.Children)
                {
                    InjectDispatcherToActions(child, dispatcher);
                }
            }
        }

        public void SetBehaviourTreeAsset(BehaviourTreeAsset asset)
        {
            if (HasTreeAsset(asset))
            {
                return;
            }

            _treeAsset = asset;

            OnTreeAssetChanged?.Invoke(this, _treeAsset);

            ResetAndRebuildTree();
        }

        private void ResetAndRebuildTree()
        {
            ResetTree();
            _rootNodeInstance = null;
            _tickTimer = 0.0f;

            if (_treeAsset == null)
            {
                Debug.LogWarning($"{name} has no BehaviourTreeAsset assigned.");
                enabled = false;
                return;
            }

            _rootNodeInstance = _treeAsset.CloneTree();

            if (_dispatcher == null)
            {
                Debug.LogWarning($"No action request dispatcher found on {gameObject.name}");
            } else
            {
                InjectDispatcherToActions(_rootNodeInstance, _dispatcher);
            }

            OnTreeStarted?.Invoke(this, EventArgs.Empty);
        }

        private void LogNodeStatus(BehaviourTreeStatus status)
        {
            if (!_debug)
            {
                return;
            }

            Debug.Log($"[BT][{gameObject.name}] Tick: Root node status = {status}");
        }
    }
}