using BackendSystem.Repositories;
using CoreSystem;
using UnityEngine;
using Zenject;

namespace BackendSystem.TelegramSystem
{
    public class TelegramInfo : MonoBehaviour
    {
        [Inject] private FireBaseRespository _db;
        public static string firstName = "";

        // Start is called before the first frame update
        public async void JsSendInfo(string _name)
        {
            
            var name = _name;
            if (PlayerPrefs.HasKey("player"))
            {
                if (await _db.CheckIfNameExistsAsync(PlayerPrefs.GetString("player")) && PlayerPrefs.GetString("player") != name)
                {
                     await _db.ChangePlayerNameAsync(PlayerPrefs.GetString("player"), name);
                }
                var score = await _db.GetScoreByNameAsync(name);
                if (score != null)
                {
                    Debug.LogError("score: " + score);
                    PlayerPrefs.SetInt("score", (int)score);
                }
            }
            
            PlayerPrefs.SetString("player", name);
            PlayerPrefs.Save();
            var topPlayers = await _db.LoadTopPlayersAsync();
            Bootstrapper.UpdateLeaderboardCommand?.Execute(topPlayers);
         
  
        }
        // Update is called once per frame
        void Update()
        {
        
        }
    }
    [System.Serializable]
    public struct UserData
    {
        public string firstName;
    }
}
