using Descent.Common.AI.BehaviourTree.Context;
using Descent.Gameplay.Game;
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

        public bool Check(BehaviourTreeContextRegistry contextRegistry)
        {
            AIMovementContext context = contextRegistry.GetContext(typeof(AIMovementContext)) as AIMovementContext;

            if (context == null)
            {
                return false;
            }

            if (context.AgentTransform == null)
            {
                return false;
            }

            if (!GameController.Instance.GetPlayer(out GameObject player))
            {
                return false;
            }

            Vector3 directionToPlayer = player.transform.position - context.AgentTransform.position;
            float distance = directionToPlayer.magnitude;

            if (distance > _viewDistance)
            {
                return false;
            }

            float angle = Vector3.Angle(context.AgentTransform.forward, directionToPlayer);
            if (angle > _fieldOfView * 0.5f)
            {
                return false;
            }

            if (Physics.Raycast(context.AgentTransform.position, directionToPlayer.normalized,
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
            IsPlayerVisibleCondition clone = new IsPlayerVisibleCondition();
            clone._viewDistance = _viewDistance;
            clone._fieldOfView = _fieldOfView;
            return clone;
        }
    }
}