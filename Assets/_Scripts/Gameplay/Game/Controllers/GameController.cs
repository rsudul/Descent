using Descent.Common.Attributes.Gameplay.Player;
using Descent.Gameplay.Game.Interfaces;
using Descent.Common.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Descent.Gameplay.Game.Controllers
{
    using SaveSystem = SaveSystem.SaveSystem;

    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        private IInputController _inputController;

        private bool _saveGame = false;
        private bool _loadGame = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            InitializeGame();
            InitializeControllers();
        }

        private void InitializeGame()
        {
            List<IInitializable> initializables = FindObjectsOfType<MonoBehaviour>(false)
                .OfType<IInitializable>()
                .OrderByDescending(i => i.InitializationPriority)
                .ToList();

            foreach (IInitializable initializable in initializables)
            {
                initializable.Initialize();
            }

            foreach (IInitializable initializable in initializables)
            {
                initializable.LateInitialize();
            }
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

        public bool GetPlayer(out GameObject player)
        {
            player = null;
            GameObject[] allGameObjects = FindObjectsOfType<GameObject>();

            foreach (GameObject go in allGameObjects)
            {
                MonoBehaviour[] monoBehaviours = go.GetComponents<MonoBehaviour>();

                foreach (MonoBehaviour monoBehaviour in monoBehaviours)
                {
                    if (monoBehaviour == null)
                    {
                        continue;
                    }

                    Type type = monoBehaviour.GetType();
                    if (type.GetCustomAttributes(typeof(IsPlayerObjectAttribute), true).Length > 0)
                    {
                        player = go;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}