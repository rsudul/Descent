using System;

namespace Descent.Common.Attributes.AI
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ConditionInvertFieldAttribute : Attribute
    {
        public ConditionInvertFieldAttribute() { }
    }
}