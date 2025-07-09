using Descent.Common.Attributes.Gameplay.Player;
using Descent.Gameplay.Entities;
using Descent.Gameplay.Collisions;
using Descent.Gameplay.Events.Arguments;
using Descent.Gameplay.Input;
using Descent.Gameplay.Systems.Durability.Health;
using Descent.Gameplay.Systems.Durability.Repair;
using Descent.Gameplay.Movement;
using Descent.Gameplay.Player.Animations;
using Descent.Gameplay.Player.Camera;
using Descent.Gameplay.Player.Collisions;
using Descent.Gameplay.Player.Combat;
using Descent.Gameplay.Player.Input;
using Descent.Gameplay.Player.Movement;
using Descent.Gameplay.Systems.Hostility;
using UnityEngine;

namespace Descent.Gameplay.Player
{
    [IsPlayerObject]
    public class Player : Actor, IRepairable
    {
        private IInputController _inputController;
        private IPlayerMovementController _playerMovementController;
        private IHealthController _healthController;

        private Vector2 _lookInput = Vector2.zero;
        private float _bankInput = 0.0f;
        private Vector2 _moveInput = Vector2.zero;
        private bool _shootInput = false;

        public IHealthController HealthController => _healthController;

        [Header("Controllers")]
        [SerializeField]
        private PlayerCameraController _playerCameraController = null;
        [SerializeField]
        private HitController _hitController = null;
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
        [SerializeField]
        private Faction _faction = null;
        [SerializeField]
        private HealthSettings _healthSettings = null;

        private void Awake()
        {
            // this should be put somewhere else
            Cursor.lockState = CursorLockMode.Locked;
        }

        public override void Initialize()
        {
            InvokeOnBeforeInitialize();
            InitializeControllers();
            InvokeOnAfterInitialize();
        }

        private void InitializeControllers()
        {
            _inputController = new PlayerInputController();
            _playerMovementController = new PlayerMovementController();
            _healthController = new HealthController(_healthSettings);

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
        }

        private void UpdateControllersPhysics(float deltaTime)
        {
            _playerMovementController.UpdateMovement(transform, _rigidbody, deltaTime);
        }

        private void OnCollision(object sender, CollisionEventArgs args)
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

            // instead, have IDamageable component on Player, healthcontroller will subscribe to idamageable event
            //_healthController?.TakeDamage(Random.Range(10.0f, 20.0f), args.CollisionPoint);
        }

        public void RepairFull()
        {
            _healthController?.RestoreToFullHealth();
        }

        public void RepairPartial(float amount)
        {
            _healthController.Heal(amount);
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
}