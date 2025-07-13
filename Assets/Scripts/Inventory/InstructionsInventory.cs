using System;
using System.Collections.Generic;
using StaticData.Data;
using UnityEngine;
using Words;
using Words.Data;


namespace Inventory
{
    public class InstructionsInventory
    {
        private readonly WordSubmitter wordSubmitter;

        public Dictionary<InstructionData, InstructionExecutor> Instructions { get; private set; } =
            new Dictionary<InstructionData, InstructionExecutor>();

        public event Action<WordSubmissionData> OnInstructionsExecuted;


        public InstructionsInventory(WordSubmitter wordSubmitter)
        {
            this.wordSubmitter = wordSubmitter;
        }


        public void Init()
        {
            wordSubmitter.OnWordSubmitted += ExecuteInstructions;
        }

        
        public void UnsubscribeFromEvents()
        {
            wordSubmitter.OnWordSubmitted -= ExecuteInstructions;
        }
        

        public void AddInstruction(InstructionData instructionData)
        {
            InstructionExecutor instructionExecutor = CreateInstructionExecutor(instructionData.instructionName);
            Instructions.Add(instructionData, instructionExecutor);
        }


        private void ExecuteInstructions(WordSubmissionData wordSubmissionData)
        {
            WordSubmissionData modifiedWordSubmissionData = wordSubmissionData;

            foreach (InstructionExecutor instructionExecutor in Instructions.Values)
            {
                modifiedWordSubmissionData = instructionExecutor.Execute(modifiedWordSubmissionData);
            }

            OnInstructionsExecuted?.Invoke(modifiedWordSubmissionData);
        }


        private InstructionExecutor CreateInstructionExecutor(string instructionName)
        {
            InstructionExecutor instructionExecutor = instructionName switch
            {
                "MOV" => new MovExecutor(),
                _ => null
            };
            Debug.Log(instructionName);
            Debug.Log(instructionExecutor);
            return instructionExecutor;
        }
    }
}
