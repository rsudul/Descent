using Descent.Attributes.Gameplay.Player;
using Descent.Common;
using Descent.Common.Collisions.Controllers;
using Descent.Common.Events.Arguments;
using Descent.Common.Input;
using Descent.Gameplay.Health;
using Descent.Gameplay.Movement;
using Descent.Gameplay.Player.Animations;
using Descent.Gameplay.Player.Camera;
using Descent.Gameplay.Player.Collisions;
using Descent.Gameplay.Player.Combat;
using Descent.Gameplay.Player.Health;
using Descent.Gameplay.Player.Input;
using Descent.Gameplay.Player.Movement;
using Descent.Gameplay.Player.Settings.Movement;
using UnityEngine;

namespace Descent.Gameplay.Player
{
    [IsPlayerObject]
    public class Player : Actor, IRepairable
    {
        private IInputController _inputController;
        private PlayerShootingController _playerShootingController;
        private IPlayerMovementController _playerMovementController;

        private Vector2 _lookInput = Vector2.zero;
        private float _bankInput = 0.0f;
        private Vector2 _moveInput = Vector2.zero;
        private bool _shootInput = false;

        [Header("Controllers")]
        [SerializeField]
        private PlayerCameraController _playerCameraController = null;
        [SerializeField]
        private HitController _hitController = null;
        [SerializeField]
        private PlayerDamageController _playerDamageController = null;
        [SerializeField]
        private PlayerAnimationsController _playerAnimationsController = new PlayerAnimationsController();
        [SerializeField]
        private PlayerCollisionsController _playerCollisionsController = new PlayerCollisionsController();

        [Header("References")]
        [SerializeField]
        private Rigidbody _rigidbody = null;
        [SerializeField]
        private Transform _playerBody = null;

        [Header("Settings")]
        [SerializeField]
        private PlayerMovementSettings _playerMovementSettings = null;

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
            _playerMovementController = new PlayerMovementController();

            PlayerMovementController playerMovementController = _playerMovementController as PlayerMovementController;

            playerMovementController.Initialize(_playerBody, _rigidbody, _playerMovementSettings);
            _playerAnimationsController.Initialize(_playerCameraController.CameraTransform);
            _playerCollisionsController.Initialize(_hitController, _rigidbody);

            _playerCollisionsController.OnCollision += OnCollision;
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
            PlayerMovementController playerMovementController = _playerMovementController as PlayerMovementController;
            playerMovementController.SetPitchYawAndRoll(_lookInput.x, _lookInput.y, _bankInput);
            playerMovementController.SetMovementFactors(_moveInput.x, _moveInput.y);
            _playerAnimationsController.SetMovementVelocity(new Vector3(_moveInput.x, 0.0f, _moveInput.y));
        }

        private void UpdateControllers(float deltaTime)
        {
            _playerMovementController.UpdateLook(_playerBody, _rigidbody, Time.deltaTime);
            _playerAnimationsController.UpdateAnimations(deltaTime);

            if (_shootInput)
            {
                // store projectile spawn position somewhere else
                _playerShootingController.Shoot(transform.forward,
                    transform.position + transform.forward * 0.5f, deltaTime, _playerCollisionsController.Collider);
            }
        }

        private void UpdateControllersPhysics(float deltaTime)
        {
            _playerMovementController.UpdateMovement(transform, _rigidbody, deltaTime);
        }

        private void OnCollision(object sender, CollisionEventArguments args)
        {
            if (args.IsTrigger)
            {
                return;
            }

            Vector3 impactVelocity = _playerMovementController.Velocity;
            float shakeStrength = impactVelocity.magnitude;

            _playerMovementController.Bounce(_rigidbody, args.CollisionNormal);
            _playerCameraController.Shake(shakeStrength);
            _playerMovementController.FreezeMovement();

            _playerDamageController?.TakeDamage(Random.Range(0.0f, 20.0f), args.CollisionPoint);
        }

        public void RepairFull()
        {

        }

        public void RepairPartial(float amount)
        {

        }
    }
}