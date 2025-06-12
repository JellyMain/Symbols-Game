using System;
using System.Collections.Generic;
using DG.Tweening;
using Febucci.UI;
using Score;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Words.Data;
using Zenject;


namespace UI
{
    public class CurrentScoreUI : MonoBehaviour
    {
        [SerializeField] private ScoreProgressBarUI scoreProgressBarUI;
        [SerializeField] private CharsScoreUI charsScoreUI;
        [SerializeField] private TMP_Text currentScoreText;
        [SerializeField] private TMP_Text currentScoreSign;
        private ScoreService scoreService;
        private int previousScoreValue;
        private Sequence currentScoreTextSequence;



        [Inject]
        private void Construct(ScoreService scoreService)
        {
            this.scoreService = scoreService;
        }


        private void Start()
        {
            charsScoreUI.OnTotalScoreAnimated += UpdateCurrentScoreText;
            UpdateCurrentScoreText();
        }


        private void OnDisable()
        {
            CleanupActiveAnimations();
        }


        private void CleanupActiveAnimations()
        {
            if (currentScoreTextSequence != null && currentScoreTextSequence.IsActive())
            {
                currentScoreTextSequence.Kill();
                currentScoreTextSequence = null;
            }
        }


        private void UpdateCurrentScoreText()
        {
            if (currentScoreTextSequence != null && currentScoreTextSequence.IsActive())
            {
                currentScoreTextSequence.Kill(true);
            }

            (int value, string unit) formated = StringUtility.FormatMemoryAllocation(scoreService.Score);

            currentScoreTextSequence = DOTween.Sequence();

            currentScoreTextSequence.Append(currentScoreText.DOCounter(previousScoreValue, formated.value, 0.5f, false));
            currentScoreTextSequence.Insert(0.5f, currentScoreSign.transform.DOPunchScale(1 * Vector3.one, 0.1f));
            currentScoreTextSequence.Insert(0.5f, currentScoreText.transform.DOPunchScale(1 * Vector3.one, 0.1f));
            currentScoreSign.text = formated.unit;
            previousScoreValue = scoreService.Score;

            scoreProgressBarUI.UpdateScoreProgressBar();
        }


       
    }
}