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
            new NodeCreationMenuItem("Composite/Random Selector", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeRandomSelectorNode>();
                node.Name = "Random Selector";
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
            new NodeCreationMenuItem("Composite/Repeat N Times", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeRepeatNTimesNode>();
                node.Name = "Repeat N Times";
                return node;
            }),
            new NodeCreationMenuItem("Composite/Parallel", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeParallelNode>();
                node.Name = "Parallel";
                return node;
            }),
            new NodeCreationMenuItem("Composite/Guard", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeGuardNode>();
                node.Name = "Guard";
                return node;
            }),
            new NodeCreationMenuItem("Composite/Interrupt", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeInterruptNode>();
                node.Name = "Interrupt";
                return node;
            }),
            new NodeCreationMenuItem("Composite/Timeout", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeTimeoutNode>();
                node.Name = "Timeout";
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
            new NodeCreationMenuItem("Action/Fetch Target", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeActionNode>();
                node.Name = "Fetch Target";
                node.Action = new FetchTargetAction();
                return node;
            }),
            new NodeCreationMenuItem("Action/Predictive Aim", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeActionNode>();
                node.Name = "Predictive Aim";
                node.Action = new PredictiveAimAction();
                return node;
            }),
            new NodeCreationMenuItem("Action/Wait", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeActionNode>();
                node.Name = "Wait";
                node.Action = new WaitAction();
                return node;
            }),
            new NodeCreationMenuItem("Action/Shoot", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeActionNode>();
                node.Name = "Shoot";
                node.Action = new ShootAction();
                return node;
            }),
            new NodeCreationMenuItem("Action/Move To Last Seen Position", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeActionNode>();
                node.Name = "Move To Last Seen Position";
                node.Action = new MoveToLastSeenPositionAction();
                return node;
            }),
            new NodeCreationMenuItem("Action/Cooldown", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeActionNode>();
                node.Name = "Cooldown";
                node.Action = new CooldownAction();
                return node;
            }),
            new NodeCreationMenuItem("Action/Set Alert Level", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeActionNode>();
                node.Name = "Set Alert Level";
                node.Action = new SetAlertLevelAction();
                return node;
            }),
            new NodeCreationMenuItem("Action/Update Suspicion", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeActionNode>();
                node.Name = "Update Suspicion";
                node.Action = new UpdateSuspicionAction();
                return node;
            }),
            new NodeCreationMenuItem("Action/Update Timers", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeActionNode>();
                node.Name = "Update Timers";
                node.Action = new UpdateTimersAction();
                return node;
            }),
            new NodeCreationMenuItem("Action/Store Last Known Position", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeActionNode>();
                node.Name = "Store Last Known Position";
                node.Action = new StoreLastKnownPositionAction();
                return node;
            }),
            new NodeCreationMenuItem("Action/Always Fail", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeActionNode>();
                node.Name = "Always Fail";
                node.Action = new AlwaysFailAction();
                return node;
            }),
            new NodeCreationMenuItem("Condition/Always True", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeConditionNode>();
                node.Name = "Always True";
                node.Condition = new AlwaysTrueCondition();
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
            }),
            new NodeCreationMenuItem("Condition/Is Aim Aligned", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeConditionNode>();
                node.Name = "Is Aim Aligned";
                node.Condition = new IsAimAlignedCondition();
                return node;
            }),
            new NodeCreationMenuItem("Condition/Alert Level", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeConditionNode>();
                node.Name = "Alert Level";
                node.Condition = new AlertLevelCondition();
                return node;
            }),
            new NodeCreationMenuItem("Condition/Suspicion Level", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeConditionNode>();
                node.Name = "Suspicion Level";
                node.Condition = new SuspicionLevelCondition();
                return node;
            }),
            new NodeCreationMenuItem("Condition/Timer Check", () =>
            {
                var node = ScriptableObject.CreateInstance<BehaviourTreeConditionNode>();
                node.Name = "Timer Check";
                node.Condition = new TimerCondition();
                return node;
            })
        };
    }
}