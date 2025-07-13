using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace StaticData.Data
{
    [CreateAssetMenu(menuName = "StaticData/LevelsConfig", fileName = "LevelsConfig")]
    public class LevelsConfig : SerializedScriptableObject
    {
        public Dictionary<int, LevelData> levelsConfig;
    }
}