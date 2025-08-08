using System.Collections.Generic;
using UnityEngine;
using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.Common.Attributes.AI;
using Descent.AI.BehaviourTree.Requests;
using Descent.Gameplay.AI.BehaviourTree.Context;
using Descent.Gameplay.AI.BehaviourTree.Actions.Data;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    [System.Serializable]
    public class UpdateSuspicionAction : IBehaviourTreeAction
    {
        private BehaviourTreeActionRequestDispatcher _dispatcher = null;

        [ShowInNodeInspector("Increase rate")]
        [SerializeField]
        private float _increaseRate = 1.0f;

        [ShowInNodeInspector("Decay rate")]
        [SerializeField]
        private float _decayRate = 0.2f;

        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            if (_dispatcher == null)
            {
                return BehaviourTreeStatus.Failure;
            }

            AIPerceptionContext perceptionContext = contextRegistry.GetContext(typeof(AIPerceptionContext)) as AIPerceptionContext;

            if (perceptionContext == null)
            {
                return BehaviourTreeStatus.Failure;
            }

            bool hasTarget = perceptionContext.CurrentTarget != null;

            UpdateSuspicionActionData data = new UpdateSuspicionActionData(_increaseRate, _decayRate, hasTarget);

            BehaviourTreeRequestResult result = _dispatcher.RequestAction(BehaviourTreeActionType.UpdateSuspicion, data);

            return result == BehaviourTreeRequestResult.Success ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
        }

        public IBehaviourTreeAction Clone()
        {
            UpdateSuspicionAction clone = new UpdateSuspicionAction();
            clone._increaseRate = _increaseRate;
            clone._decayRate = _decayRate;
            return clone;
        }

        public void ResetAction()
        {

        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
    }
}