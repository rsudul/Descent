using Descent.Common.AI.BehaviourTree.Actions;
using Descent.Common.AI.BehaviourTree.Actions.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Requests
{
    public class BehaviourTreeActionRequestDispatcher : MonoBehaviour
    {
        private readonly Dictionary<Transform, IBehaviourTreeRequestReceiver> _receivers =
            new Dictionary<Transform, IBehaviourTreeRequestReceiver>();

        public void Register(Transform agent, IBehaviourTreeRequestReceiver receiver)
        {
            _receivers[agent] = receiver;
        }

        public void Unregister(Transform agent)
        {
            _receivers.Remove(agent);
        }

        public BehaviourTreeRequestResult RequestAction(Transform agent,
            BehaviourTreeActionType actionType, IBehaviourTreeActionData data)
        {
            if (!_receivers.TryGetValue(agent, out IBehaviourTreeRequestReceiver receiver))
            {
                return BehaviourTreeRequestResult.Failure;
            }

            return receiver.HandleRequest(actionType, data);
        }
    }
}