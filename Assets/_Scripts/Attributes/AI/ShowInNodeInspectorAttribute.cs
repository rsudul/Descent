using System;

namespace Descent.Attributes.AI
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ShowInNodeInspectorAttribute : Attribute
    {
        public string Label { get; }
        public int Priority { get; }

        public ShowInNodeInspectorAttribute(string label = null, int priority = 0)
        {
            Label = label;
            Priority = priority;
        }
    }
}