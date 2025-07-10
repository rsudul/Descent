using Descent.Gameplay.Input;
using System;
using UnityEngine;

namespace Descent.Gameplay.Player.Input
{
    public class PlayerInputController : IDisposable
    {
        private bool _moveUp = false;
        private bool _moveDown = false;

        public Vector2 MoveInput { get; private set; }
        public float MoveVerticalInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public float BankingInput { get; private set; }
        public bool FireInput { get; private set; }

        public event EventHandler OnFirePressed;

        public PlayerInputController()
        {
            InputManager.Instance.OnMoveChanged += HandleMoveChanged;
            InputManager.Instance.OnMoveUpChanged += HandleMoveUpChanged;
            InputManager.Instance.OnMoveDownChanged += HandleMoveDownChanged;
            InputManager.Instance.OnLookChanged += HandleLookChanged;
            InputManager.Instance.OnBankingChanged += HandleBankingChanged;
            InputManager.Instance.OnFirePressed += HandleFirePressed;
            InputManager.Instance.OnFireReleased += HandleFireReleased;
        }

        public void Dispose()
        {
            InputManager.Instance.OnMoveChanged -= HandleMoveChanged;
            InputManager.Instance.OnMoveUpChanged -= HandleMoveUpChanged;
            InputManager.Instance.OnMoveDownChanged -= HandleMoveDownChanged;
            InputManager.Instance.OnLookChanged -= HandleLookChanged;
            InputManager.Instance.OnBankingChanged -= HandleBankingChanged;
            InputManager.Instance.OnFirePressed -= HandleFirePressed;
            InputManager.Instance.OnFireReleased -= HandleFireReleased;
        }

        private void HandleMoveChanged(object sender, Vector2 value)
        {
            MoveInput = value;
        }

        private void HandleMoveUpChanged(object sender, float value)
        {
            _moveUp = Mathf.Abs(value) > 0.01f ? true : false;
            MoveVerticalInput = _moveDown ? 0.0f : value;
        }

        private void HandleMoveDownChanged(object sender, float value)
        {
            _moveDown = Mathf.Abs(value) > 0.01f ? true : false;
            MoveVerticalInput = _moveUp ? 0.0f : value;
        }

        private void HandleLookChanged(object sender, Vector2 value)
        {
            LookInput = value;
        }

        private void HandleBankingChanged(object sender, float value)
        {
            BankingInput = value;
        }

        private void HandleFirePressed(object sender, EventArgs args)
        {
            FireInput = true;
            OnFirePressed?.Invoke(this, EventArgs.Empty);
        }

        private void HandleFireReleased(object sender, EventArgs args)
        {
            FireInput = false;
        }
    }
}