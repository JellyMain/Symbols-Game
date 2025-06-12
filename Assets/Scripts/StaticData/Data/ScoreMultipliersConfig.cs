using System.Collections.Generic;
using Score;
using Sirenix.OdinInspector;
using UnityEngine;


namespace StaticData.Data
{
    [CreateAssetMenu (menuName = "StaticData/ScoreMultipliersConfig", fileName = "ScoreMultipliersConfig")]
    public class ScoreMultipliersConfig : SerializedScriptableObject
    {
        public Dictionary<int, MultiplierData> multipliersConfig;
    }
}