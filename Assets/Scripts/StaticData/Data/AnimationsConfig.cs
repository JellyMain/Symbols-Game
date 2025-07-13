using UnityEngine;


namespace StaticData.Data
{
    [CreateAssetMenu (menuName = "StaticData/AnimationsConfig", fileName = "AnimationsConfig")]
    public class AnimationsConfig : ScriptableObject
    {
        public ScoreAnimations scoreAnimations;
    }
}
