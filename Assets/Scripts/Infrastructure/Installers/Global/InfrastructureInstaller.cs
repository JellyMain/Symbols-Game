using Assets;
using DG.Tweening;
using Infrastructure.Services;
using Input;
using Progress;
using Scenes;
using StaticData.Services;
using Typewriter;
using UI;
using UnityEngine;
using Zenject;


namespace Infrastructure.Installers.Global
{
    public class InfrastructureInstaller : MonoInstaller
    {
        [SerializeField] private LoadingScreen loadingScreenPrefab;


        public override void InstallBindings()
        {
            DOTween.Init();
            CreateAndBindLoadingScreen();
            BindSceneLoader();
            BindAssetProvider();
            BindContainerService();
            BindStaticDataService();
            BindSaveLoadService();
            BindPersistentPlayerProgress();
            BindLocalContainerPasser();
            BindInputService();
        }


        private void BindInputService()
        {
            Container.Bind<InputService>().AsSingle();
        }


        private void BindPersistentPlayerProgress()
        {
            Container.Bind<PersistentPlayerProgress>().AsSingle();
        }


        private void BindSaveLoadService()
        {
            Container.Bind<SaveLoadService>().AsSingle().NonLazy();
        }


        private void BindStaticDataService()
        {
            Container.Bind<StaticDataService>().AsSingle().NonLazy();
        }


        private void BindContainerService()
        {
            Container.Bind<ContainerService>().AsSingle().NonLazy();
        }


        private void BindAssetProvider()
        {
            Container.BindInterfacesAndSelfTo<AssetProvider>().AsSingle().NonLazy();
        }


        private void BindSceneLoader()
        {
            Container.Bind<SceneLoader>().AsSingle();
        }


        private void CreateAndBindLoadingScreen()
        {
            Container.Bind<LoadingScreen>().FromComponentInNewPrefab(loadingScreenPrefab).AsSingle().NonLazy();
        }


        private void BindLocalContainerPasser()
        {
            Container.Bind<LocalContainerPasser>().AsSingle().CopyIntoDirectSubContainers().NonLazy();
        }
    }
}
