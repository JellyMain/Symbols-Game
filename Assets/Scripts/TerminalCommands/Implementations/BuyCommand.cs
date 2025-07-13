using Shop;
using StaticData.Data;


namespace TerminalCommands.Implementations
{
    public class BuyCommand : ICommand
    {
        private readonly ShopService shopService;
        public string Name => "buy";
        public string Description => "Buy a selected instruction, takes one argument: instruction name";
        public string[] Aliases => new string[] { "b", "purchase" };


        public BuyCommand(ShopService shopService)
        {
            this.shopService = shopService;
        }


        public CommandResult Execute(string[] args)
        {
            if (!CanExecute(args))
            {
                CommandResult failureResult = new CommandResult(false,
                    $"> Invalid number of arguments for \"buy\" command. Expected 1, got {args.Length}");

                return failureResult;
            }

            string instructionName = args[0];

            foreach (InstructionData instructionData in shopService.SelectedInstructions)
            {
                if (instructionData.instructionName == instructionName)
                {
                    if (shopService.TryBuyInstruction(instructionData))
                    {
                        CommandResult successfullyBoughtResult =
                            new CommandResult(true, "> Instruction bought successfully");
                        return successfullyBoughtResult;
                    }

                    CommandResult notEnoughTokensResult =
                        new CommandResult(false, "> Not enough tokens to buy this instruction");
                    return notEnoughTokensResult;
                }
            }

            CommandResult instructionWasNotFoundResult =
                new CommandResult(false, $"> Instruction \"{instructionName}\" was not found");
            return instructionWasNotFoundResult;
        }


        public bool CanExecute(string[] args)
        {
            if (args.Length == 1)
            {
                return true;
            }

            return false;
        }
    }
}
