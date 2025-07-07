using Descent.Common.Collisions.Parameters;
using System;
using UnityEngine;

namespace Descent.Common.Events.Arguments
{
    public class CollisionEventArguments : EventArgs
    {
        public Transform Transform { get; private set; } = null;
        public Vector3 CollisionPoint { get; private set; } = Vector3.zero;
        public Vector3 CollisionNormal { get; private set; } = Vector3.zero;
        public CollisionParameters CollisionParameters { get; private set; } = null;
        public bool IsTrigger { get; private set; } = false;

        public CollisionEventArguments(Transform transform, Vector3 collisionPoint, Vector3 collisionNormal,
            CollisionParameters collisionParameters, bool isTrigger)
        {
            Transform = transform;
            CollisionPoint = collisionPoint;
            CollisionNormal = collisionNormal;
            CollisionParameters = collisionParameters;
            IsTrigger = isTrigger;
        }
    }
}