using UnityEngine;
using Descent.AI.BehaviourTree.Context;
using Descent.Gameplay.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Conditions;
using Descent.Common.Attributes.AI;
using System.Collections.Generic;
using Descent.AI.BehaviourTree.Core;

namespace Descent.Gameplay.AI.BehaviourTree.Conditions
{
    [System.Serializable]
    public class IsAimAlignedCondition : IBehaviourTreeCondition
    {
        [ShowInNodeInspector("Angle threshold")]
        [SerializeField]
        private float _maxAngleDeg = 1.0f;

        public bool Check(BehaviourTreeContextRegistry contextRegistry)
        {
            AIPerceptionContext perceptionContext = contextRegistry.GetContext(typeof(AIPerceptionContext)) as AIPerceptionContext;
            AIRotationContext rotationContext = contextRegistry.GetContext(typeof(AIRotationContext)) as AIRotationContext;
            if (perceptionContext == null || rotationContext == null)
            {
                return false;
            }

            Transform target = perceptionContext.CurrentTarget;
            if (target == null)
            {
                return false;
            }

            Vector3 delta = target.position - perceptionContext.AgentTransform.position;
            float desiredY = Mathf.Atan2(delta.x, delta.z) * Mathf.Rad2Deg;

            float currentY = rotationContext.CurrentYAngle;
            float angleDiff = Mathf.DeltaAngle(currentY, desiredY);

            return Mathf.Abs(angleDiff) <= _maxAngleDeg;
        }

        public void ResetCondition()
        {

        }

        public IBehaviourTreeCondition Clone()
        {
            IsAimAlignedCondition clone = new IsAimAlignedCondition();
            clone._maxAngleDeg = _maxAngleDeg;
            return clone;
        }

        public IEnumerable<ValuePinDefinition> GetRequiredPins()
        {
            yield break;
        }
    }
}