using ProjectSC.Common;
using ProjectSC.Gameplay.Player.Input;
using ProjectSC.Gameplay.Player.Movement;
using UnityEngine;

namespace ProjectSC.Gameplay.Player
{
    public class Player : Actor
    {
        private IInputController _inputController;

        private float _deltaTime = 0.0f;

        private Vector2 _lookInput = Vector2.zero;
        private Vector2 _moveInput = Vector2.zero;
        private bool _shootInput = false;

        [SerializeField]
        private Transform _playerBody = null;

        [SerializeField]
        private Rigidbody _rigidbody = null;
        [SerializeField]
        private PlayerMovementController _playerMovementController = new PlayerMovementController();

        private void Awake()
        {
            InitializeControllers();

            // this should be put somewhere else
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void InitializeControllers()
        {
            _inputController = new PlayerInputController();

            _playerMovementController.Initialize(_playerBody, _rigidbody);
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

            _moveInput = new Vector2(_inputController.MoveX, _inputController.MoveY);
            _moveInput = _moveInput.normalized;

            _shootInput = _inputController.Shoot;
        }

        private void FeedInputToControllers()
        {
            _playerMovementController.SetPitchAndYaw(_lookInput.x, _lookInput.y);
            _playerMovementController.SetMovementFactors(_moveInput.x, _moveInput.y);
        }

        private void UpdateControllers(float deltaTime)
        {
            _playerMovementController.UpdateLook(_playerBody, deltaTime);
        }

        private void UpdateControllersPhysics(float deltaTime)
        {
            _playerMovementController.UpdateMovement(transform, _rigidbody, deltaTime);
        }
    }
}