using System;
using System.Globalization;
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
        [SerializeField] private TMP_Text dialogueText;
        private MultiplierService multiplierService;


        [Inject]
        private void Construct(MultiplierService multiplierService)
        {
            this.multiplierService = multiplierService;
        }


        private void OnEnable()
        {
            multiplierService.OnSliderFilled += UpdateMultiplierText;
        }


        private void OnDisable()
        {
            multiplierService.OnSliderFilled -= UpdateMultiplierText;
        }


        private void Start()
        {
            multiplierText.text = $"{multiplierService.CurrentMultiplier}x";
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
            multiplierText.text = $"{multiplierData.multiplier}x";
            dialogueText.text = multiplierData.dialogueText;
        }
    }
}
