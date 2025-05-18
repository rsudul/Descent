using Descent.Common;
using Descent.Common.Input;
using Descent.Gameplay.Player.Animations;
using Descent.Gameplay.Player.Combat;
using Descent.Gameplay.Player.Input;
using Descent.Gameplay.Player.Movement;
using UnityEngine;

namespace Descent.Gameplay.Player
{
    public class Player : Actor
    {
        private IInputController _inputController;
        private PlayerShootingController _playerShootingController;

        private Vector2 _lookInput = Vector2.zero;
        private float _bankInput = 0.0f;
        private Vector2 _moveInput = Vector2.zero;
        private bool _shootInput = false;

        [SerializeField]
        private Transform _playerBody = null;
        [SerializeField]
        private Transform _playerCameraPivot = null;

        [SerializeField]
        private Rigidbody _rigidbody = null;
        [SerializeField]
        private PlayerMovementController _playerMovementController = new PlayerMovementController();
        [SerializeField]
        private PlayerAnimationsController _playerAnimationsController = new PlayerAnimationsController();

        private void Awake()
        {
            Initialize();
            InitializeControllers();

            // this should be put somewhere else
            Cursor.lockState = CursorLockMode.Locked;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        private void InitializeControllers()
        {
            _inputController = new PlayerInputController();
            _playerShootingController = new PlayerShootingController();

            _playerMovementController.Initialize(_playerBody, _rigidbody);
            _playerAnimationsController.Initialize(_playerCameraPivot);
        }

        private void Update()
        {
            float deltaTime = GetDeltaTime();

            GetInput();
            FeedInputToControllers();
            UpdateControllers(deltaTime);
        }

        private void FixedUpdate()
        {
            float fixedDeltaTime = GetFixedDeltaTime();

            UpdateControllersPhysics(fixedDeltaTime);
        }

        private float GetDeltaTime()
        {
            return Time.deltaTime;
        }

        private float GetFixedDeltaTime()
        {
            return Time.fixedDeltaTime;
        }

        private void GetInput()
        {
            _inputController.Refresh();

            _lookInput = new Vector2(_inputController.LookX, _inputController.LookY);
            _bankInput = _inputController.Banking;

            _moveInput = new Vector2(_inputController.MoveX, _inputController.MoveY);
            _moveInput = _moveInput.normalized;

            _shootInput = _inputController.Shoot;
        }

        private void FeedInputToControllers()
        {
            _playerMovementController.SetPitchYawAndRoll(_lookInput.x, _lookInput.y, _bankInput);
            _playerMovementController.SetMovementFactors(_moveInput.x, _moveInput.y);
            _playerAnimationsController.SetMovementVelocity(new Vector3(_moveInput.x, 0.0f, _moveInput.y));
        }

        private void UpdateControllers(float deltaTime)
        {
            _playerMovementController.UpdateLook(_playerBody, _rigidbody, deltaTime);
            _playerAnimationsController.UpdateAnimations(deltaTime);

            if (_shootInput)
            {
                // store projectile spawn position somewhere else
                _playerShootingController.Shoot(transform.forward,
                    transform.position + transform.forward * 0.5f, deltaTime);
            }
        }

        private void UpdateControllersPhysics(float deltaTime)
        {
            _playerMovementController.UpdateMovement(transform, _rigidbody, deltaTime);
        }
    }
}