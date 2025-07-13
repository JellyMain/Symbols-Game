using Assets;
using Const;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;


namespace Factories
{
    public class CameraFactory : BaseFactory
    {
        public CameraFactory(AssetProvider assetProvider, DiContainer diContainer) :
            base(diContainer, assetProvider) { }


        public async UniTask<Camera> CreateCamera()
        {
            Transform cameraParent = new GameObject("CameraParent").transform;
            return await InstantiatePrefabWithComponent<Camera>(RuntimeConstants.PrefabAddresses.MAIN_CAMERA,
                cameraParent);
        }


        protected override void WarmUpPrefabs()
        {
            WarmUpPrefab(RuntimeConstants.PrefabAddresses.MAIN_CAMERA);
        }
    }
}
