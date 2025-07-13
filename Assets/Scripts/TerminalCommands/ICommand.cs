namespace TerminalCommands
{
    public interface ICommand
    {
        public string Name { get; }
        public string Description { get; }
        public string[] Aliases { get; }
        public CommandResult Execute(string[] args);
        public bool CanExecute(string[] args);
    }
}