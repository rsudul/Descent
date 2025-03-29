using Descent.Common.Input;
using Descent.Constants;

namespace Descent.Gameplay.Game.Input
{
    using Input = UnityEngine.Input;

    public class GameInputController : IInputController
    {
        private float _lookX = 0.0f;
        private float _lookY = 0.0f;
        private float _banking = 0.0f;

        private float _moveX = 0.0f;
        private float _moveY = 0.0f;

        private bool _shoot = false;

        private bool _saveGame = false;
        private bool _loadGame = false;

        public float LookX { get { return _lookX; } }
        public float LookY { get { return _lookY; } }
        public float Banking { get { return _banking; } }

        public float MoveX { get { return _moveX; } }
        public float MoveY { get { return _moveY; } }

        public bool Shoot { get { return _shoot; } }

        public bool SaveGame { get { return _saveGame; } }
        public bool LoadGame { get { return _loadGame; } }

        public void Refresh()
        {
            _saveGame = Input.GetButtonDown(InputConstants.SaveGame);
            _loadGame = Input.GetButtonDown(InputConstants.LoadGame);
        }
    }
}