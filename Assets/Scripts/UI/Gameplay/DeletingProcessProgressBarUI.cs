using System;
using DG.Tweening;
using Febucci.UI;
using GameLoop;
using TMPro;
using UnityEngine;
using Zenject;


namespace UI
{
    public class DeletingProcessProgressBarUI : MonoBehaviour
    {
        [SerializeField] private TextAnimator_TMP progressBarFillText;
        [SerializeField] private TMP_Text progressBarPercentText;
        private DeletingProcessService deletingProcessService;


        [Inject]
        private void Construct(DeletingProcessService deletingProcessService)
        {
            this.deletingProcessService = deletingProcessService;
        }


        private void Update()
        {
            UpdateProgressBar();
        }


        private void UpdateProgressBar()
        {
            int percentage = (int)(deletingProcessService.NormalizedProgress * 100);
            int progressBarFill = Mathf.Clamp(percentage / 5, 0, 20);

            string filledSigns = new string('█', progressBarFill);
            string emptySigns = new string('░', 20 - progressBarFill);

            progressBarPercentText.text = percentage.ToString();
            progressBarFillText.SetText($"{filledSigns}{emptySigns}");
        }
    }
}
