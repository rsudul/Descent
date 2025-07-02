using Descent.Common.AI.BehaviourTree.Actions;
using Descent.Common.AI.BehaviourTree.Actions.Data;

namespace Descent.Common.AI.BehaviourTree.Core.Requests
{
    public interface IBehaviourTreeRequestReceiver
    {
        BehaviourTreeRequestResult HandleRequest(BehaviourTreeActionType actionType, IBehaviourTreeActionData data);
    }
}