using Descent.Gameplay.Systems.WeaponSystem.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Descent.Gameplay.Systems.WeaponSystem
{
    [CreateAssetMenu(fileName = "WeaponsConfig", menuName = "Descent/Weapons/Weapons Config")]
    public class WeaponsConfig : ScriptableObject
    {
        [field: SerializeField] public List<WeaponData> StartingWeapons { get; private set; }
    }
}