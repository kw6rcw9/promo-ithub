using UnityEngine;

namespace BackendSystem
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        public bool IsRestarted { get; private set; } = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SetRestarted(bool value)
        {
            IsRestarted = value;
        }

        public void ResetRestartFlag()
        {
            IsRestarted = false;
        }
    }
}