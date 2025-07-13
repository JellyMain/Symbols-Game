using Assets;
using Zenject;


namespace Factories
{
    public class MetaUIFactory : BaseFactory
    {
        public MetaUIFactory(AssetProvider assetProvider, DiContainer diContainer) :
            base(diContainer, assetProvider) { }


        protected override void WarmUpPrefabs()
        {
            
        }
    }
}