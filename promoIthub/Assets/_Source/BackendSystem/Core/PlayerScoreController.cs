using System;
using BackendSystem.Repositories;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BackendSystem.Core
{
    public class PlayerScoreController 
    {
        [Inject] private FireBaseRespository _db;
        
        public async UniTask<bool> CheckName(string name)
        {
            return await _db.CheckIfNameExistsAsync(name);
        }
        public async void SendRecord(long score,PlayerData data = null )
        {
           
           
            if (PlayerPrefs.HasKey("player"))
            {
                try
                {
                    await _db.UpdateScoreIfHigherAsync(PlayerPrefs.GetString("player"), score);

                }
                catch (Exception e)
                {
                    Debug.Log("Ошибка при отправке рекорда: " + e.Message);
                }
            }
            else
            {
                PlayerPrefs.SetString("player", data?.Name);
                await _db.UpdateScoreIfHigherAsync(data?.Name, score);
            }
        }
    }
}
