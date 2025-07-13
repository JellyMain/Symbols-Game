using System;
using StaticData.Data;
using StaticData.Services;
using UnityEngine;
using Words;
using Words.Data;
using Zenject;


namespace Score
{
    public class MultiplierService : MonoBehaviour
    {
        [SerializeField] private float sliderSpeed = 1;
        private ScoreMultipliersConfig scoreMultipliersConfig;
        private int stage;
        public float CurrentMultiplier { get; private set; } = 1;
        private float fillValue;
        private float maxFillValue;
        public float NormalizedFillValue => fillValue / maxFillValue;
        public event Action<MultiplierData> OnSliderFilled;



        [Inject]
        private void Construct(StaticDataService staticDataService)
        {
            scoreMultipliersConfig = staticDataService.ScoreMultipliersConfig;
        }


        private void Start()
        {
            SetStageData();
        }
        

        private void Update()
        {
            if (fillValue < maxFillValue)
            {
                fillValue -= Time.deltaTime * sliderSpeed;

                if (fillValue < 0)
                {
                    fillValue = 0;
                }
            }
            else
            {
                stage++;
                SetStageData();
            }
        }


        public void AddValue(WordSubmissionData wordSubmissionData)
        {
            if (wordSubmissionData.isWordCorrect)
            {
                int wordScore = wordSubmissionData.correctChars;
                fillValue += wordScore;
            }
        }


        private void SetStageData()
        {
            MultiplierData multiplierData = scoreMultipliersConfig.multipliersConfig[stage];

            CurrentMultiplier = multiplierData.multiplier;
            maxFillValue = multiplierData.valueToFill;

            fillValue = 0;
            OnSliderFilled?.Invoke(multiplierData);
        }


        public void ResetMultiplier()
        {
            CurrentMultiplier = 1;
            stage = 0;
        }
    }
}
