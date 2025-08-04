using System;
using UnityEngine;
using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Context;
using Descent.Common.Attributes.AI;
using System.Collections.Generic;

namespace Descent.AI.BehaviourTree.Nodes
{
    [Serializable]
    [NodeInspectorLabel("Action")]
    public class BehaviourTreeActionNode : BehaviourTreeNode
    {
        [SerializeReference]
        [ShowInNodeInspector("Action")]
        public IBehaviourTreeAction Action;

        public override IEnumerable<ValuePinDefinition> ValuePins
        {
            get
            {
                if (Action is IBehaviourTreeActionWithPins actionWithPins)
                {
                    return actionWithPins.GetRequiredPins();
                }

                return base.ValuePins;
            }
        }

        public BehaviourTreeActionNode()
        {
            if (Action is BehaviourTreeActionWithPins actionWithPins)
            {
                actionWithPins.SetNodeGuid(GUID);
            }
        }

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Action == null)
            {
                Debug.LogError($"[{GetType().Name}]: Action is null.");
                return BehaviourTreeStatus.Failure;
            }

            BehaviourTreeStatus status = Action?.Execute(contextRegistry) ?? BehaviourTreeStatus.Failure;
            Status = status;
            return status;
        }

        public override void ResetNode()
        {
            Action?.ResetAction();
        }

        public override BehaviourTreeNode CloneNode()
        {
            BehaviourTreeActionNode clone = ScriptableObject.CreateInstance<BehaviourTreeActionNode>();
            clone.Name = Name;
            clone.Position = Position;
            clone.Action = Action?.Clone();
            return clone;
        }
    }
}