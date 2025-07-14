using System;
using System.Threading.Tasks;
using BackendSystem;
using InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CoreSystem
{
    public class Game: MonoBehaviour
    {
        [Inject] private InputListener _inputListener;
        [SerializeField] private GameObject mainCanvas;
        [SerializeField] private GameObject player;
        public void MainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void RestartGame()
        {
            GameStateManager.Instance.SetRestarted(true);
            SceneManager.LoadScene(0);
           
        }

        

        public void StartGame()
        {
            _inputListener.EnableInput();
        }
    }
}
