using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Context;
using Descent.Common.Attributes.AI;
using Descent.Common.PersistentGUID;

namespace Descent.AI.BehaviourTree.Nodes
{
    [Serializable]
    public abstract class BehaviourTreeNode : ScriptableObject
    {
        [SerializeField]
        protected string _guid;

        [SerializeField]
        protected BehaviourTreeNode _parent = null;
        [SerializeField]
        protected Vector2 _position = Vector2.zero;

        [SerializeField]
        [ShowInNodeInspector("Name", 1000)]
        [NodeNameField]
        protected string _name = "Node";

        public BehaviourTreeStatus Status { get; protected set; }
        public string GUID => _guid;

        public virtual IEnumerable<ValuePinDefinition> ValuePins
        {
            get
            {
                yield break;
            }
        }

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

        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(_guid))
            {
                _guid = UniqueIDGenerator.GenerateUniqueID();
                EditorUtility.SetDirty(this);
            }
#endif
        }

        public abstract BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry);

        public abstract void ResetNode();

        public abstract BehaviourTreeNode CloneNode();

        public T GetInputValue<T>(string pinName, BehaviourTreeContextRegistry contextRegistry)
        {
            if (contextRegistry.TryGetValueConnection(GUID, pinName, out ValueConnection valueConnection))
            {
                BehaviourTreeGetVariableNode source = contextRegistry.GetNodeInstance(valueConnection.SourceNodeGUID)
                    as BehaviourTreeGetVariableNode;
                if (source != null)
                {
                    source.Tick(contextRegistry);
                    return (T)source.CachedValue.GetValue();
                }
            }

            return contextRegistry.Blackboard.Get<T>(pinName);
        }

        public void ForceGenerateGuid()
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(_guid))
            {
                _guid = UniqueIDGenerator.GenerateUniqueID();
                EditorUtility.SetDirty(this);
            }
#endif
        }
    }
}