using Descent.Common.AI.BehaviourTree.Core.Context;
using System;
using UnityEngine;

public interface IBehaviourTreeContextProvider
{
    BehaviourTreeContext GetBehaviourTreeContext(Type contextType, GameObject agent);
}