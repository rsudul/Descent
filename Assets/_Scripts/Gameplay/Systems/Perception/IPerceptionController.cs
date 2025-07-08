using System.Collections.Generic;
using Descent.Gameplay.Entities;

namespace Descent.Gameplay.Systems.Perception
{
    public interface IPerceptionController
    {
        IReadOnlyCollection<Actor> VisibleActors { get; }
    }
}