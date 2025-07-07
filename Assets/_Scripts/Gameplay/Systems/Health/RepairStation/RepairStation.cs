using Descent.Common.Collisions.Controllers;
using Descent.Common.Events.Arguments;
using UnityEngine;

namespace Descent.Gameplay.Systems.Health.RepairStation
{
    public class RepairStation : MonoBehaviour
    {
        [SerializeField]
        private HitController _hitController = null;

        private void Awake()
        {
            _hitController.OnHit += OnHit;
        }

        private void OnHit(object sender, CollisionEventArguments args)
        {
            if (!args.Transform.TryGetComponent<IRepairable>(out IRepairable repairable))
            {
                return;
            }

            repairable.RepairFull();
        }
    }
}