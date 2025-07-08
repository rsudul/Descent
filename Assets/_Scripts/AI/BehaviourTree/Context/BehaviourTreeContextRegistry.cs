using System;
using System.Collections.Generic;

namespace Descent.AI.BehaviourTree.Context
{
    public class BehaviourTreeContextRegistry
    {
        private readonly Dictionary<Type, BehaviourTreeContext> _contexts = new Dictionary<Type, BehaviourTreeContext>();

        public int Count => _contexts.Count;

        public void RegisterContext(Type contextType, BehaviourTreeContext context)
        {
            _contexts[contextType] = context;
        }

        public BehaviourTreeContext GetContext(Type contextType)
        {
            if (!_contexts.TryGetValue(contextType, out BehaviourTreeContext context))
            {
                return null;
            }

            return context;
        }
    }
}