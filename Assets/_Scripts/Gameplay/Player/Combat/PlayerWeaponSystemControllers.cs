using Descent.Gameplay.Systems.WeaponSystem;
using Descent.Gameplay.Systems.WeaponSystem.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Descent.Gameplay.Player.Combat
{
    public class PlayerWeaponSystemController : WeaponSystemController
    {
        private List<Weapon> _playerWeapons;

        public IReadOnlyList<Weapon> PlayerWeapons => _playerWeapons;

        public void Initialize(WeaponsConfig weaponsConfig)
        {
            _playerWeapons = new List<Weapon>();
            foreach (WeaponData weaponData in weaponsConfig.StartingWeapons)
            {
                Weapon weapon = new Weapon(weaponData, null);
                _playerWeapons.Add(weapon);
            }

            OnFired += (object sender, EventArgs args) => Debug.Log("Player fired weapon");
            OnReloaded += (object sender, EventArgs args) => Debug.Log("Player reloaded");
        }
    }
}