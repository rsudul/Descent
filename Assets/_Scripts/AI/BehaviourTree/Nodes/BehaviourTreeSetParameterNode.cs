using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Nodes
{
    public class BehaviourTreeSetParameterNode<T> : BehaviourTreeNode
    {
        [SerializeField]
        private string _key;
        [SerializeField]
        private T _value;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            contextRegistry.Blackboard.Set(_key, _value);
            Status = BehaviourTreeStatus.Success;
            return Status;
        }

        public override void ResetNode()
        {
            Status = BehaviourTreeStatus.Running;
        }

        public override BehaviourTreeNode CloneNode()
        {
            BehaviourTreeSetParameterNode<T> clone = ScriptableObject.CreateInstance<BehaviourTreeSetParameterNode<T>>();
            clone.Name = Name;
            clone.Position = Position;
            clone._key = _key;
            clone._value = _value;
            return clone;
        }
    }
}