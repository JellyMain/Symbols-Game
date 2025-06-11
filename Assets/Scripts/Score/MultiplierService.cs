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
        private WordSubmitter wordSubmitter;
        private int stage;
        private float multiplier = 1;
        private float fillValue;
        private float maxFillValue;
        private string multiplierText;
        public float NormalizedFillValue => fillValue / maxFillValue;
        public event Action<MultiplierData> OnSliderFilled;



        [Inject]
        private void Construct(StaticDataService staticDataService, WordSubmitter wordSubmitter)
        {
            this.wordSubmitter = wordSubmitter;
            scoreMultipliersConfig = staticDataService.ScoreMultipliersConfig;
        }


        private void Start()
        {
            SetStageData();
            wordSubmitter.OnWordSubmitted += AddValue;
        }


        private void OnDisable()
        {
            wordSubmitter.OnWordSubmitted -= AddValue;
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


        private void AddValue(WordSubmissionData wordSubmissionData)
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

            multiplier = multiplierData.multiplier;
            maxFillValue = multiplierData.valueToFill;
            multiplierText = multiplierData.text;

            fillValue = 0;
            OnSliderFilled?.Invoke(multiplierData);
        }
    }
}
