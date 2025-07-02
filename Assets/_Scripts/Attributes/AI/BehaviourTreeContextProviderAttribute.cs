using System;

namespace Descent.Attributes.AI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class BehaviourTreeContextProviderAttribute : Attribute
    {
        public Type ContextType { get; }

        public BehaviourTreeContextProviderAttribute(Type contextType)
        {
            ContextType = contextType;
        }
    }
}