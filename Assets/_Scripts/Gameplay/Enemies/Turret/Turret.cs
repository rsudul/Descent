using UnityEngine;
using Descent.Gameplay.Systems.Hostility;
using System.Collections.Generic;
using Descent.Gameplay.Entities;
using Descent.Gameplay.Systems.Perception;

namespace Descent.Gameplay.Enemies.Turret
{
    public class Turret : Enemy
    {
        private List<Actor> _visibleActors = new List<Actor>();

        private float _perceptionTimer = 0.0f;
        private float _perceptionInterval = 0.2f;

        public override IReadOnlyCollection<Actor> VisibleActors => _visibleActors;

        [SerializeField]
        private Transform _headTransform = null;
        [SerializeField]
        private float _turnSpeed = 180.0f;
        [SerializeField]
        private Faction _faction = null;
        [SerializeField]
        private PerceptionSettings _perceptionSettings = null;

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

        private bool RotateHeadTowards(Vector3 targetPosition, float turnSpeed)
        {
            if (_headTransform == null)
            {
                return false;
            }

            Vector3 direction = targetPosition - _headTransform.position;
            direction.y = 0.0f;

            if (direction.sqrMagnitude < 0.0001f)
            {
                return true;
            }

            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            _headTransform.rotation = Quaternion.RotateTowards(_headTransform.rotation, lookRotation,
                turnSpeed * Time.deltaTime);

            float angle = Quaternion.Angle(_headTransform.rotation, lookRotation);
            return angle < 1.0f;
        }

        private Actor ChooseTarget(IReadOnlyCollection<Actor> actors)
        {
            float minDist = float.MaxValue;
            Actor closest = null;

            foreach (Actor actor in actors)
            {
                float dist = Vector3.Distance(transform.position, actor.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = actor;
                }
            }

            return closest;
        }
    }
}