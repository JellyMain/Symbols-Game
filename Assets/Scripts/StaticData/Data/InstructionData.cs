using Instructions;
using UnityEngine;


namespace StaticData.Data
{
    [CreateAssetMenu(menuName = "StaticData/Instructions/InstructionData", fileName = "InstructionData")]
    public class InstructionData : ScriptableObject
    {
        public string instructionName;
        public InstructionRarity rarity;
        public int cost;
    }
}