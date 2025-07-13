using Cysharp.Threading.Tasks;
using Factories;
using Inventory;
using StaticData.Data;
using UnityEngine;
using Zenject;


namespace UI.Overview
{
    public class OverviewInstructionsContent : MonoBehaviour
    {
        [SerializeField] private Transform instructionsContainer;
        private InstructionsInventory instructionsInventory;
        private GameplayUIFactory gameplayUIFactory;


        [Inject]
        private void Construct(GameplayUIFactory gameplayUIFactory, InstructionsInventory instructionsInventory)
        {
            this.instructionsInventory = instructionsInventory;
            this.gameplayUIFactory = gameplayUIFactory;
        }


        public void Init()
        {
            CreateInstructionsUI().Forget();
        }

        
        private async UniTaskVoid CreateInstructionsUI()
        {
            foreach (InstructionData instructionData in instructionsInventory.Instructions.Keys)
            {
                OverviewInstructionUI createdInstructionUI = await gameplayUIFactory.CreateOverviewInstructionsUI(instructionsContainer);
                createdInstructionUI.Init(instructionData);
            }
        }
    }
}