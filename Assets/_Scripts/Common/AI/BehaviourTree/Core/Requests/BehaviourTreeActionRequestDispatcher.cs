using Descent.Common.AI.BehaviourTree.Actions;
using Descent.Common.AI.BehaviourTree.Actions.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Core.Requests
{
    public class BehaviourTreeActionRequestDispatcher : MonoBehaviour
    {
        public static BehaviourTreeActionRequestDispatcher Instance { get; private set; }

        private readonly Dictionary<Transform, IBehaviourTreeRequestReceiver> _receivers =
            new Dictionary<Transform, IBehaviourTreeRequestReceiver>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

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
            if (!_receivers.TryGetValue(agent, out var receiver))
            {
                return BehaviourTreeRequestResult.Failure;
            }

            return receiver.HandleRequest(actionType, data);
        }
    }
}