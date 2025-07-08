using Descent.AI.BehaviourTree.Core;
using System;

namespace Descent.AI.BehaviourTree.Events.Arguments
{
    public class TickEventArgs : EventArgs
    {
        public float DeltaTime { get; }
        public BehaviourTreeStatus RootStatus { get; }

        public TickEventArgs(float deltaTime, BehaviourTreeStatus rootStatus)
        {
            DeltaTime = deltaTime;
            RootStatus = rootStatus;
        }
    }
}