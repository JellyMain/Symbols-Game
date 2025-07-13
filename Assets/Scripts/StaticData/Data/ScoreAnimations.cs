using System;


namespace StaticData.Data
{
    [Serializable]
    public class ScoreAnimations
    {
        public float scaleAppearAnimationTime =0.3f;
        public float punchScaleAppearAnimationTime = 0.2f;
        public float moveToCenterAnimationTime = 0.7f;
        public float multiplierPunchScaleAnimationTime = 0.1f;
        public float totalScorePunchScaleAnimationTime = 0.1f;
        public float scaleDisappearAnimationTime = 0.5f;
        public float rawToFinalScoreCounterAnimationTime = 0.2f;
    }
}
