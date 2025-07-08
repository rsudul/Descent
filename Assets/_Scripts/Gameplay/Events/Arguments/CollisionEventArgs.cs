using Descent.Gameplay.Collisions;
using System;
using UnityEngine;

namespace Descent.Gameplay.Events.Arguments
{
    public class CollisionEventArgs : EventArgs
    {
        public Transform Transform { get; private set; } = null;
        public Vector3 CollisionPoint { get; private set; } = Vector3.zero;
        public Vector3 CollisionNormal { get; private set; } = Vector3.zero;
        public CollisionParameters CollisionParameters { get; private set; } = null;
        public bool IsTrigger { get; private set; } = false;

        public CollisionEventArgs(Transform transform, Vector3 collisionPoint, Vector3 collisionNormal,
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