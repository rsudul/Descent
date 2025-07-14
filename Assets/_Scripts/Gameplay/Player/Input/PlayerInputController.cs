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
        public bool GearUpInput { get; private set; }
        public bool GearDownInput { get; private set; }

        public event EventHandler OnFirePressed;
        public event EventHandler OnGearUpPressed;
        public event EventHandler OnGearDownPressed;

        public PlayerInputController()
        {
            InputManager.Instance.OnMoveChanged += HandleMoveChanged;
            InputManager.Instance.OnMoveUpChanged += HandleMoveUpChanged;
            InputManager.Instance.OnMoveDownChanged += HandleMoveDownChanged;
            InputManager.Instance.OnLookChanged += HandleLookChanged;
            InputManager.Instance.OnBankingChanged += HandleBankingChanged;
            InputManager.Instance.OnFirePressed += HandleFirePressed;
            InputManager.Instance.OnFireReleased += HandleFireReleased;
            InputManager.Instance.OnGearUpPressed += HandleGearUpPressed;
            InputManager.Instance.OnGearUpReleased += HandleGearUpReleased;
            InputManager.Instance.OnGearDownPressed += HandleGearDownPressed;
            InputManager.Instance.OnGearDownReleased += HandleGearDownReleased;
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
            InputManager.Instance.OnGearUpPressed -= HandleGearUpPressed;
            InputManager.Instance.OnGearUpReleased -= HandleGearUpReleased;
            InputManager.Instance.OnGearDownPressed -= HandleGearDownPressed;
            InputManager.Instance.OnGearDownReleased -= HandleGearDownReleased;
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

        private void HandleGearUpPressed(object sender, EventArgs args)
        {
            GearUpInput = true;
            OnGearUpPressed?.Invoke(this, EventArgs.Empty);
        }

        private void HandleGearUpReleased(object sender, EventArgs args)
        {
            GearUpInput = false;
        }

        private void HandleGearDownPressed(object sender, EventArgs args)
        {
            GearDownInput = true;
            OnGearDownPressed?.Invoke(this, EventArgs.Empty);
        }

        private void HandleGearDownReleased(object sender, EventArgs args)
        {
            GearDownInput = false;
        }
    }
}