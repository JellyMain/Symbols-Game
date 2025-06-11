using System;
using Input;
using Typewriter;
using Words.Data;


namespace Words
{
    public class WordSubmitter : IDisposable
    {
        private readonly InputService inputService;
        private readonly TargetWordService targetWordService;
        private readonly TypedWordValidator typedWordValidator;
        public event Action<WordSubmissionData> OnWordSubmitted;


        public WordSubmitter(InputService inputService, TargetWordService targetWordService,
            TypedWordValidator typedWordValidator)
        {
            this.inputService = inputService;
            this.targetWordService = targetWordService;
            this.typedWordValidator = typedWordValidator;
        }


        public void Init()
        {
            inputService.OnEnterPressed += SubmitWordAndCollectData;
        }


        private void SubmitWordAndCollectData()
        {
            int correctChars = typedWordValidator.CurrentWordCharStates.Count;
            bool isWordCorrect = typedWordValidator.CurrentWord == targetWordService.TargetWord;

            WordSubmissionData wordSubmissionData = new WordSubmissionData(correctChars, isWordCorrect);
            OnWordSubmitted?.Invoke(wordSubmissionData);

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
