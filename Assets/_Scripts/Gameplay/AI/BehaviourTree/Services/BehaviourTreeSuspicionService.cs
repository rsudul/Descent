using UnityEngine;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Services;
using Descent.Gameplay.AI.BehaviourTree.Context;
using Descent.Gameplay.Systems.Alert;

namespace Descent.Gameplay.AI.BehaviourTree.Services
{
    [DisallowMultipleComponent]
    public class BehaviourTreeSuspicionService : MonoBehaviour, IBehaviourTreeService
    {
        private ISuspicionController _suspicionController;

        [Header("Interval")]
        [SerializeField]
        private float _interval = 0.1f;

        [Header("Rates")]
        [SerializeField]
        private float _increaseRate = 0.6f;
        [SerializeField]
        private float _decayRate = 0.25f;

        [Header("Debug sync to BB")]
        [SerializeField]
        private bool _writeToBlackboard = true;
        [SerializeField]
        private string _blackboardHasTargetKey = "HasActiveTarget";
        [SerializeField]
        private string _blackboardSuspicionKey = "Suspicion";

        public float Interval => _interval <= 0.0f ? 0.1f : _interval;

        public void OnStart(BehaviourTreeContextRegistry contextRegistry, GameObject owner)
        {
            _suspicionController = owner.GetComponent<ISuspicionController>();
            if (_suspicionController == null)
            {
                Debug.LogWarning("[BehaviourTreeSuspicionService] No ISuspicionController on owner. Service will be idle.");
            }
        }

        public void Tick(BehaviourTreeContextRegistry contextRegistry, float deltaTime)
        {
            if (_suspicionController == null)
            {
                return;
            }

            AIPerceptionContext perceptionContext = contextRegistry.GetContext(typeof(AIPerceptionContext)) as AIPerceptionContext;
            bool hasTarget = perceptionContext?.CurrentTarget != null;

            _suspicionController.UpdateSuspicion(_increaseRate, _decayRate, hasTarget, deltaTime);

            if (_writeToBlackboard)
            {
                contextRegistry.Blackboard.Set(_blackboardHasTargetKey, hasTarget);
                contextRegistry.Blackboard.Set(_blackboardSuspicionKey, _suspicionController.SuspicionLevel);
            }
        }

        public void OnStop()
        {

        }
    }
}