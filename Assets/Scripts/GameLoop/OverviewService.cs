using Factories;
using Input;
using UI;
using UnityEngine;


namespace GameLoop
{
    public class OverviewService
    {
        private readonly GameplayUIFactory gameplayUIFactory;
        private readonly InputService inputService;
        private OverviewWindowUI overviewWindowUI;


        public OverviewService(GameplayUIFactory gameplayUIFactory, InputService inputService)
        {
            this.gameplayUIFactory = gameplayUIFactory;
            this.inputService = inputService;
        }


        public void Init()
        {
            inputService.OnEscapePressed += OpenCloseOverviewWindow;
        }


        public void UnsubscribeFromEvents()
        {
            inputService.OnEscapePressed -= OpenCloseOverviewWindow;
        }


        private async void OpenCloseOverviewWindow()
        {
            if (overviewWindowUI != null)
            {
                Object.Destroy(overviewWindowUI.gameObject);
                return;
            }

            overviewWindowUI = await gameplayUIFactory.CreateOverviewWindow();
        }
    }
}
