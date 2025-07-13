using System;
using System.Collections.Generic;
using DG.Tweening;
using Score;
using StaticData.Data;
using StaticData.Services;
using TMPro;
using UnityEngine;
using Utils;
using Words.Data;
using Zenject;


namespace UI
{
    public class CharsScoreUI : MonoBehaviour
    {
        [SerializeField] private Transform spawnedCharScoreParent;
        [SerializeField] private TMP_Text animatedScoreTextPrefab;
        [SerializeField] private TMP_Text multiplierText;
        private Sequence activeScoreCharSequence;
        private Sequence activeCombineSequence;
        private List<TMP_Text> activeScoreTexts = new List<TMP_Text>();
        private ScoreAnimations scoreAnimations;
        private ScoreService scoreService;
        public event Action OnTotalScoreAnimated;


        [Inject]
        private void Construct(ScoreService scoreService, StaticDataService staticDataService)
        {
            this.scoreService = scoreService;
            scoreAnimations = staticDataService.AnimationsConfig.scoreAnimations;
        }


        private void OnEnable()
        {
            scoreService.OnScoreCalculated += CreateScoreChars;
        }


        private void OnDisable()
        {
            scoreService.OnScoreCalculated -= CreateScoreChars;
        }


        private TMP_Text SpawnScoreText(int charScore)
        {
            (int value, string unit) formated = StringUtility.FormatMemoryAllocation(charScore);
            TMP_Text spawnedScoreText = Instantiate(animatedScoreTextPrefab, spawnedCharScoreParent);
            spawnedScoreText.transform.localPosition = Vector3.zero;

            spawnedScoreText.text = $"+{formated.value}{formated.unit}";

            return spawnedScoreText;
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

            foreach (var scoreText in activeScoreTexts)
            {
                if (scoreText != null)
                {
                    Destroy(scoreText.gameObject);
                }
            }


            activeScoreTexts.Clear();
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
                activeScoreCharSequence.Insert(0,
                    spawnedScoreText.DOScale(1, scoreAnimations.scaleAppearAnimationTime).From(0)
                        .SetEase(Ease.OutElastic));
                activeScoreCharSequence.Append(spawnedScoreText.transform.DOPunchScale(1 * Vector3.one,
                    scoreAnimations.punchScaleAppearAnimationTime));
            }

            foreach (TMP_Text spawnedScoreText in activeScoreTexts)
            {
                activeScoreCharSequence.Insert(
                    scoreAnimations.punchScaleAppearAnimationTime * wordSubmissionData.correctChars +
                    scoreAnimations.scaleAppearAnimationTime,
                    spawnedScoreText.transform
                        .DOMove(spawnedCharScoreParent.position, scoreAnimations.moveToCenterAnimationTime)
                        .SetEase(Ease.InBack));
            }

            activeScoreCharSequence.SetLink(gameObject).OnComplete(() =>
                CombineCharsScoreIntoOne(activeScoreTexts, wordSubmissionData));
        }



        private void CombineCharsScoreIntoOne(List<TMP_Text> spawnedScoreTexts, WordSubmissionData wordSubmissionData)
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

            int rawWordScore = wordSubmissionData.correctChars;
            int finalMultipliedScore = (int)(wordSubmissionData.correctChars * wordSubmissionData.currentMultiplier);

            TMP_Text totalScoreText = SpawnScoreText(rawWordScore);
            totalScoreText.fontSize += 10;
            activeScoreTexts.Add(totalScoreText);


            activeCombineSequence.Append(multiplierText.transform.DOPunchScale(1 * Vector3.one,
                scoreAnimations.multiplierPunchScaleAnimationTime));
            activeCombineSequence.Append(totalScoreText.transform
                .DOPunchScale(1 * Vector3.one, scoreAnimations.totalScorePunchScaleAnimationTime)
                .OnComplete(() =>
                {
                    (int value, string unit) formated = StringUtility.FormatMemoryAllocation(finalMultipliedScore);
                    totalScoreText.text = $"+{finalMultipliedScore}{formated.unit}";
                    OnTotalScoreAnimated?.Invoke();
                }));
            
            activeCombineSequence.Append(totalScoreText.DOScale(0, scoreAnimations.scaleDisappearAnimationTime)
                .SetEase(Ease.InBack));

            activeCombineSequence.SetLink(gameObject).OnComplete(() =>
            {
                if (totalScoreText != null)
                {
                    Destroy(totalScoreText.gameObject);
                }

                activeScoreTexts.Remove(totalScoreText);
            });
        }
    }
}
