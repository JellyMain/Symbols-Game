using System;
using System.Collections.Generic;
using DG.Tweening;
using Score;
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
        private ScoreService scoreService;
        public event Action OnTotalScoreAnimated;


        [Inject]
        private void Construct(ScoreService scoreService)
        {
            this.scoreService = scoreService;
        }


        private void Start()
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
                .OnComplete(() => OnTotalScoreAnimated?.Invoke()));
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
    }
}
