using Descent.Gameplay.AI.BehaviourTree.Actions.Movement;
using System.Collections.Generic;
using UnityEngine;
using Descent.AI.BehaviourTree.Nodes;
using Descent.Gameplay.AI.BehaviourTree.Actions;
using Descent.Gameplay.AI.BehaviourTree.Conditions;

namespace Descent.AI.BehaviourTree.Editor
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
            new NodeCreationMenuItem("Composite/Priority Reactive Selector", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreePriorityReactiveSelector>();
                node.Name = "Priority Reactive Selector";
                return node;
            }),
            new NodeCreationMenuItem("Composite/Sequence", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeSequenceNode>();
                node.Name = "Sequence";
                return node;
            }),
            new NodeCreationMenuItem("Composite/Repeat Forever", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeRepeatForeverNode>();
                node.Name = "Repeat Forever";
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
            }),
            new NodeCreationMenuItem("Action/Rotate To Target", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeActionNode>();
                node.Name = "Rotate To Target";
                node.Action = new RotateToTargetAction();
                return node;
            }),
            new NodeCreationMenuItem("Action/Stop Rotation", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeActionNode>();
                node.Name = "Stop Rotation";
                node.Action = new StopRotationAction();
                return node;
            }),
            new NodeCreationMenuItem("Action/Scan Area", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeActionNode>();
                node.Name = "Scan Area";
                node.Action = new ScanAreaAction();
                return node;
            }),
            new NodeCreationMenuItem("Condition/Compare Blackboard", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeCompareNode>();
                node.Name = "Compare";
                return node;
            }),
            new NodeCreationMenuItem("Condition/Is Hostile Target Visible", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeConditionNode>();
                node.Name = "Is Hostile Target Visible";
                node.Condition = new IsHostileTargetVisibleCondition();
                return node;
            })
        };
    }
}