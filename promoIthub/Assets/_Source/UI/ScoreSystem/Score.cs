using System;

namespace ScoreSystem
{
    public class Score
    {
        private int _score;
        public static Action<int> ScoreChanged;

        public void IncreaseScore(int num)
        {
            _score += num;
            ScoreChanged?.Invoke(_score);
        }
    }
}