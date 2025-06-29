using Descent.Common.AI.BehaviourTree.Core;
using Descent.Common.AI.BehaviourTree.Core.Context;
using Descent.Gameplay.Player;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Conditions
{
    [System.Serializable]
    public class IsPlayerVisibleCondition : IBehaviourTreeCondition
    {
        [SerializeField]
        private float _viewDistance = 10.0f;
        [SerializeField]
        private float _fieldOfView = 120.0f;

        public IsPlayerVisibleCondition() { }

        public bool Check(BehaviourTreeContext context)
        {
            if (context is not AIMovementContext movementContext)
            {
                return false;
            }

            if (movementContext.AgentTransform == null)
            {
                return false;
            }

            GameObject player = null;
            if (player == null)
            {
                return false;
            }

            Vector3 directionToPlayer = player.transform.position - movementContext.AgentTransform.position;
            float distance = directionToPlayer.magnitude;

            if (distance > _viewDistance)
            {
                return false;
            }

            float angle = Vector3.Angle(movementContext.AgentTransform.forward, directionToPlayer);
            if (angle > _fieldOfView * 0.5f)
            {
                return false;
            }

            if (Physics.Raycast(movementContext.AgentTransform.position, directionToPlayer.normalized,
                out RaycastHit hit, _viewDistance))
            {
                if (hit.collider.GetComponent<Player>())
                {
                    return true;
                }
            }

            return false;
        }

        public IBehaviourTreeCondition Clone()
        {
            return new IsPlayerVisibleCondition();
        }
    }
}