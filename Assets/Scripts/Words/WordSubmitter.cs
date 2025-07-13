using System;
using GameLoop;
using Input;
using Score;
using Typewriter;
using UnityEngine;
using Words.Data;


namespace Words
{
    public class WordSubmitter : IDisposable
    {
        private readonly InputService inputService;
        private readonly TargetWordService targetWordService;
        private readonly TypedWordValidator typedWordValidator;
        private readonly DeletingProcessService deletingProcessService;
        private readonly MultiplierService multiplierService;
        public event Action<WordSubmissionData> OnWordSubmitted;
        public int WordsTyped { get; private set; }
        public int CorrectWordsTyped { get; private set; }
        public int MaxCorrectWordsStreak { get; private set; }
        private int correctWordsStreak;


        public WordSubmitter(InputService inputService, TargetWordService targetWordService,
            TypedWordValidator typedWordValidator, DeletingProcessService deletingProcessService,
            MultiplierService multiplierService)
        {
            this.inputService = inputService;
            this.targetWordService = targetWordService;
            this.typedWordValidator = typedWordValidator;
            this.deletingProcessService = deletingProcessService;
            this.multiplierService = multiplierService;
        }


        public void Init()
        {
            inputService.OnEnterPressed += SubmitWordAndCollectData;
        }


        public void UnsubscribeFromEvents()
        {
            inputService.OnEnterPressed -= SubmitWordAndCollectData;
        }


        public void ClearLevelStats()
        {
            WordsTyped = 0;
            CorrectWordsTyped = 0;
            MaxCorrectWordsStreak = 0;
            correctWordsStreak = 0;
        }


        private void SubmitWordAndCollectData()
        {
            WordsTyped++;

            int correctChars = 0;
            bool isWordCorrect = typedWordValidator.CurrentWord == targetWordService.TargetWord;

            if (isWordCorrect)
            {
                correctChars = typedWordValidator.CurrentWordCharStates.Count;

                CorrectWordsTyped++;
                correctWordsStreak++;

                if (correctWordsStreak > MaxCorrectWordsStreak)
                {
                    MaxCorrectWordsStreak = correctWordsStreak;
                }
            }
            else
            {
                foreach (bool charState in typedWordValidator.CurrentWordCharStates)
                {
                    if (charState)
                    {
                        correctChars++;
                    }
                }

                deletingProcessService.AddTime(10);
                correctWordsStreak = 0;
            }

            WordSubmissionData wordSubmissionData =
                new WordSubmissionData(correctChars, isWordCorrect, multiplierService.CurrentMultiplier);
            OnWordSubmitted?.Invoke(wordSubmissionData);
            multiplierService.AddValue(wordSubmissionData);
            targetWordService.SetNewTargetWord();
            typedWordValidator.ClearCurrentWord();
            typedWordValidator.ClearCharStates();
        }


        public void Dispose()
        {
            inputService.OnEnterPressed -= SubmitWordAndCollectData;
        }
    }
}
