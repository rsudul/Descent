using UnityEngine;
using Descent.AI.BehaviourTree.Context;
using Descent.Gameplay.Systems.Alert;

namespace Descent.Gameplay.AI.BehaviourTree.Context
{
    public class AIAlertContext : BehaviourTreeContext
    {
        private IAlertController _alertController = null;

        public AlertLevel CurrentAlertLevel => _alertController.CurrentAlertLevel;
        public float AlertTimer => _alertController.AlertTimer;
        public float CombatTimer => _alertController.CombatTimer;
        public float SearchTimeRemaining => _alertController.SearchTimeRemaining;

        public AIAlertContext(GameObject owner, IAlertController alertController) : base(owner)
        {
            _alertController = alertController;
        }
    }
}