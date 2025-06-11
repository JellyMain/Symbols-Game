using Assets;
using Zenject;


namespace Factories
{
    public class GameplayUIFactory
    {
        private readonly DiContainer diContainer;
        private readonly AssetProvider assetProvider;


        public GameplayUIFactory(DiContainer diContainer, AssetProvider assetProvider)
        {
            this.diContainer = diContainer;
            this.assetProvider = assetProvider;
        }
        
    }
}