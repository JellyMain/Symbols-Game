using Const;
using Currency;
using Infrastructure.GameStates;
using Infrastructure.Services;
using Scenes;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;


namespace Cheats
{
    public class CheatsService : MonoBehaviour
    {
        private enum GameState
        {
            BootstrapState,
            LoadProgressState,
            LoadMetaState,
            LoadLevelState,
            GameplayState,
            LevelWinState,
            ShopState
        }


        private GameStateMachine gameStateMachine;
        private SceneLoader sceneLoader;
        private ContainerService containerService;


        [Inject]
        private void Construct(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            ContainerService containerService)
        {
            this.containerService = containerService;
            this.sceneLoader = sceneLoader;
            this.gameStateMachine = gameStateMachine;
        }


        [Button]
        private void AddTokens(int amount)
        {
            CurrencyService currencyService = containerService.LocalContainer.TryResolve<CurrencyService>();

            if (currencyService != null)
            {
                currencyService.AddTokens(amount);
            }
            else
            {
                Debug.LogError("Currency service is not resolved");
            }
        }


        [Button]
        private void EnterGameState(GameState gameState)
        {
            if (gameState == GameState.BootstrapState)
            {
                sceneLoader.Load(RuntimeConstants.Scenes.BOOTSTRAP_SCENE,
                    () => gameStateMachine.Enter<BootstrapState>());
            }
            else if (gameState == GameState.GameplayState)
            {
                sceneLoader.Load(RuntimeConstants.Scenes.GAME_SCENE,
                    () => gameStateMachine.Enter<GameplayState>());
            }
            else if (gameState == GameState.LevelWinState)
            {
                sceneLoader.Load(RuntimeConstants.Scenes.GAME_SCENE,
                    () => gameStateMachine.Enter<LevelWinState>());
            }
            else if (gameState == GameState.LoadLevelState)
            {
                sceneLoader.Load(RuntimeConstants.Scenes.GAME_SCENE,
                    () => gameStateMachine.Enter<LoadLevelState>());
            }
            else if (gameState == GameState.LoadMetaState)
            {
                sceneLoader.Load(RuntimeConstants.Scenes.MAIN_MENU_SCENE,
                    () => gameStateMachine.Enter<LoadMetaState>());
            }
            else if (gameState == GameState.LoadProgressState)
            {
                sceneLoader.Load(RuntimeConstants.Scenes.BOOTSTRAP_SCENE,
                    () => gameStateMachine.Enter<LoadProgressState>());
            }
            else if (gameState == GameState.ShopState)
            {
                sceneLoader.Load(RuntimeConstants.Scenes.GAME_SCENE,
                    () => gameStateMachine.Enter<ShopState>());
            }
        }
    }
}
