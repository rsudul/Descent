using Descent.Common.Input;
using Descent.Gameplay.Game.Input;
using UnityEngine;

namespace Descent.Gameplay.Game
{
    using SaveSystem = SaveSystem.SaveSystem;

    public class GameController : MonoBehaviour
    {
        private IInputController _inputController;

        private bool _saveGame = false;
        private bool _loadGame = false;

        private void Awake()
        {
            InitializeControllers();
        }

        private void InitializeControllers()
        {
            _inputController = new GameInputController();
        }

        private void Update()
        {
            GetInput();
            ProcessInput();
        }

        private void GetInput()
        {
            _inputController.Refresh();

            _saveGame = _inputController.SaveGame;
            _loadGame = _inputController.LoadGame;
        }

        private void ProcessInput()
        {
            if (_saveGame)
            {
                SaveSystem.Save();
                _saveGame = false;
                return;
            }

            if (_loadGame)
            {
                SaveSystem.Load();
                _loadGame = false;
                return;
            }
        }
    }
}