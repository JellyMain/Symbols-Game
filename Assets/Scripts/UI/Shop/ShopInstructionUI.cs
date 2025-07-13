using Instructions;
using StaticData.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    public class ShopInstructionUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text instructionNameText;
        [SerializeField] private TMP_Text rarityText;
        [SerializeField] private TMP_Text costText;
        private InstructionData instructionData;


        public void Init(InstructionData data)
        {
            instructionData = data;
            UpdateInstructionUI();
        }


        private void UpdateInstructionUI()
        {
            instructionNameText.text = instructionData.instructionName;

            rarityText.text = instructionData.rarity switch
            {
                InstructionRarity.Common => "[COMMON]",
                InstructionRarity.Rare => "[RARE]",
                InstructionRarity.Epic => "[EPIC]",
                InstructionRarity.Legendary => "[LEGENDARY]",
                _ => rarityText.text
            };

            costText.text = instructionData.cost.ToString();
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }
    }
}
