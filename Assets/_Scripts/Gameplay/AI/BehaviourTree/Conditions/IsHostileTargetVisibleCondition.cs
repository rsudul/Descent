using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Conditions;
using Descent.Gameplay.AI.BehaviourTree.Context;
using Descent.Gameplay.Systems.Hostility;
using Descent.Gameplay.Entities;
using UnityEngine;

namespace Descent.Gameplay.AI.BehaviourTree.Conditions
{
    [System.Serializable]
    public class IsHostileTargetVisibleCondition : IBehaviourTreeCondition
    {
        public IsHostileTargetVisibleCondition() { }

        public bool Check(BehaviourTreeContextRegistry contextRegistry)
        {
            AIPerceptionContext perceptionContext = contextRegistry.
                            GetContext(typeof(AIPerceptionContext)) as AIPerceptionContext;

            AIFactionContext factionContext = contextRegistry.GetContext(typeof(AIFactionContext)) as AIFactionContext;

            if (perceptionContext == null || factionContext == null)
            {
                return false;
            }

            Faction faction = factionContext.Faction;

            if (faction == null)
            {
                return false;
            }

            foreach (Actor actor in perceptionContext.PerceptionController.VisibleActors)
            {
                if (!actor.TryGetComponent<IFactionMember>(out IFactionMember member))
                {
                    continue;
                }

                if (member.GetFaction().IsHostileTo(faction))
                {
                    return true;
                }
            }

            return false;
        }

        public IBehaviourTreeCondition Clone()
        {
            IsHostileTargetVisibleCondition clone = new IsHostileTargetVisibleCondition();
            return clone;
        }

        public void ResetCondition()
        {

        }
    }
}