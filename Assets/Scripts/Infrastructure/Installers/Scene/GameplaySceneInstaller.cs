using Currency;
using Factories;
using GameLoop;
using Infrastructure.GameStates;
using Inventory;
using Levels;
using Score;
using Shop;
using StaticData.Data;
using Typewriter;
using UnityEngine;
using Words;
using Zenject;



namespace Infrastructure.Installers.Scene
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        [SerializeField] private MultiplierService multiplierServicePrefab;
        [SerializeField] private WpmService wpmServicePrefab;
        [SerializeField] private DeletingProcessService deletingProcessServicePrefab;


        public override void InstallBindings()
        {
            BindGameStates();
            BindGameplayUIFactory();
            BindTypedWordValidator();
            BindWordsProvider();
            BindScoreService();
            BindTargetWordService();
            BindWordSubmitterService();
            CreateAndBindMultiplierService();
            CreateAndBindWpmService();
            BindShopService();
            BindCameraFactory();
            BindLevelService();
            BindCurrencyService();
            BindShopTerminalService();
            BindTerminalCommandsService();
            BindInstructionsInventory();
            BindTerminalCommandsFactory();
            CreateAndBindDeletingProcessService();
            BindOverviewService();
        }


        private void BindOverviewService()
        {
            Container.Bind<OverviewService>().AsSingle().NonLazy();
        }


        private void CreateAndBindDeletingProcessService()
        {
            Container.Bind<DeletingProcessService>().FromComponentInNewPrefab(deletingProcessServicePrefab).AsSingle()
                .NonLazy();
        }


        private void BindTerminalCommandsFactory()
        {
            Container.BindInterfacesAndSelfTo<TerminalCommandsFactory>().AsSingle().NonLazy();
        }


        private void BindInstructionsInventory()
        {
            Container.Bind<InstructionsInventory>().AsSingle().NonLazy();
        }


        private void BindTerminalCommandsService()
        {
            Container.BindInterfacesAndSelfTo<TerminalCommandsService>().AsSingle().NonLazy();
        }


        private void BindShopTerminalService()
        {
            Container.BindInterfacesAndSelfTo<ShopTerminalService>().AsSingle().NonLazy();
        }


        private void BindCurrencyService()
        {
            Container.Bind<CurrencyService>().AsSingle().NonLazy();
        }


        private void BindLevelService()
        {
            Container.BindInterfacesAndSelfTo<LevelService>().AsSingle().NonLazy();
        }


        private void BindCameraFactory()
        {
            Container.BindInterfacesAndSelfTo<CameraFactory>().AsSingle();
        }


        private void BindShopService()
        {
            Container.Bind<ShopService>().AsSingle().NonLazy();
        }


        private void CreateAndBindWpmService()
        {
            Container.Bind<WpmService>().FromComponentInNewPrefab(wpmServicePrefab).AsSingle().NonLazy();
        }


        private void CreateAndBindMultiplierService()
        {
            Container.Bind<MultiplierService>().FromComponentInNewPrefab(multiplierServicePrefab).AsSingle().NonLazy();
        }


        private void BindWordSubmitterService()
        {
            Container.BindInterfacesAndSelfTo<WordSubmitter>().AsSingle().NonLazy();
        }


        private void BindTargetWordService()
        {
            Container.Bind<TargetWordService>().AsSingle().NonLazy();
        }


        private void BindScoreService()
        {
            Container.BindInterfacesAndSelfTo<ScoreService>().AsSingle().NonLazy();
        }


        private void BindWordsProvider()
        {
            Container.Bind<WordsProvider>().AsSingle().NonLazy();
        }


        private void BindTypedWordValidator()
        {
            Container.BindInterfacesAndSelfTo<TypedWordValidator>().AsSingle().NonLazy();
        }


        private void BindGameplayUIFactory()
        {
            Container.BindInterfacesAndSelfTo<GameplayUIFactory>().AsSingle();
        }


        private void BindGameStates()
        {
            Container.Bind<LoadLevelState>().AsSingle().NonLazy();
            Container.Bind<GameplayState>().AsSingle().NonLazy();
            Container.Bind<LevelWinState>().AsSingle().NonLazy();
            Container.Bind<ShopState>().AsSingle().NonLazy();
        }
    }
}
