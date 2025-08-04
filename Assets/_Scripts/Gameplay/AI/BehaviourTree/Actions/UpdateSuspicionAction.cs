using UnityEngine;
using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;
using Descent.Common.Attributes.AI;

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

        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            float currentSuspicion = contextRegistry.Blackboard.Get<float>("SuspicionLevel", 0.0f);
            bool hasTarget = contextRegistry.Blackboard.Get<bool>("HasActiveTarget", false);

            if (hasTarget)
            {
                currentSuspicion += _increaseRate * Time.deltaTime;
                currentSuspicion = Mathf.Clamp01(currentSuspicion);
            } else if (_useDecay)
            {
                float decayRate = contextRegistry.Blackboard.Get<float>("SuspicionDecayRate", 0.5f);
                currentSuspicion -= decayRate * Time.deltaTime;
                currentSuspicion = Mathf.Max(0.0f, currentSuspicion);
            }

            contextRegistry.Blackboard.Set("SuspicionLevel", currentSuspicion);
            return BehaviourTreeStatus.Success;
        }

        public void ResetAction()
        {

        }

        public IBehaviourTreeAction Clone()
        {
            UpdateSuspicionAction clone = new UpdateSuspicionAction();
            clone._increaseRate = _increaseRate;
            clone._useDecay = _useDecay;
            return clone;
        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {

        }
    }
}