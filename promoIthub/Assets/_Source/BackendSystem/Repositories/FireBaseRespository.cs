using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using R3;
using Zenject;

namespace BackendSystem.Repositories
{
    public class FireBaseRespository
    {
        
        private const string DATABASE_URL = "https://promoithub-default-rtdb.europe-west1.firebasedatabase.app/leaderboard.json"; 
        // 🚀 Сохранение результата игрока
        public async UniTask SendScoreAsync(string playerName, long score)
        {
            PlayerData data = new PlayerData(playerName, score);
            string json = JsonUtility.ToJson(data);

            UnityWebRequest request = new UnityWebRequest(DATABASE_URL, "POST");
            byte[] body = Encoding.UTF8.GetBytes(json);

            request.uploadHandler = new UploadHandlerRaw(body);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            
            Debug.Log("Отправляем JSON: " + json);
            Debug.Log("POST в URL: " + DATABASE_URL);

            await request.SendWebRequest();
            
            Debug.Log("Ответ сервера: " + request.downloadHandler.text);

            if (request.result != UnityWebRequest.Result.Success)
                Debug.LogError("Ошибка отправки: " + request.error);
            else
                Debug.Log("Счёт успешно отправлен!");
        }
        // 📥 Загрузка топа игроков
        public async UniTask<List<PlayerData>> LoadTopPlayersAsync(int topCount = 10)
        {
            UnityWebRequest request = UnityWebRequest.Get(DATABASE_URL);
            request.downloadHandler = new DownloadHandlerBuffer();

            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка загрузки: " + request.error);
                return new List<PlayerData>();
            }

            string json = request.downloadHandler.text;
            

            // Преобразуем JSON в словарь
            var rawDict = JsonUtility.FromJson<Wrapper>(FixJson(json));
            foreach (var VARIABLE in rawDict.items)
            {
                
            Debug.Log(VARIABLE.Value);
            }
            var result = rawDict.items.Select(kvp => kvp.Value).ToList();

            // Сортируем и возвращаем
            return result.OrderByDescending(p => p.Score).Take(topCount).ToList();
        }
        
        public async UniTask<bool> CheckIfNameExistsAsync(string targetName)
        {
            string url = "https://promoithub-default-rtdb.europe-west1.firebasedatabase.app/leaderboard.json";

            UnityWebRequest request = UnityWebRequest.Get(url);
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка загрузки: " + request.error);
                return false; 
            }

            string json = request.downloadHandler.text;

            Wrapper wrapper = ParseLeaderboardJson(json); 
            Dictionary<string, PlayerData> entries = wrapper.items;

            foreach (var player in entries.Values)
            {
                if (player.Name.Equals(targetName, StringComparison.OrdinalIgnoreCase))
                {
                    return true; // имя уже существует
                }
            }

            return false; // имя не найдено
        }
        public async UniTask<long?> GetScoreByNameAsync(string name)
        {
            string url = "https://promoithub-default-rtdb.europe-west1.firebasedatabase.app/leaderboard.json";

            UnityWebRequest request = UnityWebRequest.Get(url);
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка получения счёта: " + request.error);
                return null;
            }

            string json = request.downloadHandler.text;
            Wrapper wrapper = ParseLeaderboardJson(json);
            Dictionary<string, PlayerData> entries = wrapper.items;

            foreach (var player in entries.Values)
            {
                if (player.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return player.Score;
                }
            }

            Debug.LogWarning($"Игрок с именем '{name}' не найден.");
            return null;
        }

        public async UniTask<bool> ChangePlayerNameAsync(string oldName, string newName)
        {
            string url = "https://promoithub-default-rtdb.europe-west1.firebasedatabase.app/leaderboard.json";

            UnityWebRequest getRequest = UnityWebRequest.Get(url);
            await getRequest.SendWebRequest();

            if (getRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка загрузки данных: " + getRequest.error);
                return false;
            }

            string json = getRequest.downloadHandler.text;
            Wrapper wrapper = ParseLeaderboardJson(json);
            Dictionary<string, PlayerData> entries = wrapper.items;

            foreach (var kvp in entries)
            {
                if (kvp.Value.Name.Equals(oldName, StringComparison.OrdinalIgnoreCase))
                {
                    string key = kvp.Key;
                    long score = kvp.Value.Score;

                    PlayerData updated = new PlayerData(newName, score);
                    string updatedJson = JsonUtility.ToJson(updated);

                    string updateUrl = $"https://promoithub-default-rtdb.europe-west1.firebasedatabase.app/leaderboard/{key}.json";

                    UnityWebRequest putRequest = UnityWebRequest.Put(updateUrl, updatedJson);
                    putRequest.SetRequestHeader("Content-Type", "application/json");

                    await putRequest.SendWebRequest();

                    if (putRequest.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError("Ошибка обновления имени: " + putRequest.error);
                        return false;
                    }

                    Debug.Log($"Имя игрока изменено: '{oldName}' → '{newName}'");
                    return true;
                }
            }

            Debug.LogWarning($"Игрок с именем '{oldName}' не найден.");
            return false;
        }

        public async UniTask UpdateScoreIfHigherAsync(string playerName, long newScore)
        {
    string url = "https://promoithub-default-rtdb.europe-west1.firebasedatabase.app/leaderboard.json";

    UnityWebRequest getRequest = UnityWebRequest.Get(url);
    await getRequest.SendWebRequest();

    if (getRequest.result != UnityWebRequest.Result.Success)
    {
        Debug.LogError("Ошибка загрузки: " + getRequest.error);
        return;
    }

    string json = getRequest.downloadHandler.text;

    // Если база пустая
    if (string.IsNullOrEmpty(json) || json == "null")
    {
        // Проверка на уникальность перед созданием
        if (await CheckIfNameExistsAsync(playerName))
        {
            Debug.LogWarning("Имя уже существует, запись не создана.");
            return;
        }

        await SendScoreAsync(playerName, newScore);
        return;
    }

    // Преобразуем JSON в словарь
    Wrapper wrapper = ParseLeaderboardJson(json);
    Dictionary<string, PlayerData> entries = wrapper.items;

    string playerKey = null;
    long currentScore = -1;

    foreach (var kvp in entries)
    {
        if (kvp.Value.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase))
        {
            playerKey = kvp.Key;
            currentScore = kvp.Value.Score;
            break;
        }
    }

    if (playerKey == null)
    {
        // Проверка на уникальность перед созданием
        if (await CheckIfNameExistsAsync(playerName))
        {
            Debug.LogWarning("Имя уже существует, запись не создана.");
            return;
        }

        await SendScoreAsync(playerName, newScore);
        return;
    }

    if (newScore > currentScore)
    {
        string updateUrl = $"https://promoithub-default-rtdb.europe-west1.firebasedatabase.app/leaderboard/{playerKey}.json";

        PlayerData updatedData = new PlayerData(playerName, newScore);
        string updatedJson = JsonUtility.ToJson(updatedData);

        UnityWebRequest putRequest = UnityWebRequest.Put(updateUrl, updatedJson);
        putRequest.SetRequestHeader("Content-Type", "application/json");

        await putRequest.SendWebRequest();

        if (putRequest.result != UnityWebRequest.Result.Success)
            Debug.LogError("Ошибка обновления: " + putRequest.error);
        else
            Debug.Log("Рекорд обновлён!");
    }
    else
    {
        Debug.Log("Новый счёт не выше текущего — не обновляем.");
    }
}


        private Wrapper ParseLeaderboardJson(string json)
        {
            Wrapper wrapper = new Wrapper();
            wrapper.entries = new List<Entry>();

            if (string.IsNullOrEmpty(json) || json == "null")
                return wrapper;

            json = json.Trim().TrimStart('{').TrimEnd('}');
            if (string.IsNullOrEmpty(json))
                return wrapper;

            string[] entries = json.Split(new[] { "}," }, System.StringSplitOptions.None);

            foreach (var entry in entries)
            {
                int sep = entry.IndexOf(':');
                if (sep < 0) continue;

                string key = entry.Substring(0, sep).Trim().Trim('"');
                string val = entry.Substring(sep + 1).Trim();
                if (!val.EndsWith("}")) val += "}";

                string entryJson = $"{{\"key\":\"{key}\",\"value\":{val}}}";
                Entry parsed = JsonUtility.FromJson<Entry>(entryJson);
                wrapper.entries.Add(parsed);
            }

            return wrapper;
        }

        [System.Serializable]
        private class SimpleWrapper
        {
            public Dictionary<string, object> data;
        }


        // 🔧 Вспомогательные классы и метод для парсинга JSON словаря
        [System.Serializable]
        private class Wrapper
        {
            public List<Entry> entries;

            public Dictionary<string, PlayerData> items => entries.ToDictionary(e => e.key, e => e.value);
        }

        [System.Serializable]
        private class Entry
        {
            public string key;
            public PlayerData value;
        }

        private string FixJson(string json)
        {
            // Превращаем словарь вида { "id1": {...}, "id2": {...} } в формат, который можно парсить Unity JsonUtility
            json = json.Trim().TrimStart('{').TrimEnd('}');
            var entries = json.Split(new[] { "}," }, System.StringSplitOptions.None);

            var fixedEntries = entries.Select(e =>
            {
                int sep = e.IndexOf(':');  
                string key = e.Substring(0, sep).Trim().Trim('"');
                string val = e.Substring(sep + 1).Trim();
                if (!val.EndsWith("}")) val += "}";
                return $"{{\"key\":\"{key}\",\"value\":{val}}}";
            });

            return "{\"entries\":[" + string.Join(",", fixedEntries) + "]}";
        }
    }
}
