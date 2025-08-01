using UnityEngine;
using Descent.AI.BehaviourTree.Nodes;
using Descent.AI.BehaviourTree.Variables;

namespace Descent.AI.BehaviourTree.Core
{
    [CreateAssetMenu(menuName = "Descent/AI/BehaviourTree")]
    public class BehaviourTreeAsset : ScriptableObject
    {
        [SerializeField]
        public BehaviourTreeNode Root;

        [SerializeField]
        private VariableContainer _variableContainer = new VariableContainer();
        
        public VariableContainer VariableContainer => _variableContainer;

        public BehaviourTreeNode CloneTree()
        {
            return Root.CloneNode();
        }

        private void OnEnable()
        {
            if (_variableContainer == null)
            {
                _variableContainer = new VariableContainer();
            }
        }

        private void OnValidate()
        {
            if (_variableContainer == null)
            {
                _variableContainer = new VariableContainer();
            }
        }
    }
}