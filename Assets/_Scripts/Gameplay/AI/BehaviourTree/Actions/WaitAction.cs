using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;
using Descent.Common.Attributes.AI;
using UnityEngine;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    /// <summary>
    /// ---Wait---
    /// 
    /// Action node which returns Running during specified time,
    /// after that returns Success.
    /// </summary>
    public class WaitAction : IBehaviourTreeAction
    {
        private float _startTime = 0.0f;
        private bool _started = false;

        [ShowInNodeInspector("Wait time")]
        [SerializeField]
        private float _waitTime = 0.0f;

        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            if (!_started)
            {
                _startTime = Time.time;
                _started = true;
            }

            if (Time.time - _startTime < _waitTime)
            {
                return BehaviourTreeStatus.Running;
            }

            return BehaviourTreeStatus.Success;
        }

        public IBehaviourTreeAction Clone()
        {
            WaitAction clone = new WaitAction();
            clone._waitTime = _waitTime;
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