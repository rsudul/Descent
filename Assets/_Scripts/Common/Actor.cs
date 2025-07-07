using Descent.Gameplay.Systems.Hostility;
using Descent.Gameplay.Systems.Hostility.Data;
using System;
using UnityEngine;

namespace Descent.Common
{
    public abstract class Actor : MonoBehaviour, IFactionMember
    {
        public event EventHandler<Faction> OnChangedFaction;

        protected virtual void Initialize()
        {

        }

        public abstract Faction GetFaction();

        public abstract void SetFaction(Faction faction);

        protected void InvokeFactionChanged(Faction faction)
        {
            OnChangedFaction?.Invoke(this, faction);
        }
    }
}