using UnityEngine;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.Gameplay.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Requests;
using Descent.Common.Attributes.AI;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    /// <summary>
    /// Behaviour Tree action node that fetches the current target from perception and stores
    /// its last known position in the blackboard.
    /// Returns Failure if no target is available.
    /// 
    /// Uses contexts:
    ///  - AIPerceptionContext
    /// </summary>
    public class FetchTargetAction : IBehaviourTreeAction
    {
        [SerializeField]
        [ShowInNodeInspector("Last seen key")]
        private string _lastSeenKey;

        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            if (string.IsNullOrEmpty(_lastSeenKey))
            {
                Debug.LogError("[FetchTargetAction] lastSeenKey is null or empty - cannot write to blackboard.");
                return BehaviourTreeStatus.Failure;
            }

            AIPerceptionContext perceptionContext = contextRegistry.GetContext(typeof(AIPerceptionContext)) as AIPerceptionContext;

            if (perceptionContext == null)
            {
                Debug.Log("[FetchTargetAction] No AIPerceptionContext available.");
                return BehaviourTreeStatus.Failure;
            }

            if (perceptionContext.CurrentTarget == null)
            {
                Debug.Log("[FetchTargetAction] No current target available.");
                return BehaviourTreeStatus.Failure;
            }

            Vector3 lastPosition = perceptionContext.CurrentTarget.position;
            contextRegistry.Blackboard.Set<Vector3>(_lastSeenKey, lastPosition);
            //Debug.Log($"[FetchTargetAction] Target acquired at {lastPosition}.");

            return BehaviourTreeStatus.Success;
        }

        public IBehaviourTreeAction Clone()
        {
            FetchTargetAction clone = new FetchTargetAction();
            clone._lastSeenKey = _lastSeenKey;
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