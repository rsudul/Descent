using UnityEngine;
using Descent.Common.AI.BehaviourTree.Core.Context;
using Descent.Common.AI.BehaviourTree.Nodes;
using Descent.Gameplay.AI.Movement;
using Descent.Attributes.AI;

namespace Descent.Common.AI.BehaviourTree.Core
{
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
    }
}