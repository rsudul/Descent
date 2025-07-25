using UnityEngine;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.Common.Attributes.AI;
using Descent.AI.BehaviourTree.Requests;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    /// <summary>
    /// ---Cooldown---
    /// 
    /// Action node that enforces a cooldown period after a shoot or action.
    /// Returns Running until the cooldown has elapsed, then returns Success.
    /// 
    /// Uses contexts:
    /// </summary>
    public class CooldownAction : IBehaviourTreeAction
    {
        private float _startTime = 0.0f;
        private bool _started = false;

        [ShowInNodeInspector("Cooldown time")]
        private float _cooldownTime = 0.0f;

        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            if (!_started)
            {
                _startTime = Time.time;
                _started = true;
            }

            if (Time.time - _startTime < _cooldownTime)
            {
                return BehaviourTreeStatus.Running;
            }

            return BehaviourTreeStatus.Success;
        }

        public IBehaviourTreeAction Clone()
        {
            CooldownAction clone = new CooldownAction();
            clone._cooldownTime = _cooldownTime;
            return clone;
        }

        public void ResetAction()
        {
            _started = false;
        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {

        }
    }
}