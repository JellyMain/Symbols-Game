using System;
using DG.Tweening;
using Febucci.UI;
using Score;
using TMPro;
using UnityEngine;
using Utils;
using Zenject;


namespace UI
{
    public class ScoreProgressBarUI : MonoBehaviour
    {
        [SerializeField] private TextAnimator_TMP progressBarFillText;
        [SerializeField] private TMP_Text progressBarPercentText;
        private ScoreService scoreService;
        private int previousProgressPercent;


        [Inject]
        private void Construct(ScoreService scoreService)
        {
            this.scoreService = scoreService;
        }


        public void UpdateScoreProgressBar()
        {
            if (scoreService.TargetScore != 0)
            {
                int percentage = (int)((float)scoreService.Score / scoreService.TargetScore * 100);
                progressBarPercentText.DOCounter(previousProgressPercent, percentage, 0.5f).SetLink(gameObject);
                previousProgressPercent = percentage;

                int progressBarFill = Mathf.Clamp(percentage / 10, 0, 10);
                
                string filledSigns = new string('#', progressBarFill);
                string coloredFilledSigns = StringUtility.AddColorTag(filledSigns, "green");
                string emptySigns = new string('.', 10 - progressBarFill);

                progressBarFillText.SetText($"{coloredFilledSigns}{emptySigns}");
            }
        }
    }
}