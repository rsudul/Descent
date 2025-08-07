using System.Collections.Generic;
using Descent.AI.BehaviourTree.Conditions;
using Descent.AI.BehaviourTree.Context;
using Descent.Gameplay.AI.BehaviourTree.Context;
using Descent.Gameplay.Entities;
using Descent.Gameplay.Systems.Hostility;

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

            if (perceptionContext?.VisibleActors == null
                || factionContext?.Faction == null)
            {
                return false;
            }

            IReadOnlyList<Actor> visibleActors = perceptionContext.VisibleActors;
            if (visibleActors.Count == 0)
            {
                return false;
            }

            Faction myFaction = factionContext.Faction;

            foreach (Actor actor in perceptionContext.VisibleActors)
            {
                if (actor == null)
                {
                    continue;
                }

                if (!actor.TryGetComponent<IFactionMember>(out IFactionMember member))
                {
                    continue;
                }

                Faction otherFaction = member.GetFaction();

                if (otherFaction != null && myFaction.IsHostileTo(otherFaction))
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