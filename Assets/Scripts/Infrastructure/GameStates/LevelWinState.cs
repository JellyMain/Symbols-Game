using Factories;
using GameLoop;
using Infrastructure.GameStates.Interfaces;
using Levels;
using Score;
using StaticData.Services;
using UI;
using Words;
using Object = UnityEngine.Object;


namespace Infrastructure.GameStates
{
    public class LevelWinState : IGameState
    {
        private readonly StaticDataService staticDataService;
        private readonly ScoreService scoreService;
        private readonly GameplayUIFactory gameplayUIFactory;
        private readonly WpmService wpmService;
        private readonly WordSubmitter wordSubmitter;
        private readonly LevelService levelService;
        private readonly DeletingProcessService deletingProcessService;
        private readonly MultiplierService multiplierService;
        private readonly TypedWordValidator typedWordValidator;


        public LevelWinState(ScoreService scoreService, GameplayUIFactory gameplayUIFactory, WpmService wpmService,
            WordSubmitter wordSubmitter, LevelService levelService, DeletingProcessService deletingProcessService,
            MultiplierService multiplierService, TypedWordValidator typedWordValidator)
        {
            this.scoreService = scoreService;
            this.gameplayUIFactory = gameplayUIFactory;
            this.wpmService = wpmService;
            this.wordSubmitter = wordSubmitter;
            this.levelService = levelService;
            this.deletingProcessService = deletingProcessService;
            this.multiplierService = multiplierService;
            this.typedWordValidator = typedWordValidator;
            this.scoreService = scoreService;
            this.gameplayUIFactory = gameplayUIFactory;
            this.wpmService = wpmService;
            this.wordSubmitter = wordSubmitter;
        }


        public void Enter()
        {
            LevelCompleted();
        }


        private void ResetLevelVariables()
        {
            wordSubmitter.ClearLevelStats();
            typedWordValidator.ResetIsFirstCharTyped();
            multiplierService.ResetMultiplier();
            wpmService.StopTracking();
            scoreService.ResetScore();
            deletingProcessService.ResetTimer();
        }


        private async void LevelCompleted()
        {
            TransitionScreen transitionScreen = await gameplayUIFactory.CreateTransitionCanvas();

            await transitionScreen.FadeIn();

            DestroyGameplayUI();

            levelService.SetNextLevel();

            LevelStats levelStats = new LevelStats()
            {
                correctWords = wordSubmitter.CorrectWordsTyped,
                maxCorrectWordsStreak = wordSubmitter.MaxCorrectWordsStreak,
                maxWpm = wpmService.MaxWpm,
                memoryAllocated = scoreService.Score,
                wordsTyped = wordSubmitter.WordsTyped
            };

            LevelRewards levelRewards = scoreService.CalculateLevelRewards(levelStats);

            WinScreenUI winScreenUI = await gameplayUIFactory.CreateLevelCompletedWindow();

            winScreenUI.SetLevelStats(levelStats);
            winScreenUI.SetLevelRewards(levelRewards);

            ResetLevelVariables();
        }



        private void DestroyGameplayUI()
        {
            Object.Destroy(gameplayUIFactory.DonutRenderer);
            Object.Destroy(gameplayUIFactory.ScoreCanvas);
            Object.Destroy(gameplayUIFactory.WordsCanvas);
            Object.Destroy(gameplayUIFactory.WpmCanvas);
            Object.Destroy(gameplayUIFactory.DeleingProcessCanvas);
        }
    }
}
