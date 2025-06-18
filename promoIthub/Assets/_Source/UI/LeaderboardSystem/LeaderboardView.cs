using System;
using System.Collections.Generic;
using System.Linq;
using CoreSystem;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;

namespace UI.LeaderboardSystem
{
    public class LeaderboardView : MonoBehaviour
    {
        [SerializeField] private List<TMP_Text> fields;
        private Queue<TMP_Text> _fieldsQueue;

        private void Start()
        {
            _fieldsQueue = new Queue<TMP_Text>();
            foreach (var field in fields)
            {
                _fieldsQueue.Enqueue(field);
            }
            Bootstrapper.UpdateLeaderboardCommand
                .Subscribe(UpdateLeaderboard).AddTo(this);
        }


        void UpdateLeaderboard(List<PlayerData> newData)
        {
            Debug.Log("Updated");
            foreach (var field in newData)
            {
                if (_fieldsQueue.Count > 0)
                {
                    var fieldText = _fieldsQueue.Dequeue();
                    fieldText.text = $"{field.Name}: {field.Score}";
                    _fieldsQueue.Enqueue(fieldText);
                }
                else
                {
                    Debug.Log("Таблица переполнена");
                    break;
                }
            }
        }
    }
}
