using Descent.AI.BehaviourTree.Actions;
using Descent.Gameplay.AI.BehaviourTree.Actions.Data;

namespace Descent.AI.BehaviourTree.Requests
{
    public interface IBehaviourTreeRequestReceiver
    {
        BehaviourTreeRequestResult HandleRequest(BehaviourTreeActionType actionType, IBehaviourTreeActionData data);
    }
}