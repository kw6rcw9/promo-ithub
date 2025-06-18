using System;

namespace ScoreSystem
{
    public class Score
    {
        public static int ScoreCount { get;  set; } 
        public static Action<int> ScoreChanged;

        public void IncreaseScore(int num)
        {
            ScoreCount += num;
            ScoreChanged?.Invoke(ScoreCount);
        }
    }
}