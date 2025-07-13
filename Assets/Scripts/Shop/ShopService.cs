using System;
using System.Collections.Generic;
using Currency;
using Infrastructure.GameStates;
using Infrastructure.Services;
using Inventory;
using StaticData.Data;
using StaticData.Services;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Shop
{
    public class ShopService
    {
        private readonly CurrencyService currencyService;
        private readonly InstructionsInventory instructionsInventory;
        private readonly GameStateMachine gameStateMachine;
        private readonly InstructionsConfig instructionsConfig;
        public List<InstructionData> SelectedInstructions { get; private set; }


        public ShopService(StaticDataService staticDataService, CurrencyService currencyService,
            InstructionsInventory instructionsInventory, GameStateMachine gameStateMachine)
        {
            this.currencyService = currencyService;
            this.instructionsInventory = instructionsInventory;
            this.gameStateMachine = gameStateMachine;
            instructionsConfig = staticDataService.InstructionsConfig;
        }


        public void SelectRandomInstructions()
        {
            SelectedInstructions = new List<InstructionData>();

            for (int i = 0; i < 4; i++)
            {
                int randomIndex = Random.Range(0, instructionsConfig.instructionsConfig.Count);
                SelectedInstructions.Add(instructionsConfig.instructionsConfig[randomIndex]);
            }
        }


        public bool TryBuyInstruction(InstructionData instructionData)
        {
            if (currencyService.TrySpentTokens(instructionData.cost))
            {
                instructionsInventory.AddInstruction(instructionData);
                return true;
            }

            return false;
        }
        

        public void StartNextLevel()
        {
            gameStateMachine.Enter<LoadLevelState>();
        }
    }
}
