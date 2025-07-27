using System;

using UnityEngine;

using Descent.Common.Attributes.Gameplay.Player;
using Descent.Gameplay.Collisions;
using Descent.Gameplay.Entities;
using Descent.Gameplay.Events.Arguments;
using Descent.Gameplay.Player.Animations;
using Descent.Gameplay.Player.Camera;
using Descent.Gameplay.Player.Collisions;
using Descent.Gameplay.Player.Input;
using Descent.Gameplay.Player.Movement;
using Descent.Gameplay.Systems.Durability.Health;
using Descent.Gameplay.Systems.Durability.Repair;
using Descent.Gameplay.Systems.Hostility;
using Descent.Gameplay.Systems.WeaponSystem;
using Descent.Gameplay.Systems.WeaponSystem.Core;

namespace Descent.Gameplay.Player
{
    [IsPlayerObject]
    public class Player : Actor, IRepairable, IWeaponOwner
    {
        private PlayerInputController _inputController;
        private PlayerMovementController _playerMovementController;
        private IHealthController _healthController;
        private WeaponSystemController _playerWeaponSystemController;

        public IHealthController HealthController => _healthController;

        public GameObject GameObject => gameObject;
        public Transform WeaponMountPoint => _weaponMountPoint;

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
        [SerializeField]
        private Transform _weaponMountPoint = null;

        [Header("Settings")]
        [SerializeField]
        private PlayerMovementSettings _playerMovementSettings = null;
        [SerializeField]
        private Faction _faction = null;
        [SerializeField]
        private HealthSettings _healthSettings = null;
        [SerializeField]
        private WeaponsConfig _weaponsConfig = null;

        private void Awake()
        {
            // this should be put somewhere else
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDestroy()
        {
            _inputController?.Dispose();
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
            _playerWeaponSystemController = new WeaponSystemController();

            PlayerMovementController playerMovementController = _playerMovementController as PlayerMovementController;

            playerMovementController.Initialize(_playerBody, _rigidbody, _playerMovementSettings);
            _playerAnimationsController.Initialize(_playerCameraController.CameraTransform);
            _playerCollisionsController.Initialize(_hitController, _rigidbody);
            _playerWeaponSystemController.Initialize(_weaponsConfig, this);

            _playerWeaponSystemController.EquipWeapon(_playerWeaponSystemController.Weapons[0]);

            _inputController.OnFirePressed += OnFirePressed;
            _inputController.OnGearUpPressed += OnGearUpPressed;
            _inputController.OnGearDownPressed += OnGearDownPressed;
            _playerCollisionsController.OnCollision += OnCollision;
        }

        private void Update()
        {
            float deltaTime = GetDeltaTime();

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

        private void FeedInputToControllers()
        {
            _playerMovementController.SetPitchYawAndRoll(_inputController.LookInput.x,
                _inputController.LookInput.y,
                _inputController.BankingInput);

            _playerMovementController.SetMovementFactors(
                _inputController.MoveInput.x,
                _inputController.MoveInput.y,
                _inputController.MoveVerticalInput
                );

            if (Mathf.Abs(_inputController.MoveVerticalInput) <= 0.01f)
            {
                _playerAnimationsController.SetMovementVelocity(new Vector3(
                    _inputController.MoveInput.x,
                    0.0f,
                    _inputController.MoveInput.y
                    ));
            }
        }

        private void UpdateControllers(float deltaTime)
        {
            _playerMovementController.UpdateLook(_playerBody, _rigidbody, Time.deltaTime);
            _playerAnimationsController.UpdateAnimations(deltaTime);
        }

        private void UpdateControllersPhysics(float deltaTime)
        {
            _playerMovementController.UpdateMovement(transform,_rigidbody, deltaTime,
                _playerCollisionsController.IsTouchingWall, _playerCollisionsController.LastCollisionNormal);
        }

        private void OnCollision(object sender, CollisionEventArgs args)
        {
            float impactSpeed = args.CollisionRelativeVelocity.magnitude;

            _playerMovementController.OnCollisionImpact(_rigidbody, impactSpeed, args.CollisionNormal);

            _playerCameraController.Shake(impactSpeed);

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

        private void OnFirePressed(object sender, EventArgs args)
        {
            _playerWeaponSystemController.Fire();
        }

        private void OnGearUpPressed(object sender, EventArgs args)
        {
            _playerMovementController.IncreaseSpeedLevel();
        }

        private void OnGearDownPressed(object sender, EventArgs args)
        {
            _playerMovementController?.DecreaseSpeedLevel();
        }
    }
}