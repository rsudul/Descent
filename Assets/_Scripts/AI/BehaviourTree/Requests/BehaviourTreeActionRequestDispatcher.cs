using Descent.AI.BehaviourTree.Actions;
using Descent.Gameplay.AI.BehaviourTree.Actions.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Requests
{
    public class BehaviourTreeActionRequestDispatcher : MonoBehaviour
    {
        private Dictionary<BehaviourTreeActionType, IBehaviourTreeRequestReceiver> _receivers = new Dictionary<BehaviourTreeActionType, IBehaviourTreeRequestReceiver>();

        public void Register(BehaviourTreeActionType actionType, IBehaviourTreeRequestReceiver receiver)
        {
            _receivers[actionType] = receiver;
        }

        public void Unregister(BehaviourTreeActionType actionType)
        {
            _receivers.Remove(actionType);
        }

        public BehaviourTreeRequestResult RequestAction(BehaviourTreeActionType actionType, IBehaviourTreeActionData data)
        {
            if (!_receivers.TryGetValue(actionType, out IBehaviourTreeRequestReceiver receiver))
            {
                return BehaviourTreeRequestResult.Failure;
            }

            return receiver.HandleRequest(actionType, data);
        }
    }
}