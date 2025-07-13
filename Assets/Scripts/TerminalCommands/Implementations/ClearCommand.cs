using Shop;


namespace TerminalCommands.Implementations
{
    public class ClearCommand : ICommand
    {
        private readonly ShopTerminalService shopTerminalService;
        public string Name => "clear";
        public string Description => "Clears the terminal";
        public string[] Aliases => new string[] { "cl" };



        public ClearCommand(ShopTerminalService shopTerminalService)
        {
            this.shopTerminalService = shopTerminalService;
        }


        public CommandResult Execute(string[] args)
        {
            if (!CanExecute(args))
            {
                CommandResult failureResult = new CommandResult(false,
                    $"> Invalid number of arguments for \"clear\" command. Expected 0, got {args.Length}");

                return failureResult;
            }
            
            shopTerminalService.ClearCommandOutput();

            CommandResult successResult = new CommandResult(true, "> Terminal cleared");
            return successResult;
        }


        public bool CanExecute(string[] args)
        {
            if (args.Length == 0)
            {
                return true;
            }

            return false;
        }
    }
}
