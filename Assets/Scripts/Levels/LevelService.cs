using System;
using Factories;
using Infrastructure.GameStates;
using Infrastructure.Services;
using Score;
using StaticData.Data;
using StaticData.Services;
using UI;
using Words;


namespace Levels
{
    public class LevelService : IDisposable
    {
        private readonly ScoreService scoreService;
        private readonly GameStateMachine gameStateMachine;
        private readonly LevelsConfig levelsConfig;
        private LevelData levelData;
        private int currentLevel;


        public LevelService(StaticDataService staticDataService, ScoreService scoreService,
            GameStateMachine gameStateMachine)
        {
            this.scoreService = scoreService;
            this.gameStateMachine = gameStateMachine;
            levelsConfig = staticDataService.LevelsConfig;
        }


        public void Init()
        {
            scoreService.OnTargetScoreReached += EnterWinLevelState;
        }



        public void UnsubscribeFromEvents()
        {
            scoreService.OnTargetScoreReached -= EnterWinLevelState;
        }


        public LevelData GetLevelData()
        {
            return levelsConfig.levelsConfig[currentLevel];
        }


        private void EnterWinLevelState()
        {
            gameStateMachine.Enter<LevelWinState>();
        }


        public void SetNextLevel()
        {
            currentLevel++;
        }


        public void Dispose()
        {
            scoreService.OnTargetScoreReached -= EnterWinLevelState;
        }
    }
}
