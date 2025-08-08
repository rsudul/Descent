using UnityEngine;
using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;
using Descent.Common.Attributes.AI;
using Descent.Gameplay.AI.BehaviourTree.Actions.Data;
using Descent.Gameplay.AI.BehaviourTree.Context;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    public class ScanAreaAction : IBehaviourTreeAction
    {
        private struct ScanConfig
        {
            public float CenterAngle;
            public float ScanAngle;
            public float WaitTime;
        }

        private BehaviourTreeActionRequestDispatcher _dispatcher;

        private bool _isWaiting = false;
        private float _waitUntilTime = 0.0f;
        private int _direction = 1;
        private bool _requestedRotation = false;
        private bool _initialized = false;

        [Header("Scan configuration")]
        [ShowInNodeInspector("Use context settings")]
        [SerializeField]
        private bool _useContextSettings = true;
        [ShowInNodeInspector("Override center angle")]
        [SerializeField]
        private float _overrideCenterAngle = 0.0f;
        [ShowInNodeInspector("Override scan angle")]
        [SerializeField]
        private float _overrideScanAngle = 90.0f;
        [ShowInNodeInspector("Override wait time")]
        [SerializeField]
        private float _overrideWaitTime = 2.0f;

        private BehaviourTreeStatus status;

        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            if (_dispatcher == null)
            {
                Debug.Log("[ScanAreaAction] No dispatcher.");
                return BehaviourTreeStatus.Failure;
            }

            AIRotationContext rotationContext = contextRegistry.GetContext(typeof(AIRotationContext)) as AIRotationContext;
            if (rotationContext == null)
            {
                Debug.Log("[ScanAreaAction] No AIRotationContext found.");
                return BehaviourTreeStatus.Failure;
            }

            ScanConfig config = GetScanConfiguration(contextRegistry);

            if (!_initialized)
            {
                _initialized = true;
                _direction = 1;
            }

            if (_isWaiting)
            {
                if (Time.time < _waitUntilTime)
                {
                    return BehaviourTreeStatus.Running;
                }

                _isWaiting = false;
                _requestedRotation = false;
            }

            float targetAngle = config.CenterAngle + (_direction > 0 ? config.ScanAngle : -config.ScanAngle);

            if (!_requestedRotation && !rotationContext.IsRotating)
            {
                RotateToTargetActionData rotateData = new RotateToTargetActionData(targetAngle);

                BehaviourTreeRequestResult result = _dispatcher.RequestAction(BehaviourTreeActionType.RotateTo, rotateData);

                if (result == BehaviourTreeRequestResult.Success)
                {
                    _requestedRotation = true;
                }
                else
                {
                    return BehaviourTreeStatus.Failure;
                }
            }

            if (_requestedRotation && rotationContext.IsRotating)
            {
                return BehaviourTreeStatus.Running;
            }

            if (_requestedRotation && !rotationContext.IsRotating)
            {
                _isWaiting = true;
                _waitUntilTime = Time.time + config.WaitTime;
                _direction *= -1;
            }

            return BehaviourTreeStatus.Running;
        }

        public IBehaviourTreeAction Clone()
        {
            ScanAreaAction clone = new ScanAreaAction();
            clone._useContextSettings = _useContextSettings;
            clone._overrideCenterAngle = _overrideCenterAngle;
            clone._overrideScanAngle = _overrideScanAngle;
            clone._overrideWaitTime = _overrideWaitTime;
            return clone;
        }

        public void ResetAction()
        {
            _isWaiting = false;
            _requestedRotation = false;
            _initialized = false;
            _direction = 1;
        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        private ScanConfig GetScanConfiguration(BehaviourTreeContextRegistry contextRegistry)
        {
            if (_useContextSettings)
            {
                AIScanContext scanContext = contextRegistry.GetContext(typeof(AIScanContext)) as AIScanContext;
                if (scanContext != null)
                {
                    return new ScanConfig
                    {
                        CenterAngle = scanContext.CenterAngle,
                        ScanAngle = scanContext.ScanAngle,
                        WaitTime = scanContext.WaitTimeOnEdge
                    };
                }
            }

            return new ScanConfig
            {
                CenterAngle = _overrideCenterAngle,
                ScanAngle = _overrideScanAngle,
                WaitTime = _overrideWaitTime
            };
        }
    }
}