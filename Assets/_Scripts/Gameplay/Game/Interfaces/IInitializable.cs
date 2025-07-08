using System;

namespace Descent.Gameplay.Game.Interfaces
{
    public interface IInitializable
    {
        // the higher the priority, the sooner the obejct will be initialized (before objects with lower priority)
        int InitializationPriority { get; }

        event EventHandler OnBeforeInitialize;
        event EventHandler OnAfterInitialize;

        void Initialize();
        void LateInitialize();
    }
}