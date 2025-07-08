using Descent.Gameplay.Systems.Hostility.Data;
using Descent.AI.BehaviourTree.Context;
using UnityEngine;

namespace Descent.Gameplay.AI.BehaviourTree.Context
{
    public class AIFactionContext : BehaviourTreeContext
    {
        private Faction _faction;
        public Faction Faction => _faction;

        public AIFactionContext(GameObject owner, Faction faction) : base(owner)
        {
            _faction = faction;
        }
    }
}