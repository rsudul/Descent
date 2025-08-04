using UnityEngine;
using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    [System.Serializable]
    public class UpdateTimersAction : IBehaviourTreeAction
    {
        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            float deltaTime = Time.deltaTime;

            float alertTimer = contextRegistry.Blackboard.Get<float>("AlertTimer", 0.0f);
            alertTimer += deltaTime;
            contextRegistry.Blackboard.Set("AlertTimer", alertTimer);

            int alertLevel = contextRegistry.Blackboard.Get<int>("AlertLevel", 0);
            if (alertLevel == 3)
            {
                float combatTimer = contextRegistry.Blackboard.Get<float>("CombatTimer", 0.0f);
                combatTimer += deltaTime;
                contextRegistry.Blackboard.Set("CombatTimer", combatTimer);
            }

            if (alertLevel == 4)
            {
                float searchTime = contextRegistry.Blackboard.Get<float>("SearchTimeRemaining", 0.0f);
                searchTime -= deltaTime;
                searchTime = Mathf.Max(0.0f, searchTime);
                contextRegistry.Blackboard.Set("SearchTimeRemaining", searchTime);
            }

            return BehaviourTreeStatus.Success;
        }

        public void ResetAction()
        {

        }

        public IBehaviourTreeAction Clone()
        {
            return new UpdateTimersAction();
        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {

        }
    }
}