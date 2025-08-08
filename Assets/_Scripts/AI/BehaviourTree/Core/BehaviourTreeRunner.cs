using UnityEngine;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Nodes;
using Descent.Common.Attributes.AI;
using Descent.AI.BehaviourTree.Requests;
using Descent.AI.BehaviourTree.Events.Arguments;
using System;
using System.Collections.Generic;
using Descent.AI.BehaviourTree.Services;

namespace Descent.AI.BehaviourTree.Core
{
    [RequireComponent(typeof(BehaviourTreeActionRequestDispatcher))]
    public class BehaviourTreeRunner : MonoBehaviour
    {
        private BehaviourTreeActionRequestDispatcher _dispatcher;

        private readonly List<IBehaviourTreeService> _services = new List<IBehaviourTreeService>();
        private readonly List<float> _serviceNextTimes = new List<float>();

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
            _contextRegistry = new BehaviourTreeContextRegistry();

            InitializeProviders();
            RefreshContexts();
            InitializeServices();

            ResetAndRebuildTree();
        }

        private void OnDisable()
        {
            foreach (IBehaviourTreeService service in _services)
            {
                try
                {
                    service.OnStop();
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex, this);
                }
            }

            OnTreeStopped?.Invoke(this, EventArgs.Empty);
        }

        private void OnDestroy()
        {
            foreach (IBehaviourTreeService service in _services)
            {
                try
                {
                    service.OnStop();
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex, this);
                }
            }

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

                float now = Time.time;
                float deltaTime = Time.deltaTime;
                for (int i = 0; i < _services.Count; i++)
                {
                    if (now >= _serviceNextTimes[i])
                    {
                        try
                        {
                            _services[i].Tick(_contextRegistry, deltaTime);
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogException(ex, this);
                        }

                        _serviceNextTimes[i] = now + MathF.Max(0.01f, _services[i].Interval);
                    }
                }

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

            Debug.Log($"[BT][{gameObject.name}] Tick: Root = {status}\n" + BuildActivePath(_rootNodeInstance, 0));
        }

        private string BuildActivePath(BehaviourTreeNode node, int d)
        {
            if (node == null)
            {
                return string.Empty;
            }

            string line = new string(' ', d * 2) + $"- {node.GetType().Name} [{node.Status}]\n";

            if (node is BehaviourTreeCompositeNode comp)
            {
                foreach (BehaviourTreeNode child in comp.Children)
                {
                    if (child.Status == BehaviourTreeStatus.Running)
                    {
                        line += BuildActivePath(child, d + 1);
                    }
                }
            }

            return line;
        }

        private void InitializeServices()
        {
            _services.Clear();
            _serviceNextTimes.Clear();

            foreach (MonoBehaviour component in GetComponents<MonoBehaviour>())
            {
                if (component is IBehaviourTreeService service)
                {
                    _services.Add(service);
                    _serviceNextTimes.Add(0.0f);
                }
            }

            foreach (IBehaviourTreeService service in _services)
            {
                try
                {
                    service.OnStart(_contextRegistry, gameObject);
                } catch (System.Exception ex)
                {
                    Debug.LogException(ex, this);
                }
            }
        }
    }
}