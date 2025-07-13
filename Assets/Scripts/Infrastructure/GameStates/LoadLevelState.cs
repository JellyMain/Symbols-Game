using Cysharp.Threading.Tasks;
using Factories;
using Infrastructure.GameStates.Interfaces;
using Infrastructure.Services;
using Levels;
using Progress;
using Scenes;
using Score;
using Typewriter;
using UI;
using UnityEngine;
using Words;


namespace Infrastructure.GameStates
{
    public class LoadLevelState : IGameState
    {
        private readonly GameplayUIFactory gameplayUIFactory;
        private readonly GameStateMachine gameStateMachine;


        public LoadLevelState(GameplayUIFactory gameplayUIFactory, GameStateMachine gameStateMachine)
        {
            this.gameplayUIFactory = gameplayUIFactory;
            this.gameStateMachine = gameStateMachine;
        }


        public async void Enter()
        {
            await CreateLevel();
            gameStateMachine.Enter<GameplayState>();
        }


        private async UniTask CreateLevel()
        {
            gameplayUIFactory.CreateUIParent();
            await gameplayUIFactory.CreateScoreCanvas();
            await gameplayUIFactory.CreateWordsCanvas();
            await gameplayUIFactory.CreateWpmCanvas();
            await gameplayUIFactory.CreateDonutRenderer();
            await gameplayUIFactory.CreateDeletingProcessCanvas();
        }
    }
}
