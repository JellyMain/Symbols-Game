using System;
using System.Text;
using Words;


namespace Typewriter
{
    public class TargetWordService
    {
        private readonly WordsProvider wordsProvider;
        private readonly WordSubmitter wordSubmitter;
        public string TargetWord { get; private set; }
        public event Action<string> OnTargetWordUpdated;


        public TargetWordService(WordsProvider wordsProvider)
        {
            this.wordsProvider = wordsProvider;
        }
        

        public void SetNewTargetWord()
        {
            TargetWord = wordsProvider.GetRandomWord();
            OnTargetWordUpdated?.Invoke(TargetWord);
        } 
    }
}
