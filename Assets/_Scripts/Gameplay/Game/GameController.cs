using Descent.Common.Attributes.Gameplay.Player;
using Descent.Gameplay.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Descent.Gameplay.Game
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        private List<IInitializable> _globalInitializables = new List<IInitializable>();

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

            _globalInitializables.Add(InputManager.Instance);

            InitializeGame();
        }

        private void InitializeGame()
        {
            List<IInitializable> initializables = FindObjectsOfType<MonoBehaviour>(false)
                .OfType<IInitializable>()
                .ToList();

            initializables.AddRange(_globalInitializables);

            initializables = initializables
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