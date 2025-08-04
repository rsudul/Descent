using Descent.Gameplay.AI.BehaviourTree.Actions.Data;
using Descent.Gameplay.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;
using System.Collections.Generic;
using UnityEngine;

namespace Descent.Gameplay.AI.BehaviourTree.Actions.Movement
{
    [System.Serializable]
    public class PatrolAction : IBehaviourTreeAction
    {
        private BehaviourTreeActionRequestDispatcher _dispatcher;

        private int _currentTargetIndex = -1;
        private MoveToTargetAction _moveToTarget = null;

        private bool _isWaiting = false;
        private float _waitUntilTime = 0.0f;

        private int _direction = 1;

        [SerializeField]
        private List<Vector3> _patrolPoints = new List<Vector3>();
        [SerializeField]
        private float _waitTimeOnPoint = 2.0f;
        [SerializeField]
        private PatrolMode _patrolMode = PatrolMode.Default;

        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            if (_patrolPoints?.Count == 0 || _dispatcher == null)
            {
                Debug.LogWarning("PatrolAction: No patrol points set.");
                return BehaviourTreeStatus.Failure;
            }

            AIMovementContext context = contextRegistry.GetContext(typeof(AIMovementContext)) as AIMovementContext;
            if (context == null)
            {
                Debug.LogWarning("Patrol Action: No AIMovementContext found.");
                return BehaviourTreeStatus.Failure;
            }

            if (_isWaiting)
            {
                if (Time.time < _waitUntilTime)
                {
                    return BehaviourTreeStatus.Running;
                }

                _isWaiting = false;
            }

            if (_moveToTarget == null)
            {
                ChooseNextPatrolTarget();
                Vector3 nextTarget = _patrolPoints[_currentTargetIndex];

                SetMovementTargetAction setTarget = new SetMovementTargetAction(nextTarget);
                setTarget.InjectDispatcher(_dispatcher);
                setTarget.Execute(contextRegistry);

                _moveToTarget = new MoveToTargetAction();
                _moveToTarget.InjectDispatcher(_dispatcher);
            }

            BehaviourTreeStatus status = _moveToTarget.Execute(contextRegistry);

            if (status == BehaviourTreeStatus.Success)
            {
                Debug.Log("Arrived at point" + _patrolPoints[_currentTargetIndex]);
                _moveToTarget = null;

                _isWaiting = true;
                _waitUntilTime = Time.time + _waitTimeOnPoint;
            }

            return BehaviourTreeStatus.Running;
        }

        public IBehaviourTreeAction Clone()
        {
            return new PatrolAction();
        }

        public void ResetAction()
        {

        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        private void ChooseNextPatrolTarget()
        {
            if (_patrolPoints.Count <= 1)
            {
                _currentTargetIndex = 0;
                return;
            }

            switch (_patrolMode)
            {
                case PatrolMode.PingPong:
                    _currentTargetIndex += _direction;
                    if (_currentTargetIndex >= _patrolPoints.Count)
                    {
                        _currentTargetIndex = _patrolPoints.Count - 2;
                        _direction = -1;
                    } else if (_currentTargetIndex < 0)
                    {
                        _currentTargetIndex = 1;
                        _direction = 1;
                    }
                    break;

                case PatrolMode.Random:
                    int newIndex;
                    do
                    {
                        newIndex = Random.Range(0, _patrolPoints.Count);
                    } while (_patrolPoints.Count > 1 && newIndex == _currentTargetIndex);
                    _currentTargetIndex = newIndex;
                    break;

                case PatrolMode.Default:
                default:
                    _currentTargetIndex = (_currentTargetIndex + 1) % _patrolPoints.Count;
                    break;
            }
        }
    }
}