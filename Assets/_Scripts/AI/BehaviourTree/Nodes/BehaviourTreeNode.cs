using UnityEngine;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Context;
using Descent.Common.Attributes.AI;
using System;

namespace Descent.AI.BehaviourTree.Nodes
{
    [Serializable]
    public abstract class BehaviourTreeNode : ScriptableObject
    {
        [SerializeField]
        protected BehaviourTreeNode _parent = null;
        [SerializeField]
        protected Vector2 _position = Vector2.zero;

        [SerializeField]
        [ShowInNodeInspector("Name", 1000)]
        [NodeNameField]
        protected string _name = "Node";

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

        public Vector2 Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged?.Invoke(this, nameof(Position));
                }
            }
        }

        public BehaviourTreeNode Parent
        {
            get => _parent;
            set
            {
                if (_parent != value)
                {
                    _parent = value;
                    OnPropertyChanged?.Invoke(this, nameof(Parent));
                }
            }
        }

        public event EventHandler<string> OnPropertyChanged;

        public abstract BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry);

        public abstract void ResetNode();

        public abstract BehaviourTreeNode CloneNode();
    }
}