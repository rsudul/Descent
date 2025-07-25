using UnityEngine;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;
using Descent.Gameplay.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Actions;
using Descent.Gameplay.AI.BehaviourTree.Actions.Data;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    /// <summary>
    /// ---Shoot---
    /// 
    /// Action node that requests a shooting command via dispatcher.
    /// Returns Running for one tick while the request is being processed, then Success.
    /// </summary>
    public class ShootAction : IBehaviourTreeAction
    {
        private BehaviourTreeActionRequestDispatcher _dispatcher;
        private bool _requested = false;

        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            if (_dispatcher == null)
            {
                Debug.LogError("[ShootAction]: dispatcher not set.");
                return BehaviourTreeStatus.Failure;
            }

            AIRotationContext rotationContext = contextRegistry.GetContext(typeof(AIRotationContext)) as AIRotationContext;
            if (rotationContext == null)
            {
                Debug.LogWarning("[ShootAction]: No AIRotationContext found.");
                return BehaviourTreeStatus.Failure;
            }

            if (!_requested)
            {
                ShootActionData data = new ShootActionData();
                BehaviourTreeRequestResult result = _dispatcher.RequestAction(rotationContext.AgentTransform,
                                                                              BehaviourTreeActionType.Shoot,
                                                                              data);

                if (result == BehaviourTreeRequestResult.Failure)
                {
                    Debug.LogWarning("[ShootAction]: dispatcher failed to enqueue shoot.");
                    return BehaviourTreeStatus.Failure;
                }

                _requested = true;
                return BehaviourTreeStatus.Running;
            }

            _requested = false;
            return BehaviourTreeStatus.Success;
        }

        public IBehaviourTreeAction Clone()
        {
            ShootAction clone = new ShootAction();
            return clone;
        }

        public void ResetAction()
        {
            _requested = false;
        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
    }
}