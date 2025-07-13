using Shop;
using TerminalCommands.Implementations;
using Zenject;


namespace Factories
{
    public class TerminalCommandsFactory
    {
        private readonly DiContainer diContainer;
        private readonly ShopService shopService;
        private readonly ShopTerminalService shopTerminalService;


        public TerminalCommandsFactory(DiContainer diContainer)
        {
            this.diContainer = diContainer;
        }


        public BuyCommand CreateBuyCommand()
        {
            return diContainer.Instantiate<BuyCommand>();
        }


        public ClearCommand CreateClearCommand()
        {
            return diContainer.Instantiate<ClearCommand>();
        }
    }
}
