using UnityEngine;
using Descent.Common.AI.BehaviourTree.Core.Context;
using Descent.Common.AI.BehaviourTree.Nodes;
using Descent.Gameplay.AI.Movement;

namespace Descent.Common.AI.BehaviourTree.Core
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        private BehaviourTreeNode _rootNodeInstance;
        private BehaviourTreeContext _context;

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

            _context = new AIMovementContext(gameObject, GetComponent<AIMovementController>());

            _rootNodeInstance = _treeAsset.CloneTree();
        }

        private void Update()
        {
            if (_rootNodeInstance == null || _context == null)
            {
                return;
            }

            _rootNodeInstance.Tick(_context);
        }
    }
}