using UnityEngine;
using Descent.Gameplay.Systems.Hostility;
using System.Collections.Generic;
using Descent.Gameplay.Entities;
using Descent.Gameplay.Systems.Perception;
using Descent.Common.Attributes.AI;
using Descent.Gameplay.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Context;
using Descent.Gameplay.Movement;
using Descent.Gameplay.Systems.WeaponSystem.Core;
using Descent.Gameplay.Systems.WeaponSystem;

namespace Descent.Gameplay.Enemies.Turret
{
    [BehaviourTreeContextProvider(typeof(AIScanContext))]
    [BehaviourTreeContextProvider(typeof(AIFactionContext))]
    [BehaviourTreeContextProvider(typeof(AIPerceptionContext))]
    public class Turret : Enemy, IBehaviourTreeContextProvider
    {
        private List<Actor> _visibleActors = new List<Actor>();

        private float _perceptionTimer = 0.0f;
        private float _perceptionInterval = 0.2f;

        private WeaponSystemController _weaponSystemController;

        public override IReadOnlyCollection<Actor> VisibleActors => _visibleActors;

        public override Transform WeaponMountPoint => _weaponMountPoint;

        [SerializeField]
        private Transform _headTransform = null;
        [SerializeField]
        private Faction _faction = null;
        [SerializeField]
        private PerceptionSettings _perceptionSettings = null;
        [SerializeField]
        private TurretSettings _turretSettings = null;
        [SerializeField]
        private WeaponsConfig _weaponsConfig = null;
        [SerializeField]
        private Transform _weaponMountPoint = null;

        public override void Initialize()
        {
            _weaponSystemController = new WeaponSystemController();
            _weaponSystemController.Initialize(_weaponsConfig, this);
            if (_weaponSystemController.Weapons.Count > 0)
            {
                _weaponSystemController.EquipWeapon(_weaponSystemController.Weapons[0]);
            }
        }

        private void Update()
        {
            _perceptionTimer -= Time.deltaTime;
            if (_perceptionTimer <= 0.0f)
            {
                UpdatePerception();
                _perceptionTimer = _perceptionInterval;
            }
        }

        private void OnDestroy()
        {
            _visibleActors.Clear();
        }

        public override Faction GetFaction()
        {
            return _faction;
        }

        public override void SetFaction(Faction faction)
        {
            if (faction == _faction)
            {
                return;
            }

            _faction = faction;
            InvokeFactionChanged(_faction);
        }

        private void UpdatePerception()
        {
            _visibleActors.Clear();

            Collider[] hits = Physics.OverlapSphere(transform.position, _perceptionSettings.Range,
                _perceptionSettings.DetectionMask);

            foreach (Collider hit in hits)
            {
                Actor actor = hit.GetComponent<Actor>();

                if (actor == null || actor == this)
                {
                    continue;
                }

                Vector3 directionToTarget = (actor.transform.position - _headTransform.position).normalized;
                float angle = Vector3.Angle(_headTransform.forward, directionToTarget);
                if (angle > _perceptionSettings.ViewAngle * 0.5f)
                {
                    continue;
                }

                Vector3 origin = _headTransform.position + Vector3.up * 0.5f;
                float distance = Vector3.Distance(_headTransform.position, actor.transform.position);
                if (Physics.Raycast(origin, (actor.transform.position - origin).normalized, out RaycastHit hitInfo,
                    distance, _perceptionSettings.DetectionMask))
                {
                    if (hitInfo.collider != hit)
                    {
                        continue;
                    }
                }

                _visibleActors.Add(actor);
            }
        }

        public BehaviourTreeContext GetBehaviourTreeContext(System.Type contextType, GameObject agent)
        {
            if (contextType == typeof(AIScanContext))
            {
                IAIRotationController rotationController = GetComponent<IAIRotationController>();
                return new AIScanContext(agent, rotationController, _turretSettings.ScanCenterAngle,
                    _turretSettings.ScanAngle, _turretSettings.WaitTimeOnEdge);
            }

            if (contextType == typeof(AIFactionContext))
            {
                return new AIFactionContext(agent, _faction);
            }

            if (contextType == typeof(AIPerceptionContext))
            {
                return new AIPerceptionContext(agent, this,
                    _visibleActors.Count > 0 ? _visibleActors[0].transform : null);
            }

            return null;
        }
    }
}