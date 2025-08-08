using UnityEngine;
using Descent.AI.BehaviourTree.Context;

namespace Descent.AI.BehaviourTree.Services
{
    public interface IBehaviourTreeService
    {
        float Interval { get; }
        void OnStart(BehaviourTreeContextRegistry contextRegistry, GameObject owner);
        void Tick(BehaviourTreeContextRegistry contextRegistry, float deltaTime);
        void OnStop();
    }
}