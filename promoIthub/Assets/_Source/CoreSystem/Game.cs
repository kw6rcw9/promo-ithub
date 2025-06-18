using InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CoreSystem
{
    public class Game: MonoBehaviour
    {
        [Inject] private InputListener _inputListener;
        public void Restart()
        {
            SceneManager.LoadScene(0);
        }

        public void StartGame()
        {
            _inputListener.EnableInput();
        }
    }
}
