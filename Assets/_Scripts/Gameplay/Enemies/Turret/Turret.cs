using UnityEngine;
using Descent.Gameplay.Systems.Hostility;
using System.Collections.Generic;
using Descent.Gameplay.Entities;
using Descent.Gameplay.Systems.Perception;
using Descent.Common.Attributes.AI;
using Descent.Gameplay.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Context;
using Descent.Gameplay.Movement;

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

        public override IReadOnlyCollection<Actor> VisibleActors => _visibleActors;

        [SerializeField]
        private Faction _faction = null;
        [SerializeField]
        private PerceptionSettings _perceptionSettings = null;
        [SerializeField]
        private TurretSettings _turretSettings = null;

        private void Update()
        {
            _perceptionTimer -= Time.deltaTime;
            if (_perceptionTimer <= 0.0f)
            {
                UpdatePerception();
                _perceptionTimer = _perceptionInterval;
            }
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

                Vector3 directionToTarget = (actor.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, directionToTarget);
                if (angle > _perceptionSettings.ViewAngle * 0.5f)
                {
                    continue;
                }

                Vector3 origin = transform.position + Vector3.up * 0.5f;
                float distance = Vector3.Distance(transform.position, actor.transform.position);
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
                return new AIPerceptionContext(agent, this);
            }

            return null;
        }
    }
}