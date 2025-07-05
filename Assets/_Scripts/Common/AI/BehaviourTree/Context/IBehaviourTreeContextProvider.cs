using System;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Context
{
    public interface IBehaviourTreeContextProvider
    {
        BehaviourTreeContext GetBehaviourTreeContext(Type contextType, GameObject agent);
    }
}