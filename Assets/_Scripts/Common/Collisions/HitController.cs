using System;
using UnityEngine;

namespace Descent.Common.Collisions
{
    [RequireComponent(typeof(Collider))]
    public class HitController : MonoBehaviour
    {
        public event EventHandler OnHit;

        private void OnCollisionEnter(Collision collision)
        {
            OnHit?.Invoke(this, new EventArgs());
        }
    }
}