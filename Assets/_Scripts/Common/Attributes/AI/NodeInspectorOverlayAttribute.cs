using Descent.AI.BehaviourTree.Core;
using System;

namespace Descent.Common.Attributes.AI
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class NodeInspectorOverlayAttribute : Attribute
    {
        public NodeInspectorOverlayType OverlayType { get; }

        public NodeInspectorOverlayAttribute(NodeInspectorOverlayType overlayType)
        {
            OverlayType = overlayType;
        }
    }
}