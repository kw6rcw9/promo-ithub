using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoreSystem
{
    public class Game: MonoBehaviour
    {
        public void Restart()
        {
            SceneManager.LoadScene(0);
        }
    }
}
