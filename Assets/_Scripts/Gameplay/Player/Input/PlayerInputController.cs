using Descent.Constants;

namespace Descent.Gameplay.Player.Input
{
    using Input = UnityEngine.Input;

    public class PlayerInputController : IInputController
    {
        private float _lookX = 0.0f;
        private float _lookY = 0.0f;
        private float _banking = 0.0f;

        private float _moveX = 0.0f;
        private float _moveY = 0.0f;

        private bool _shoot = false;

        public float LookX { get { return _lookX; } }
        public float LookY { get { return _lookY; } }
        public float Banking { get { return _banking; } }

        public float MoveX { get { return _moveX; } }
        public float MoveY { get { return _moveY; } }

        public bool Shoot { get { return _shoot; } }

        public void Refresh()
        {
            _lookX = Input.GetAxis(InputConstants.LookHorizontal);
            _lookY = Input.GetAxis(InputConstants.LookVertical);
            _banking = Input.GetAxis(InputConstants.Banking);

            _moveX = Input.GetAxis(InputConstants.MoveHorizontal);
            _moveY = Input.GetAxis(InputConstants.MoveForward);

            _shoot = Input.GetButtonDown(InputConstants.Shoot);
        }
    }
}