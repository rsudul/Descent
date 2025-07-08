using Descent.Gameplay.Game.Initialization;
using Descent.Gameplay.Systems.Hostility;
using Descent.Gameplay.Systems.Hostility.Data;
using System;

namespace Descent.Gameplay.Entities
{
    public abstract class Actor : Entity, IFactionMember, IInitializable
    {
        public int InitializationPriority { get; protected set; }

        public event EventHandler OnBeforeInitialize;
        public event EventHandler OnAfterInitialize;

        public event EventHandler<Faction> OnChangedFaction;

        public virtual void Initialize()
        {

        }

        public virtual void LateInitialize()
        {

        }

        protected void InvokeOnBeforeInitialize()
        {
            OnBeforeInitialize?.Invoke(this, EventArgs.Empty);
        }

        protected void InvokeOnAfterInitialize()
        {
            OnAfterInitialize?.Invoke(this, EventArgs.Empty);
        }

        public abstract Faction GetFaction();

        public abstract void SetFaction(Faction faction);

        protected void InvokeFactionChanged(Faction faction)
        {
            OnChangedFaction?.Invoke(this, faction);
        }
    }
}