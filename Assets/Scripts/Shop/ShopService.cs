using StaticData.Data;
using StaticData.Services;


namespace Shop
{
    public class ShopService
    {
        private InstructionsConfig instructionsConfig;
        
        
        public ShopService(StaticDataService staticDataService)
        {
            instructionsConfig = staticDataService.InstructionsConfig;
        }


        private void SelectRandomInstructions()
        {
            
        }
    }
}
