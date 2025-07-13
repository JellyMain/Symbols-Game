using System;
using Inventory;
using Levels;
using Typewriter;
using UnityEngine;
using Words;
using Words.Data;


namespace Score
{
    public class ScoreService : IDisposable
    {
        private readonly InstructionsInventory instructionsInventory;
        public int Score { get; private set; }
        public int TargetScore { get; private set; }
        public event Action<WordSubmissionData> OnScoreCalculated;
        public event Action OnTargetScoreUpdated;
        public event Action OnTargetScoreReached;
        private const float CORRECT_WORD_REWARD_FACTOR = 0.2f;
        private const float MAX_STREAK_REWARD_FACTOR = 0.5f;
        private const float MEMORY_ALLOCATION_REWARD_FACTOR = 0.01f;
        private const float WORDS_TYPED_REWARD_FACTOR = 0.1f;
        private const float MAX_WPM_REWARD_FACTOR = 0.01f;



        public ScoreService(InstructionsInventory instructionsInventory)
        {
            this.instructionsInventory = instructionsInventory;
        }


        public void Init()
        {
            instructionsInventory.OnInstructionsExecuted += CalculateScore;
        }


        public void UnsubscribeFromEvents()
        {
            instructionsInventory.OnInstructionsExecuted -= CalculateScore;
        }


        public void SetTargetScore(int targetScore)
        {
            TargetScore = targetScore;
            OnTargetScoreUpdated?.Invoke();
        }


        private void CalculateScore(WordSubmissionData wordSubmissionData)
        {
            int calculatedScore = (int)(wordSubmissionData.correctChars * wordSubmissionData.currentMultiplier);
            Score += calculatedScore;
            OnScoreCalculated?.Invoke(wordSubmissionData);

            if (Score >= TargetScore)
            {
                OnTargetScoreReached?.Invoke();
            }
        }


        public LevelRewards CalculateLevelRewards(LevelStats levelStats)
        {
            int correctWordsReward = Mathf.RoundToInt(levelStats.correctWords * CORRECT_WORD_REWARD_FACTOR);
            int maxCorrectWordsStreakReward =
                Mathf.RoundToInt(levelStats.maxCorrectWordsStreak * MAX_STREAK_REWARD_FACTOR);
            int memoryAllocatedReward = Mathf.RoundToInt(levelStats.memoryAllocated * MEMORY_ALLOCATION_REWARD_FACTOR);
            int wordsTypedReward = Mathf.RoundToInt(levelStats.wordsTyped * WORDS_TYPED_REWARD_FACTOR);
            int maxWpmReward = Mathf.RoundToInt(levelStats.maxWpm * MAX_WPM_REWARD_FACTOR);
            int totalReward = correctWordsReward + maxCorrectWordsStreakReward + memoryAllocatedReward +
                              wordsTypedReward + maxWpmReward;

            LevelRewards levelRewards = new LevelRewards()
            {
                correctWordsReward = correctWordsReward,
                maxCorrectWordsStreakReward = maxCorrectWordsStreakReward,
                memoryAllocatedReward = memoryAllocatedReward,
                wordsTypedReward = wordsTypedReward,
                maxWpmReward = maxWpmReward,
                totalReward = totalReward
            };

            return levelRewards;
        }



        public void ResetScore()
        {
            Score = 0;
        }


        public void Dispose()
        {
            instructionsInventory.OnInstructionsExecuted -= CalculateScore;
        }
    }
}
