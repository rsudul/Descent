using Descent.Gameplay.Systems.WeaponSystem.Core;
using System;
using UnityEngine;

namespace Descent.Gameplay.Player.Combat
{
    public class PlayerWeaponSystemController : WeaponSystemController
    {
        private void Start()
        {
            OnFired += (object sender, EventArgs args) => Debug.Log("Player fired weapon");
            OnReloaded += (object sender, EventArgs args) => Debug.Log("Player reloaded");
        }
    }
}