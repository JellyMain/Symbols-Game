namespace Words.Data
{
    public class WordSubmissionData
    {
        public readonly int correctChars;
        public readonly bool isWordCorrect;
    
        public WordSubmissionData(int correctChars, bool isWordCorrect)
        {
            this.correctChars = correctChars;
            this.isWordCorrect = isWordCorrect;
        }
    }
}
