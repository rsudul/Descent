using System.Collections.Generic;
using Descent.Common;

namespace Descent.Gameplay.Systems.Perception
{
    public interface IPerceptionController
    {
        IReadOnlyCollection<Actor> VisibleActors { get; }
    }
}