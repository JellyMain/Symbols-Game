namespace TerminalCommands
{
    public class CommandResult
    {
        public bool Result { get; private set; }
        public string Output { get; private set; }
        
        
        public CommandResult(bool result, string output)
        {
            Result = result;
            Output = output;
        }
    }
}
