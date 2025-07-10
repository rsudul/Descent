using Descent.Gameplay.Input;
using System;
using UnityEngine;

namespace Descent.Gameplay.Player.Input
{
    public class PlayerInputController : IDisposable
    {
        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public float BankingInput { get; private set; }
        public bool FireInput { get; private set; }

        public event EventHandler OnFirePressed;

        public PlayerInputController()
        {
            InputManager.Instance.OnMoveChanged += OnMoveChanged;
            InputManager.Instance.OnLookChanged += OnLookChanged;
            InputManager.Instance.OnBankingChanged += OnBankingChanged;
            InputManager.Instance.OnFirePressed += HandleFirePressed;
            InputManager.Instance.OnFireReleased += OnFireReleased;
        }

        public void Dispose()
        {
            InputManager.Instance.OnMoveChanged -= OnMoveChanged;
            InputManager.Instance.OnLookChanged -= OnLookChanged;
            InputManager.Instance.OnBankingChanged -= OnBankingChanged;
            InputManager.Instance.OnFirePressed -= HandleFirePressed;
            InputManager.Instance.OnFireReleased -= OnFireReleased;
        }

        private void OnMoveChanged(object sender, Vector2 value)
        {
            MoveInput = value;
        }

        private void OnLookChanged(object sender, Vector2 value)
        {
            LookInput = value;
        }

        private void OnBankingChanged(object sender, float value)
        {
            BankingInput = value;
        }

        private void HandleFirePressed(object sender, EventArgs args)
        {
            FireInput = true;
            OnFirePressed?.Invoke(this, EventArgs.Empty);
        }

        private void OnFireReleased(object sender, EventArgs args)
        {
            FireInput = false;
        }
    }
}