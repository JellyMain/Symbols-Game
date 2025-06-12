using Factories;
using Infrastructure.GameStates;
using Score;
using Typewriter;
using UnityEngine;
using Words;
using WordsProv;
using Zenject;



namespace Infrastructure.Installers.Scene
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        [SerializeField] private MultiplierService multiplierServicePrefab;
        [SerializeField] private WpmService wpmServicePrefab;
        
        
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
            Container.Bind<GameplayUIFactory>().AsSingle();
        }


        private void BindGameStates()
        {
            Container.Bind<LoadLevelState>().AsSingle().NonLazy();
            Container.Bind<GameLoopState>().AsSingle().NonLazy();
        }
    }
}


