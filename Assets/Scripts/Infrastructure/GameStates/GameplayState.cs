using GameLoop;
using Infrastructure.GameStates.Interfaces;
using Inventory;
using Levels;
using Progress;
using Score;
using StaticData.Data;
using Typewriter;
using UI;
using Words;


namespace Infrastructure.GameStates
{
    public class GameplayState : IExitableState
    {
        private readonly TargetWordService targetWordService;
        private readonly ScoreService scoreService;
        private readonly LevelService levelService;
        private readonly LoadingScreen loadingScreen;
        private readonly TypedWordValidator typedWordValidator;
        private readonly WordSubmitter wordSubmitter;
        private readonly SaveLoadService saveLoadService;
        private readonly OverviewService overviewService;
        private readonly InstructionsInventory instructionsInventory;
        private readonly DeletingProcessService deletingProcessService;


        public GameplayState(TargetWordService targetWordService, ScoreService scoreService, LevelService levelService,
            LoadingScreen loadingScreen, TypedWordValidator typedWordValidator, WordSubmitter wordSubmitter,
            SaveLoadService saveLoadService, OverviewService overviewService,
            InstructionsInventory instructionsInventory, DeletingProcessService deletingProcessService)
        {
            this.targetWordService = targetWordService;
            this.scoreService = scoreService;
            this.levelService = levelService;
            this.loadingScreen = loadingScreen;
            this.typedWordValidator = typedWordValidator;
            this.wordSubmitter = wordSubmitter;
            this.saveLoadService = saveLoadService;
            this.overviewService = overviewService;
            this.instructionsInventory = instructionsInventory;
            this.deletingProcessService = deletingProcessService;
        }


        public void Enter()
        {
            InitGameplayServices();
            PrepareLevel();
            saveLoadService.UpdateProgress();
            loadingScreen.Hide();
        }


        public void Exit()
        {
            levelService.UnsubscribeFromEvents();
            typedWordValidator.UnsubscribeFromEvents();
            wordSubmitter.UnsubscribeFromEvents();
            scoreService.UnsubscribeFromEvents();
            instructionsInventory.UnsubscribeFromEvents();
        }


        private void InitGameplayServices()
        {
            overviewService.Init();
            levelService.Init();
            typedWordValidator.Init();
            wordSubmitter.Init();
            scoreService.Init();
            instructionsInventory.Init();
        }


        private void PrepareLevel()
        {
            LevelData levelData = levelService.GetLevelData();

            targetWordService.SetNewTargetWord();
            scoreService.SetTargetScore(levelData.targetScore);
            deletingProcessService.SetTimeToDelete(levelData.timeToDelete);;
        }
    }
}
