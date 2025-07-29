using Descent.AI.BehaviourTree.Actions;
using Descent.Gameplay.AI.BehaviourTree.Actions.Data;
using System.Collections.Generic;

namespace Descent.AI.BehaviourTree.Requests
{
    public interface IBehaviourTreeRequestReceiver
    {
        BehaviourTreeRequestResult HandleRequest(BehaviourTreeActionType actionType, IBehaviourTreeActionData data);
    }
}