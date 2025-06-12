using System.Collections.Generic;
using UnityEngine;


namespace StaticData.Data
{
    [CreateAssetMenu(menuName = "StaticData/Instructions/InstructionsConfig", fileName = "InstructionsConfig")]
    public class InstructionsConfig : ScriptableObject
    {
        public List<InstructionData> instructionsConfig;
    }
}
