using Descent.Gameplay.DamageableObjects;
using System;
using UnityEngine;

namespace Descent.Gameplay.Enemies.BasicEnemy.Damageables
{
    [Serializable]
    public class BasicEnemyDamageablesController
    {
        private IDamageable _armorDamageable = null;
        private IDamageable _coreDamageable = null;

        public event EventHandler OnDied;

        [SerializeField]
        private GameObject _armorDamageableGameObject = null;
        [SerializeField]
        private GameObject _coreDamageableGameObject = null;

        public void Initialize()
        {
            if (!ValidateDamageables())
            {
                return;
            }

            _armorDamageable.OnDamageTaken += ArmorTakenDamage;
            _armorDamageable.OnDied += ArmorDied;

            _coreDamageable.OnDamageTaken += CoreTakenDamage;
            _coreDamageable.OnDied += CoreDied;
        }

        private bool ValidateDamageables()
        {
            if (_armorDamageableGameObject == null || _coreDamageableGameObject == null
                || !_armorDamageableGameObject.TryGetComponent<IDamageable>(out _armorDamageable)
                || !_coreDamageableGameObject.TryGetComponent<IDamageable>(out _coreDamageable))
            {
                return false;
            }

            return true;
        }

        private void ArmorTakenDamage(object sender, EventArgs args)
        {

        }

        private void ArmorDied(object sender, EventArgs args)
        {
            CleanUpArmor();
            _armorDamageableGameObject.SetActive(false);
        }

        private void CleanUpArmor()
        {
            _armorDamageable.OnDamageTaken -= ArmorTakenDamage;
            _armorDamageable.OnDied -= ArmorDied;
        }

        private void CoreTakenDamage(object sender, EventArgs args)
        {

        }

        private void CoreDied(object sender, EventArgs args)
        {
            OnDied?.Invoke(sender, args);
        }

        private void CleanUpCore()
        {
            _coreDamageable.OnDamageTaken -= CoreTakenDamage;
            _coreDamageable.OnDied -= CoreDied;
        }
    }
}