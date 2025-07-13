using Words.Data;


namespace Inventory
{
    public class MovExecutor : InstructionExecutor
    {
        public override WordSubmissionData Execute(WordSubmissionData wordSubmissionData)
        {
            WordSubmissionData modifiedWordSubmissionData = wordSubmissionData;
            modifiedWordSubmissionData.currentMultiplier += 5;
            return modifiedWordSubmissionData;
        }
    }
}
