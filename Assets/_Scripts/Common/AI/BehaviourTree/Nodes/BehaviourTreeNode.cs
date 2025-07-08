using UnityEngine;
using Descent.Common.AI.BehaviourTree.Core;
using Descent.Common.AI.BehaviourTree.Context;
using Descent.Common.Attributes.AI;
using System;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    [System.Serializable]
    public abstract class BehaviourTreeNode : ScriptableObject
    {
        [SerializeField]
        [ShowInNodeInspector("Name", 1000)]
        [NodeNameField]
        protected string _name = "Node";

        [SerializeField]
        public BehaviourTreeNode Parent { get; set; }
        [SerializeField]
        public Vector2 Position { get; set; }
        public BehaviourTreeStatus Status { get; protected set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged?.Invoke(this, nameof(Name));
            }
        }

        public event EventHandler<string> OnPropertyChanged;

        public abstract BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry);

        public abstract void ResetNode();
    }
}