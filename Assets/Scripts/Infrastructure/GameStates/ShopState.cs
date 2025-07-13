using Factories;
using GameLoop;
using Infrastructure.GameStates.Interfaces;
using Shop;
using StaticData.Data;
using UnityEngine;


namespace Infrastructure.GameStates
{
    public class ShopState : IExitableState
    {
        private readonly GameplayUIFactory gameplayUIFactory;
        private readonly ShopService shopService;
        private readonly ShopTerminalService shopTerminalService;
        private readonly TerminalCommandsService terminalCommandsService;
        private readonly OverviewService overviewService;


        public ShopState(GameplayUIFactory gameplayUIFactory, ShopService shopService,
            ShopTerminalService shopTerminalService, TerminalCommandsService terminalCommandsService,
            OverviewService overviewService)
        {
            this.gameplayUIFactory = gameplayUIFactory;
            this.shopService = shopService;
            this.shopTerminalService = shopTerminalService;
            this.terminalCommandsService = terminalCommandsService;
            this.overviewService = overviewService;
        }


        public void Enter()
        {
            shopService.SelectRandomInstructions();
            shopTerminalService.Init();
            terminalCommandsService.Init();
            gameplayUIFactory.CreateShopUI().Forget();
        }


        public void Exit()
        {
            overviewService.UnsubscribeFromEvents();
            DestroyShopUI();
        }



        private void DestroyShopUI()
        {
            Object.Destroy(gameplayUIFactory.ShopCanvas);
        }
    }
}
