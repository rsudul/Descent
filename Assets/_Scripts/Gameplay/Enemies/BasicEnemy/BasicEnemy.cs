using Descent.Gameplay.Enemies.BasicEnemy.Damageables;
using System;
using UnityEngine;

namespace Descent.Gameplay.Enemies.BasicEnemy
{
    public class BasicEnemy : Enemy
    {
        [SerializeField]
        private BasicEnemyDamageablesController _damageablesController = new BasicEnemyDamageablesController();

        protected void Awake()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            InitializeControllers();
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
    }
}