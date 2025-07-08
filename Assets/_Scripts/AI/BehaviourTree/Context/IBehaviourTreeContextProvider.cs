using System;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Context
{
    public interface IBehaviourTreeContextProvider
    {
        BehaviourTreeContext GetBehaviourTreeContext(Type contextType, GameObject agent);
    }
}