using System;
using PlayerSystem.TeleportSystem;
using TMPro;
using UnityEngine;

namespace ScoreSystem
{
    public class ScoreView : MonoBehaviour

    {
        [SerializeField] private TMP_Text bestScore;
        [SerializeField] private TMP_Text finalScore;
        [SerializeField] private TMP_Text text;
        private void OnEnable()
        {
            Score.ScoreChanged += UpdateScore;
            TeleportPlayer.LoseAction += UpdateFinalScore;
        }

        private void OnDisable()
        {
            Score.ScoreChanged -= UpdateScore;
            TeleportPlayer.LoseAction -= UpdateFinalScore;
        }

        void UpdateScore(int score)
        {
            text.text = $"SCORE: {score}";
        }

        void UpdateFinalScore()
        {
            if(PlayerPrefs.HasKey("score"))
            {
                if (PlayerPrefs.GetInt("score") < Score.ScoreCount)
                {
                    PlayerPrefs.SetInt("score", Score.ScoreCount);
                }
            }
            else
            {
                PlayerPrefs.SetInt("score", Score.ScoreCount);
            }
            bestScore.text = PlayerPrefs.GetInt("score").ToString();
            finalScore.text = text.text;
            
        }
        
    }
}
