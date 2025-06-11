using Assets;
using Const;
using Cysharp.Threading.Tasks;
using Progress;
using UnityEngine;
using Zenject;


namespace Factories
{
    public class MetaUIFactory
    {
        private readonly AssetProvider assetProvider;
        private readonly DiContainer diContainer;


        public MetaUIFactory(AssetProvider assetProvider, DiContainer diContainer)
        {
            this.assetProvider = assetProvider;
            this.diContainer = diContainer;
        }
    }
}
