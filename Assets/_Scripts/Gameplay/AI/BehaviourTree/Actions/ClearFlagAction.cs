using UnityEngine;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;
using Descent.Common.Attributes.AI;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    /// <summary>
    /// ---Clear Flag---
    /// 
    /// Action node that clears a boolean flag in the blackboard (sets to false).
    /// Returns Success immediately.
    /// </summary>
    public class ClearFlagAction : IBehaviourTreeAction
    {
        [SerializeField, SerializeReference]
        [ShowInNodeInspector("Flag key")]
        private string _flagKey = string.Empty;

        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            contextRegistry.Blackboard.Set(_flagKey, false);
            return BehaviourTreeStatus.Success;
        }

        public IBehaviourTreeAction Clone()
        {
            ClearFlagAction clone = new ClearFlagAction();
            clone._flagKey = _flagKey;
            return clone;
        }

        public void ResetAction()
        {

        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {

        }
    }
}