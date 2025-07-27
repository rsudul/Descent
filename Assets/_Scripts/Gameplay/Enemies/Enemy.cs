using Descent.Gameplay.Entities;
using Descent.Gameplay.Events.Arguments;
using Descent.Gameplay.Systems.Durability.Health;
using Descent.Gameplay.Systems.Perception;
using Descent.Gameplay.Systems.WeaponSystem.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Descent.Gameplay.Enemies
{
    public abstract class Enemy : Actor, IHealthController, IPerceptionController, IWeaponOwner
    {
        public abstract IReadOnlyCollection<Actor> VisibleActors { get; }

        public GameObject GameObject => gameObject;
        public abstract Transform WeaponMountPoint { get; }

        public virtual float Health => 0.0f;
        public virtual float MaxHealth => 0.0f;
        public virtual bool IsAlive => Health > 0.0f;

        public virtual event EventHandler<HealthChangedEventArgs> OnHealthChanged { add { } remove { } }
        public virtual event EventHandler OnDied { add { } remove { } }

        public virtual void Heal(float amount) { }
        public virtual void RestoreToFullHealth() { }
    }
}