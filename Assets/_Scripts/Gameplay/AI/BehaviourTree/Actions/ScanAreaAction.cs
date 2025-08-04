using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;
using Descent.Gameplay.AI.BehaviourTree.Context;
using UnityEngine;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    public class ScanAreaAction : IBehaviourTreeAction
    {
        private BehaviourTreeActionRequestDispatcher _dispatcher = null;
        private RotateToTargetAction _rotateAction = null;

        private bool _isWaiting = false;
        private float _waitUntilTime = 0.0f;

        private int _direction = 1;

        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            if (_dispatcher == null)
            {
                Debug.LogWarning("ScanAreaAction: dispatcher not set.");
                return BehaviourTreeStatus.Failure;
            }

            AIScanContext context = contextRegistry.GetContext(typeof(AIScanContext)) as AIScanContext;
            if (context == null || context.RotationController == null)
            {
                Debug.LogWarning("ScanAreaAction: No AIRotationContext found.");
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

            if (_rotateAction == null)
            {
                float centerAngle = context.CenterAngle;
                float scanAngle = context.ScanAngle;
                float targetY = (_direction > 0) ? (centerAngle + scanAngle) : (centerAngle - scanAngle);
                _rotateAction = new RotateToTargetAction();
                _rotateAction.InjectDispatcher(_dispatcher);
                _rotateAction._targetYAngle = targetY;
            }

            BehaviourTreeStatus status = _rotateAction.Execute(contextRegistry);

            if (status == BehaviourTreeStatus.Success)
            {
                float waitTimeOnEdge = context.WaitTimeOnEdge;
                _rotateAction = null;
                _isWaiting = true;
                _waitUntilTime = Time.time + waitTimeOnEdge;
                _direction *= -1;
            }

            return BehaviourTreeStatus.Running;
        }

        public IBehaviourTreeAction Clone()
        {
            ScanAreaAction clone = new ScanAreaAction();
            return clone;
        }

        public void ResetAction()
        {
            _rotateAction = null;
            _isWaiting = false;
        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
    }
}