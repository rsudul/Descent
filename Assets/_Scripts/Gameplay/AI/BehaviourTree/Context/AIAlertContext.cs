using UnityEngine;
using Descent.AI.BehaviourTree.Context;
using Descent.Gameplay.Systems.Alert;

namespace Descent.Gameplay.AI.BehaviourTree.Context
{
    public class AIAlertContext : BehaviourTreeContext
    {
        public IAlertController AlertController { get; private set; }

        public AlertLevel CurrentAlertLevel => AlertController.CurrentAlertLevel;
        public float AlertTimer => AlertController.AlertTimer;
        public float CombatTimer => AlertController.CombatTimer;
        public float SearchTimeRemaining => AlertController.SearchTimeRemaining;

        public AIAlertContext(GameObject owner, IAlertController alertController) : base(owner)
        {
            AlertController = alertController;
        }
    }
}