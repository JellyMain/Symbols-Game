using Assets;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;


namespace Factories
{
    public abstract class BaseFactory: IInitializable
    {
        private readonly DiContainer diContainer;
        private readonly AssetProvider assetProvider;

        
        protected BaseFactory(DiContainer diContainer, AssetProvider assetProvider)
        {
            this.diContainer = diContainer;
            this.assetProvider = assetProvider;
        }
        
        public void Initialize()
        {
            WarmUpPrefabs();
        }
        
        
        protected abstract void WarmUpPrefabs();
        
        
        protected void WarmUpPrefab(string prefabAddress)
        {
            PreloadPrefab(prefabAddress).Forget();
        }


        private async UniTaskVoid PreloadPrefab(string prefabAddress)
        {
            await assetProvider.LoadAsset<GameObject>(prefabAddress);
        }
        
        
        protected async UniTask<GameObject> InstantiatePrefab(string prefabAddress, Transform parent = null)
        {
            GameObject prefab = await assetProvider.LoadAsset<GameObject>(prefabAddress);
            return diContainer.InstantiatePrefab(prefab, parent);
        }

        
        protected async UniTask<T> InstantiatePrefabWithComponent<T>(string prefabAddress, Transform parent = null) 
            where T : Component
        {
            GameObject instance = await InstantiatePrefab(prefabAddress, parent);
            return instance.GetComponent<T>();
        }


        
    }
}
