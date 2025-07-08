using System;

namespace Descent.Gameplay.Systems.Hostility
{
    public interface IFactionMember
    {
        event EventHandler<Faction> OnChangedFaction;
        Faction GetFaction();
        void SetFaction(Faction faction);
    }
}