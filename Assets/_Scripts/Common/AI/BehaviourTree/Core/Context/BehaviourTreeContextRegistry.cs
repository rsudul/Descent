using System;
using System.Collections.Generic;

namespace Descent.Common.AI.BehaviourTree.Core.Context
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
            if (!_contexts.TryGetValue(contextType, out var context))
            {
                return null;
            }

            return context;
        }
    }
}