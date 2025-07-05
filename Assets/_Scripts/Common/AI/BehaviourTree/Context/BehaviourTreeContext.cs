using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Context
{
    public class BehaviourTreeContext
    {
        public GameObject Agent { get; }
        public Transform AgentTransform => Agent.transform;

        public BehaviourTreeContext(GameObject owner)
        {
            Agent = owner;
        }
    }
}