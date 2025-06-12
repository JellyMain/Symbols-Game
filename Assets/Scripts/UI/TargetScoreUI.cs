using System;
using DG.Tweening;
using Score;
using TMPro;
using UnityEngine;
using Utils;
using Zenject;


namespace UI
{
    public class TargetScoreUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text targetScoreText;
        [SerializeField] private TMP_Text targetScoreSign;
        private ScoreService scoreService;


        [Inject]
        private void Construct(ScoreService scoreService)
        {
            this.scoreService = scoreService;
        }
        
        
        private void Start()
        {
            UpdateTargetScoreText();
        }


        private void UpdateTargetScoreText()
        {
            (int value, string unit) formatted = StringUtility.FormatMemoryAllocation(scoreService.TargetScore);
            targetScoreText.DOCounter(0, formatted.value, 0.5f, false);
            targetScoreSign.text = formatted.unit;
        }
    }
}
