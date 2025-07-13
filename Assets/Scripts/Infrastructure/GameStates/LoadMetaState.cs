using Factories;
using Infrastructure.GameStates.Interfaces;
using Progress;
using UI;
using UnityEngine;


namespace Infrastructure.GameStates
{
    public class LoadMetaState : IGameState
    {
        private readonly SaveLoadService saveLoadService;
        private readonly MetaUIFactory metaUIFactory;
        private readonly LoadingScreen loadingScreen;


        public LoadMetaState(SaveLoadService saveLoadService, MetaUIFactory metaUIFactory, LoadingScreen loadingScreen)
        {
            this.saveLoadService = saveLoadService;
            this.metaUIFactory = metaUIFactory;
            this.loadingScreen = loadingScreen;
        }


        public void Enter()
        {
            CreateMeta();
            saveLoadService.UpdateProgress();
            
            loadingScreen.Hide();
        }



        private void CreateMeta() { }
    }
}