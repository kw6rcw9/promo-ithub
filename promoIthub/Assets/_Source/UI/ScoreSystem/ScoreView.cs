using System;
using TMPro;
using UnityEngine;

namespace ScoreSystem
{
    public class ScoreView : MonoBehaviour

    {
        [SerializeField] private TMP_Text text;
        private void OnEnable()
        {
            Score.ScoreChanged += UpdateScore;
        }

        private void OnDisable()
        {
            Score.ScoreChanged -= UpdateScore;
        }

        void UpdateScore(int score)
        {
            text.text = $"SCORE: {score}";
        }
    }
}
