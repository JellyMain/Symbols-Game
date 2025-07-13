using System;
using Assets;
using Cysharp.Threading.Tasks;
using Progress;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Scenes
{
    public class SceneLoader
    {
        private readonly LoadingScreen loadingScreen;
        private readonly SaveLoadService saveLoadService;
        private readonly AssetProvider assetProvider;


        public SceneLoader(LoadingScreen loadingScreen, SaveLoadService saveLoadService, AssetProvider assetProvider)
        {
            this.loadingScreen = loadingScreen;
            this.saveLoadService = saveLoadService;
            this.assetProvider = assetProvider;
        }


        public void Load(string sceneName, Action callback = null)
        {
            LoadScene(sceneName, callback).Forget();
        }


        private async UniTaskVoid LoadScene(string sceneName, Action callback)
        {
            loadingScreen.Show();
            saveLoadService.Cleanup();
            assetProvider.Cleanup();

            if (SceneManager.GetActiveScene().name != sceneName)
            {
                AsyncOperation loadNextScene = SceneManager.LoadSceneAsync(sceneName);

                while (!loadNextScene.isDone)
                {
                    await UniTask.Yield();
                }
            }

            callback?.Invoke();
        }
    }
}
