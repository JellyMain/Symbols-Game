using Factories;
using Infrastructure.GameStates.Interfaces;
using Infrastructure.Services;
using Progress;
using Scenes;
using Score;
using Typewriter;
using UnityEngine;
using Words;


namespace Infrastructure.GameStates
{
    public class LoadLevelState : IGameState
    {
        private readonly SceneLoader sceneLoader;
        private readonly GameplayUIFactory gameplayUIFactory;
        private readonly SaveLoadService saveLoadService;
        private readonly GameStateMachine gameStateMachine;
        private readonly TypedWordValidator typedWordValidator;
        private readonly TargetWordService targetWordService;
        private readonly WordSubmitter wordSubmitter;
        private readonly ScoreService scoreService;


        public LoadLevelState(SceneLoader sceneLoader, GameplayUIFactory gameplayUIFactory,
            SaveLoadService saveLoadService, GameStateMachine gameStateMachine, TypedWordValidator typedWordValidator,
            TargetWordService targetWordService, WordSubmitter wordSubmitter,
            ScoreService scoreService)
        {
            this.sceneLoader = sceneLoader;
            this.gameplayUIFactory = gameplayUIFactory;
            this.saveLoadService = saveLoadService;
            this.gameStateMachine = gameStateMachine;
            this.typedWordValidator = typedWordValidator;
            this.targetWordService = targetWordService;
            this.wordSubmitter = wordSubmitter;
            this.scoreService = scoreService;
        }


        public void Enter()
        {
            InitGameplayServices();

            CreateLevel();
            saveLoadService.UpdateProgress();

            gameStateMachine.Enter<GameLoopState>();
        }


        private void InitGameplayServices()
        {
            typedWordValidator.Init();
            targetWordService.Init();
            wordSubmitter.Init();
            scoreService.Init();
        }


        private void CreateLevel() { }
    }
}
