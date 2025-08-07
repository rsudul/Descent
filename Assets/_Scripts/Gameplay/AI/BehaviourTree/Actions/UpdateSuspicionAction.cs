using System.Collections.Generic;
using UnityEngine;
using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.Common.Attributes.AI;
using Descent.AI.BehaviourTree.Requests;
using Descent.Gameplay.AI.BehaviourTree.Context;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    [System.Serializable]
    public class UpdateSuspicionAction : IBehaviourTreeAction
    {
        [ShowInNodeInspector("Increase rate")]
        [SerializeField]
        private float _increaseRate = 1.0f;

        [ShowInNodeInspector("Use decay")]
        [SerializeField]
        private bool _useDecay = true;

        [ShowInNodeInspector("Decay rate")]
        [SerializeField]
        private float _decayRate = 0.2f;

        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            AIPerceptionContext perceptionContext = contextRegistry.GetContext(typeof(AIPerceptionContext)) as AIPerceptionContext;
            AISuspicionContext suspicionContext = contextRegistry.GetContext(typeof(AISuspicionContext)) as AISuspicionContext;

            if (perceptionContext == null || suspicionContext == null)
            {
                return BehaviourTreeStatus.Failure;
            }

            bool hasTarget = perceptionContext.CurrentTarget != null;
            float currentSuspicion = suspicionContext.SuspicionLevel;

            if (hasTarget)
            {
                currentSuspicion += _increaseRate * Time.deltaTime;
                currentSuspicion = Mathf.Clamp01(currentSuspicion);
            }
            else if (_useDecay)
            {
                currentSuspicion -= _decayRate * Time.deltaTime;
                currentSuspicion = Mathf.Max(0.0f, currentSuspicion);
            }

            contextRegistry.Blackboard.Set("SuspicionLevel", currentSuspicion);

            return BehaviourTreeStatus.Success;
        }

        public IBehaviourTreeAction Clone()
        {
            UpdateSuspicionAction clone = new UpdateSuspicionAction();
            clone._increaseRate = _increaseRate;
            clone._useDecay = _useDecay;
            clone._decayRate = _decayRate;
            return clone;
        }

        public void ResetAction()
        {

        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {

        }
    }
}