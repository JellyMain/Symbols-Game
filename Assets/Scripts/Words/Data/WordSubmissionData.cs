namespace Words.Data
{
    public class WordSubmissionData
    {
        public readonly int correctChars;
        public readonly bool isWordCorrect;
        public float currentMultiplier;


        public WordSubmissionData(int correctChars, bool isWordCorrect, float currentMultiplier)
        {
            this.correctChars = correctChars;
            this.isWordCorrect = isWordCorrect;
            this.currentMultiplier = currentMultiplier;
        }
    }
}
