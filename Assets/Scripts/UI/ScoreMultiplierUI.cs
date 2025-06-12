using System;
using Score;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace UI
{
    public class ScoreMultiplierUI : MonoBehaviour
    {
        [SerializeField] private Image rightProgressBar;
        [SerializeField] private Image leftProgressBar;
        [SerializeField] private TMP_Text multiplierText;
        private MultiplierService multiplierService;


        [Inject]
        private void Construct(MultiplierService multiplierService)
        {
            this.multiplierService = multiplierService;
        }


        private void Start()
        {
            multiplierService.OnSliderFilled += UpdateMultiplierText;
        }


        private void OnDisable()
        {
            multiplierService.OnSliderFilled -= UpdateMultiplierText;
        }


        private void Update()
        {
            UpdateMultiplierSlider();
        }


        private void UpdateMultiplierSlider()
        {
            rightProgressBar.fillAmount = multiplierService.NormalizedFillValue;
            leftProgressBar.fillAmount = multiplierService.NormalizedFillValue;
        }


        private void UpdateMultiplierText(MultiplierData multiplierData)
        {
            multiplierText.text = multiplierData.text;
        }
    }
}
