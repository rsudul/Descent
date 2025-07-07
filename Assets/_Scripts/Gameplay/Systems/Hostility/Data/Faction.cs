using System.Collections.Generic;
using UnityEngine;

namespace Descent.Gameplay.Systems.Hostility.Data
{
    [CreateAssetMenu(menuName = "Descent/Game/Faction", fileName = "Faction")]
    public class Faction : ScriptableObject
    {
        public string factionName = string.Empty;

        [Header("Relations")]
        public List<FactionRelation> Relations = new List<FactionRelation>();

        public bool IsHostileTo(Faction other)
        {
            foreach (FactionRelation relation in Relations)
            {
                if (relation.targetFaction == other)
                {
                    return relation.relation == HostilityType.Enemy;
                }
            }

            return false;
        }
    }
}