using System;

namespace Descent.Attributes.AI
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ConditionInvertFieldAttribute : Attribute
    {
        public ConditionInvertFieldAttribute() { }
    }
}