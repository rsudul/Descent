using UnityEngine;
using Descent.Common.AI.BehaviourTree.Core.Context;
using Descent.Common.AI.BehaviourTree.Nodes;
using Descent.Attributes.AI;
using Descent.Common.AI.BehaviourTree.Core.Requests;

namespace Descent.Common.AI.BehaviourTree.Core
{
    [RequireComponent(typeof(BehaviourTreeActionRequestDispatcher))]
    public class BehaviourTreeRunner : MonoBehaviour
    {
        private BehaviourTreeNode _rootNodeInstance;
        private BehaviourTreeContextRegistry _contextRegistry;

        [SerializeField]
        private BehaviourTreeAsset _treeAsset;

        private void Start()
        {
            if (_treeAsset == null)
            {
                Debug.LogWarning($"{name} has no BehaviourTreeAsset assigned.");
                enabled = false;
                return;
            }

            BuildContextRegistry();
            _rootNodeInstance = _treeAsset.CloneTree();

            BehaviourTreeActionRequestDispatcher dispatcher = GetComponent<BehaviourTreeActionRequestDispatcher>();
            if (dispatcher == null)
            {
                Debug.LogWarning($"No action request dispatcher found on {gameObject.name}");
            }
            else
            {
                InjectDispatcherToActions(_rootNodeInstance, dispatcher);
            }
        }

        private void Update()
        {
            if (_rootNodeInstance == null || _contextRegistry?.Count == 0)
            {
                return;
            }

            _rootNodeInstance.Tick(_contextRegistry);
        }

        private void BuildContextRegistry()
        {
            _contextRegistry = new BehaviourTreeContextRegistry();

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
            _rootNodeInstance?.ResetNode();
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
    }
}