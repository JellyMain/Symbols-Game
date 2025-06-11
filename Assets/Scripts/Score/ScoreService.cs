using System;
using Typewriter;
using Words;
using Words.Data;


namespace Score
{
    public class ScoreService : IDisposable
    {
        private readonly WordSubmitter wordSubmitter;
        public int Score { get; private set; }
        public int TargetScore { get; private set; } = 100;
        private readonly int currentMultiplier = 1;
        public event Action<WordSubmissionData> OnScoreCalculated;
        public event Action OnTargetScoreReached;


        public ScoreService(WordSubmitter wordSubmitter)
        {
            this.wordSubmitter = wordSubmitter;
        }


        public void Init()
        {
            wordSubmitter.OnWordSubmitted += CalculateScore;
        }


        private void CalculateScore(WordSubmissionData wordSubmissionData)
        {
            int calculatedScore = wordSubmissionData.correctChars * currentMultiplier;
            Score += calculatedScore;
            OnScoreCalculated?.Invoke(wordSubmissionData);

            if (Score >= TargetScore)
            {
                OnTargetScoreReached?.Invoke();
            }
        }


        public void Dispose()
        {
            wordSubmitter.OnWordSubmitted -= CalculateScore;
        }
    }
}
