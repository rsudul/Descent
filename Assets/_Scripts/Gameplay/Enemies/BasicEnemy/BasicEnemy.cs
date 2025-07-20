/*using Descent.Gameplay.Systems.Hostility;
using System;
using UnityEngine;

namespace Descent.Gameplay.Enemies.BasicEnemy
{
    public class BasicEnemy : Enemy
    {
        [SerializeField]
        private BasicEnemyDamageablesController _damageablesController = new BasicEnemyDamageablesController();
        [SerializeField]
        private Faction _faction = null;

        public override void Initialize()
        {
            InvokeOnBeforeInitialize();
            InitializeControllers();
            InvokeOnAfterInitialize();
        }

        private void InitializeControllers()
        {
            _damageablesController.Initialize();

            _damageablesController.OnDied += OnDied;
        }

        private void OnDied(object sender, EventArgs args)
        {
            gameObject.SetActive(false);
        }

        public override Faction GetFaction()
        {
            return _faction;
        }

        public override void SetFaction(Faction faction)
        {
            if (_faction == faction)
            {
                return;
            }

            _faction = faction;
            InvokeFactionChanged(_faction);
        }
    }
}*/