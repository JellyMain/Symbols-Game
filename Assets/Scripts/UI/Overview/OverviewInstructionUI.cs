using Instructions;
using StaticData.Data;
using TMPro;
using UnityEngine;


namespace UI
{
    public class OverviewInstructionUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text rarityText;
        [SerializeField] private TMP_Text instructionNameText;
        [SerializeField] private TMP_Text descriptionText;


        public void Init(InstructionData instructionData)
        {
            UpdateInstructionUI(instructionData);;
        }

        
        private void UpdateInstructionUI(InstructionData instructionData)
        {
            rarityText.text = instructionData.rarity switch
            {
                InstructionRarity.Common => "[COMMON]",
                InstructionRarity.Rare => "[RARE]",
                InstructionRarity.Epic => "[EPIC]",
                InstructionRarity.Legendary => "[LEGENDARY]",
                _ => rarityText.text
            };
            
            
            instructionNameText.text = instructionData.instructionName;
            descriptionText.text = instructionData.description;
        }
    }
}
