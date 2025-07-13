using Words.Data;


namespace Inventory
{
    public abstract class InstructionExecutor
    {
        public abstract WordSubmissionData Execute(WordSubmissionData wordSubmissionData);
    }
}