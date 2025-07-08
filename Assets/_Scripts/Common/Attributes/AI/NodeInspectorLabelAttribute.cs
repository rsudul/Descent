using System;

namespace Descent.Common.Attributes.AI
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class NodeInspectorLabelAttribute : Attribute
    {
        public string Label { get; }

        public NodeInspectorLabelAttribute(string label)
        {
            Label = label;
        }
    }
}