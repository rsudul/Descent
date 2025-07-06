using Descent.Common.AI.BehaviourTree.Actions.Movement;
using System.Collections.Generic;
using UnityEngine;
using Descent.Common.AI.BehaviourTree.Nodes;

namespace Descent.Common.AI.BehaviourTree.Editor
{
    public static class BehaviourTreeNodeRegistry
    {
        public static readonly List<NodeCreationMenuItem> NodeTypes = new()
        {
            new NodeCreationMenuItem("Composite/Selector", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeSelectorNode>();
                node.Name = "Selector";
                return node;
            }),
            new NodeCreationMenuItem("Composite/Sequence", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeSequenceNode>();
                node.Name = "Sequence";
                return node;
            }),
            new NodeCreationMenuItem("Composite/Repeat Until Failure", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeRepeatUntilFailureNode>();
                node.Name = "Repeat Until Failure";
                return node;
            }),
            new NodeCreationMenuItem("Composite/Repeat While Condition", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeRepeatWhileConditionNode>();
                node.Name = "Repeat While Condition";
                return node;
            }),
            new NodeCreationMenuItem("Action/Set Movement Target", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeActionNode>();
                node.Name = "Set Movement Target";
                node.Action = new SetMovementTargetAction(Vector3.zero);
                return node;
            }),
            new NodeCreationMenuItem("Action/Move To Target", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeActionNode>();
                node.Name = "Move To Target";
                node.Action = new MoveToTargetAction();
                return node;
            }),
            new NodeCreationMenuItem("Action/Patrol", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeActionNode>();
                node.Name = "Patrol";
                node.Action = new PatrolAction();
                return node;
            })
        };
    }
}