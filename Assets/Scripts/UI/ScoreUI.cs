using System;
using System.Collections.Generic;
using DG.Tweening;
using Febucci.UI;
using Score;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.UI;
using Words.Data;
using Zenject;


namespace UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text progressBarPercentText;
        [SerializeField] private TextAnimator_TMP progressBarFillText;
        [SerializeField] private TMP_Text currentScoreText;
        [SerializeField] private TMP_Text currentScoreSign;
        [SerializeField] private TMP_Text targetScoreText;
        [SerializeField] private TMP_Text targetScoreSign;
        [SerializeField] private TMP_Text multiplierText;
        [SerializeField] private Image rightSlider;
        [SerializeField] private Image leftSlider;
        [SerializeField] private TMP_Text animatedScoreTextPrefab;
        [SerializeField] private Transform spawnedCharScoreParent;
        private ScoreService scoreService;
        private MultiplierService multiplierService;
        private readonly string[] byteUnits = { "b", "kb", "mb", "gb", "tb" };
        private int previousScoreValue;
        private int previousProgressPercent;
        private Sequence activeScoreCharSequence;
        private Sequence activeCombineSequence;
        private Sequence activeScoreTextSequence;
        private List<TMP_Text> activeScoreTexts = new List<TMP_Text>();


        [Inject]
        private void Construct(ScoreService scoreService, MultiplierService multiplierService)
        {
            this.multiplierService = multiplierService;
            this.scoreService = scoreService;
        }


        private void Start()
        {
            scoreService.OnScoreCalculated += CreateScoreChars;
            multiplierService.OnSliderFilled += UpdateMultiplierText;
            UpdateCurrentScoreText();
            UpdateTargetScoreText();
        }


        private void OnDisable()
        {
            scoreService.OnScoreCalculated -= CreateScoreChars;
            multiplierService.OnSliderFilled -= UpdateMultiplierText;

            CleanupActiveAnimations();
        }


        private void CleanupActiveAnimations()
        {
            if (activeScoreCharSequence != null && activeScoreCharSequence.IsActive())
            {
                activeScoreCharSequence.Kill();
                activeScoreCharSequence = null;
            }

            if (activeCombineSequence != null && activeCombineSequence.IsActive())
            {
                activeCombineSequence.Kill();
                activeCombineSequence = null;
            }

            if (activeScoreTextSequence != null && activeScoreTextSequence.IsActive())
            {
                activeScoreTextSequence.Kill();
                activeScoreTextSequence = null;
            }

            foreach (var scoreText in activeScoreTexts)
            {
                if (scoreText != null)
                {
                    Destroy(scoreText.gameObject);
                }
            }


            activeScoreTexts.Clear();
        }


        private void Update()
        {
            UpdateMultiplierSlider();
        }


        private (int value, string unit) FormatMemoryAllocation(int bytes)
        {
            if (bytes <= 0)
            {
                return (0, byteUnits[0]);
            }

            int order = 0;
            int size = bytes;

            while (size >= 1024 && order < byteUnits.Length - 1)
            {
                order++;
                size /= 1024;
            }

            string unit = byteUnits[order];

            return (size, unit);
        }


        private void CreateScoreChars(WordSubmissionData wordSubmissionData)
        {
            CleanupActiveAnimations();

            activeScoreTexts = new List<TMP_Text>();

            activeScoreCharSequence = DOTween.Sequence();

            for (int i = 0; i < wordSubmissionData.correctChars; i++)
            {
                TMP_Text spawnedScoreText = SpawnScoreText(1);
                activeScoreTexts.Add(spawnedScoreText);
                activeScoreCharSequence.Insert(0, spawnedScoreText.DOScale(1, 0.3f).From(0).SetEase(Ease.OutElastic));
                activeScoreCharSequence.Append(spawnedScoreText.transform.DOPunchScale(1 * Vector3.one, 0.2f));
            }

            foreach (TMP_Text spawnedScoreText in activeScoreTexts)
            {
                activeScoreCharSequence.Insert(0.1f * wordSubmissionData.correctChars + 0.3f,
                    spawnedScoreText.transform.DOMove(spawnedCharScoreParent.position, 0.7f).SetEase(Ease.InBack));
            }

            activeScoreCharSequence.OnComplete(() =>
                CombineCharsScoreIntoOne(activeScoreTexts, wordSubmissionData.correctChars));
        }


        private void CombineCharsScoreIntoOne(List<TMP_Text> spawnedScoreTexts, int totalScore)
        {
            if (activeScoreCharSequence == null || !activeScoreCharSequence.IsActive())
            {
                foreach (TMP_Text spawnedScoreText in spawnedScoreTexts)
                {
                    if (spawnedScoreText != null)
                    {
                        Destroy(spawnedScoreText.gameObject);
                    }
                }

                return;
            }

            activeCombineSequence = DOTween.Sequence();

            foreach (TMP_Text spawnedScoreText in spawnedScoreTexts)
            {
                if (spawnedScoreText != null)
                {
                    Destroy(spawnedScoreText.gameObject);
                }
            }

            TMP_Text totalScoreText = SpawnScoreText(totalScore);
            totalScoreText.fontSize += 10;
            activeScoreTexts.Add(totalScoreText);

            activeCombineSequence.Append(multiplierText.transform.DOPunchScale(1 * Vector3.one, 0.1f));
            activeCombineSequence.Append(totalScoreText.transform.DOPunchScale(1 * Vector3.one, 0.1f)
                .OnComplete(UpdateCurrentScoreText));
            activeCombineSequence.Append(totalScoreText.DOScale(0, 0.5f).SetEase(Ease.InBack));

            activeCombineSequence.OnComplete(() =>
            {
                if (totalScoreText != null)
                {
                    Destroy(totalScoreText.gameObject);
                }

                activeScoreTexts.Remove(totalScoreText);
            });
        }


        private TMP_Text SpawnScoreText(int charScore)
        {
            (int value, string unit) formated = FormatMemoryAllocation(charScore);
            TMP_Text spawnedScoreText = Instantiate(animatedScoreTextPrefab, spawnedCharScoreParent);
            spawnedScoreText.transform.localPosition = Vector3.zero;

            spawnedScoreText.text = $"+{formated.value}{formated.unit}";

            return spawnedScoreText;
        }


        private void UpdateCurrentScoreText()
        {
            if (activeScoreTextSequence != null && activeScoreTextSequence.IsActive())
            {
                activeScoreTextSequence.Kill(true);
            }

            (int value, string unit) formated = FormatMemoryAllocation(scoreService.Score);

            activeScoreTextSequence = DOTween.Sequence();

            activeScoreTextSequence.Append(currentScoreText.DOCounter(previousScoreValue, formated.value, 0.5f, false));
            activeScoreTextSequence.Insert(0.5f, currentScoreSign.transform.DOPunchScale(1 * Vector3.one, 0.1f));
            activeScoreTextSequence.Insert(0.5f, currentScoreText.transform.DOPunchScale(1 * Vector3.one, 0.1f));
            currentScoreSign.text = formated.unit;
            previousScoreValue = scoreService.Score;
            
            UpdateScoreProgressBar();
        }



        private void UpdateScoreProgressBar()
        {
            int percentage = (int)((float)scoreService.Score / scoreService.TargetScore * 100);
            progressBarPercentText.DOCounter(previousProgressPercent, percentage, 0.5f);
            previousProgressPercent = percentage;
            
            int progressBarFill = percentage / 10;

            if (progressBarFill > 0)
            {
                string filledSigns = new string('#', progressBarFill);
                string coloredFilledSigns = AddColorTag(filledSigns, "green");
                string emptySigns = new string('.', 10 - progressBarFill);
                
                progressBarFillText.SetText($"{coloredFilledSigns}{emptySigns}"); 
            }
        }
        
        
        private string AddColorTag(string str, string colorName)
        {
            return $"<color={colorName}>{str}</color>";
        }


        private void UpdateTargetScoreText()
        {
            (int value, string unit) formatted = FormatMemoryAllocation(scoreService.TargetScore);
            targetScoreText.DOCounter(0, formatted.value, 0.5f, false);
            targetScoreSign.text = formatted.unit;
        }


        private void UpdateMultiplierSlider()
        {
            rightSlider.fillAmount = multiplierService.NormalizedFillValue;
            leftSlider.fillAmount = multiplierService.NormalizedFillValue;
        }


        private void UpdateMultiplierText(MultiplierData multiplierData)
        {
            multiplierText.text = multiplierData.text;
        }
    }
}
