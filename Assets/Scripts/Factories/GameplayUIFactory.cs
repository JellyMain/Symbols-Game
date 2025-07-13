using Assets;
using Const;
using Cysharp.Threading.Tasks;
using UI;
using UnityEngine;
using Zenject;


namespace Factories
{
    public class GameplayUIFactory : BaseFactory
    {
        private Transform uiParent;
        public GameObject DonutRenderer { get; private set; }
        public GameObject WordsCanvas { get; private set; }
        public GameObject ScoreCanvas { get; private set; }
        public GameObject WpmCanvas { get; private set; }
        public GameObject DeleingProcessCanvas { get; private set; }
        public GameObject ShopCanvas { get; private set; }


        public GameplayUIFactory(DiContainer diContainer, AssetProvider assetProvider) : base(diContainer,
            assetProvider) { }


        public void CreateUIParent()
        {
            uiParent = new GameObject("UIParent").transform;
        }


        protected override void WarmUpPrefabs()
        {
            WarmUpPrefab(RuntimeConstants.PrefabAddresses.GAMEPLAY_BACKGROUND_CANVAS);
            WarmUpPrefab(RuntimeConstants.PrefabAddresses.DONUT_RENDERER);
            WarmUpPrefab(RuntimeConstants.PrefabAddresses.WORD_CANVAS);
            WarmUpPrefab(RuntimeConstants.PrefabAddresses.SCORE_CANVAS);
            WarmUpPrefab(RuntimeConstants.PrefabAddresses.WPM_CANVAS);
            WarmUpPrefab(RuntimeConstants.PrefabAddresses.LEVEL_COMPLETED_WINDOW);
            WarmUpPrefab(RuntimeConstants.PrefabAddresses.TRANSITION_CANVAS);
            WarmUpPrefab(RuntimeConstants.PrefabAddresses.SHOP_WINDOW_UI);
            WarmUpPrefab(RuntimeConstants.PrefabAddresses.SHOP_INSTRUCTION_UI);
            WarmUpPrefab(RuntimeConstants.PrefabAddresses.COMMAND_OUTPUT_UI);
            WarmUpPrefab(RuntimeConstants.PrefabAddresses.DELETING_PROCESS_CANVAS);
            WarmUpPrefab(RuntimeConstants.PrefabAddresses.OVERVIEW_WINDOW_UI);
            WarmUpPrefab(RuntimeConstants.PrefabAddresses.OVERVIEW_INSTRUCTION_UI);
        }


        public async UniTask CreateDeletingProcessCanvas()
        {
            DeleingProcessCanvas =
                await InstantiatePrefab(RuntimeConstants.PrefabAddresses.DELETING_PROCESS_CANVAS, uiParent);

            Canvas canvas = DeleingProcessCanvas.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }


        public async UniTask CreateGameplayBackgroundCanvas()
        {
            GameObject spawnedObject =
                await InstantiatePrefab(RuntimeConstants.PrefabAddresses.GAMEPLAY_BACKGROUND_CANVAS, uiParent);

            Canvas canvas = spawnedObject.GetComponent<Canvas>();

            canvas.worldCamera = Camera.main;
        }


        public async UniTask<OverviewInstructionUI> CreateOverviewInstructionsUI(Transform parent)
        {
            return await InstantiatePrefabWithComponent<OverviewInstructionUI>(RuntimeConstants.PrefabAddresses
                .OVERVIEW_INSTRUCTION_UI, parent);
        }


        public async UniTask<OverviewWindowUI> CreateOverviewWindow()
        {
            OverviewWindowUI createdOverviewWindow =
                await InstantiatePrefabWithComponent<OverviewWindowUI>(
                    RuntimeConstants.PrefabAddresses.OVERVIEW_WINDOW_UI, uiParent);

            Canvas canvas = createdOverviewWindow.GetComponent<Canvas>();

            canvas.worldCamera = Camera.main;

            return createdOverviewWindow;
        }


        public async UniTask CreateDonutRenderer()
        {
            DonutRenderer = await InstantiatePrefab(RuntimeConstants.PrefabAddresses.DONUT_RENDERER, uiParent);

            Canvas canvas = DonutRenderer.GetComponent<Canvas>();

            canvas.worldCamera = Camera.main;
        }


        public async UniTask CreateWordsCanvas()
        {
            WordsCanvas = await InstantiatePrefab(RuntimeConstants.PrefabAddresses.WORD_CANVAS, uiParent);

            Canvas canvas = WordsCanvas.GetComponent<Canvas>();

            canvas.worldCamera = Camera.main;
        }


        public async UniTask CreateScoreCanvas()
        {
            ScoreCanvas = await InstantiatePrefab(RuntimeConstants.PrefabAddresses.SCORE_CANVAS, uiParent);

            Canvas canvas = ScoreCanvas.GetComponent<Canvas>();

            canvas.worldCamera = Camera.main;
        }


        public async UniTask CreateWpmCanvas()
        {
            WpmCanvas = await InstantiatePrefab(RuntimeConstants.PrefabAddresses.WPM_CANVAS, uiParent);

            Canvas canvas = WpmCanvas.GetComponent<Canvas>();

            canvas.worldCamera = Camera.main;
        }


        public async UniTask<WinScreenUI> CreateLevelCompletedWindow()
        {
            WinScreenUI spawnedObject =
                await InstantiatePrefabWithComponent<WinScreenUI>(
                    RuntimeConstants.PrefabAddresses.LEVEL_COMPLETED_WINDOW, uiParent);
            Canvas canvas = spawnedObject.GetComponent<Canvas>();

            canvas.worldCamera = Camera.main;

            return spawnedObject;
        }


        public async UniTask<TransitionScreen> CreateTransitionCanvas()
        {
            TransitionScreen spawnedObject =
                await InstantiatePrefabWithComponent<TransitionScreen>(
                    RuntimeConstants.PrefabAddresses.TRANSITION_CANVAS, uiParent);

            Canvas canvas = spawnedObject.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;

            return spawnedObject;
        }


        public async UniTaskVoid CreateShopUI()
        {
            ShopCanvas = await InstantiatePrefab(RuntimeConstants.PrefabAddresses.SHOP_WINDOW_UI, uiParent);
            Canvas canvas = ShopCanvas.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }


        public async UniTask<ShopInstructionUI> CreateInstructionUI(Transform parent)
        {
            return await InstantiatePrefabWithComponent<ShopInstructionUI>(
                RuntimeConstants.PrefabAddresses.SHOP_INSTRUCTION_UI,
                parent);
        }


        public async UniTask<CommandOutputUI> CreateCommandOutputUI(Transform parent)
        {
            return await InstantiatePrefabWithComponent<CommandOutputUI>(RuntimeConstants.PrefabAddresses
                .COMMAND_OUTPUT_UI, parent);
        }
    }
}
