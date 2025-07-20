using Descent.Gameplay.Entities;
using Descent.Gameplay.Events.Arguments;
using Descent.Gameplay.Systems.Durability.Health;
using Descent.Gameplay.Systems.Perception;
using System;
using System.Collections.Generic;

namespace Descent.Gameplay.Enemies
{
    public abstract class Enemy : Actor, IHealthController, IPerceptionController
    {
        public abstract IReadOnlyCollection<Actor> VisibleActors { get; }

        public virtual float Health => 0.0f;
        public virtual float MaxHealth => 0.0f;
        public virtual bool IsAlive => Health > 0.0f;

        public virtual event EventHandler<HealthChangedEventArgs> OnHealthChanged { add { } remove { } }
        public virtual event EventHandler OnDied { add { } remove { } }

        public virtual void Heal(float amount) { }
        public virtual void RestoreToFullHealth() { }
    }
}