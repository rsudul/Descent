using ProjectSC.Constants;

namespace ProjectSC.Gameplay.Player.Input
{
    using Input = UnityEngine.Input;

    public class PlayerInputController : IInputController
    {
        private float _lookX = 0.0f;
        private float _lookY = 0.0f;

        private float _moveX = 0.0f;
        private float _moveY = 0.0f;

        public float LookX { get { return _lookX; } }
        public float LookY { get { return _lookY; } }

        public float MoveX { get { return _moveX; } }
        public float MoveY { get { return _moveY; } }

        public void Refresh()
        {
            _lookX = Input.GetAxis(InputConstants.LookHorizontal);
            _lookY = Input.GetAxis(InputConstants.LookVertical);

            _moveX = Input.GetAxis(InputConstants.MoveHorizontal);
            _moveY = Input.GetAxis(InputConstants.MoveForward);
        }
    }
}