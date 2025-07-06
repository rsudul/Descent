using System;
using UnityEngine;

namespace Descent.Common.Events.Arguments
{
    public class DamageEventArgs : EventArgs
    {
        public float DamageAmount { get; }
        public Vector3 SourcePosition { get; }

        public DamageEventArgs(float damageAmount, Vector3 sourcePosition)
        {
            DamageAmount = damageAmount;
            SourcePosition = sourcePosition;
        }
    }
}