using UnityEngine;
using System;
using Descent.Gameplay.Game;
using UnityEngine.InputSystem;

namespace Descent.Gameplay.Input
{
    public class InputManager : IInitializable
    {
        public static InputManager Instance { get; private set; } = new InputManager();

        private DescentInputActions _inputActions;

        public Vector2 Move { get; private set; }
        public float MoveUp {  get; private set; }
        public float MoveDown { get; private set; }
        public Vector2 Look { get; private set; }
        public float Banking { get; private set; }
        public float FirePressed { get; private set; }

        public int InitializationPriority => 1000;

        public event EventHandler OnBeforeInitialize;
        public event EventHandler OnAfterInitialize;

        public event EventHandler<Vector2> OnMoveChanged;
        public event EventHandler<float> OnMoveUpChanged;
        public event EventHandler<float> OnMoveDownChanged;
        public event EventHandler<Vector2> OnLookChanged;
        public event EventHandler<float> OnBankingChanged;
        public event EventHandler OnFirePressed;
        public event EventHandler OnFireReleased;

        private InputManager()
        {

        }

        public void Initialize()
        {
            OnBeforeInitialize?.Invoke(this, EventArgs.Empty);

            _inputActions = new DescentInputActions();
            _inputActions.Enable();

            BindInputActions();

            OnAfterInitialize?.Invoke(this, EventArgs.Empty);
        }

        private void BindInputActions()
        {
            BindAction<Vector2>(_inputActions.Gameplay.Move,
                value => { Move = value; OnMoveChanged?.Invoke(this, value); },
                value => { Move = Vector2.zero; OnMoveChanged?.Invoke(this, Vector2.zero); });

            BindAction<float>(_inputActions.Gameplay.MoveUp,
                value => { MoveUp = value; OnMoveUpChanged?.Invoke(this, value); },
                value => { MoveUp = 0.0f; OnMoveUpChanged?.Invoke(this, 0.0f); });

            BindAction<float>(_inputActions.Gameplay.MoveDown,
                value => { MoveDown = value; OnMoveDownChanged?.Invoke(this, value); },
                value => { MoveDown = 0.0f; OnMoveDownChanged?.Invoke(this, 0.0f); });

            BindAction<Vector2>(_inputActions.Gameplay.Look,
                value => { Look = value; OnLookChanged?.Invoke(this, value); },
                value => { Look = Vector2.zero; OnLookChanged?.Invoke(this, Vector2.zero); });

            BindAction<float>(_inputActions.Gameplay.Banking,
                value => { Banking = value; OnBankingChanged?.Invoke(this, value); },
                value => { Banking = 0.0f; OnBankingChanged?.Invoke(this, 0.0f); });

            _inputActions.Gameplay.Fire.started += ctx =>
            {
                FirePressed = 1.0f;
                OnFirePressed?.Invoke(this, EventArgs.Empty);
            };
            _inputActions.Gameplay.Fire.canceled += ctx =>
            {
                FirePressed = 0.0f;
                OnFireReleased?.Invoke(this, EventArgs.Empty);
            };
        }

        private void BindAction<T>(InputAction action, Action<T> onPerformed, Action<T> onCanceled = null) where T : struct
        {
            action.performed += ctx => onPerformed?.Invoke(ctx.ReadValue<T>());
            if (onCanceled != null)
            {
                action.canceled += ctx => onCanceled(ctx.ReadValue<T>());
            }
        }

        public void LateInitialize()
        {

        }

        public void EnableGameplayInput()
        {
            _inputActions.Gameplay.Enable();
        }
    }
}