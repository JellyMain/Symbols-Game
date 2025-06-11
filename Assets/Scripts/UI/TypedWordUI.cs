using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using Febucci.UI;
using TMPro;
using Typewriter;
using UnityEngine;
using Words;
using Zenject;


namespace UI
{
    public class TypedWordUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text underscoresText;
        [SerializeField] private TMP_Text currentWordText;
        [SerializeField] private TMP_Text targetWordText;
        private TargetWordService targetWordService;
        private TypedWordValidator typedWordValidator;
        private DOTweenTMPAnimator textAnimator;
        public TMP_Text CurrentWordText => currentWordText;


        [Inject]
        private void Construct(TypedWordValidator typedWordValidator, TargetWordService targetWordService)
        {
            this.typedWordValidator = typedWordValidator;
            this.targetWordService = targetWordService;
        }


        private void Start()
        {
            textAnimator = new DOTweenTMPAnimator(currentWordText);
            typedWordValidator.OnWordUpdated += UpdateTypedWordUI;
            typedWordValidator.OnWordValidated += HandleWordValidation;
            targetWordService.OnTargetWordUpdated += SetTargetWordUI;
        }


        private void OnDisable()
        {
            typedWordValidator.OnWordUpdated -= UpdateTypedWordUI;
            typedWordValidator.OnWordValidated -= HandleWordValidation;
            targetWordService.OnTargetWordUpdated -= SetTargetWordUI;
        }


        private void SetTargetWordUI(string word)
        {
            targetWordText.text = word;
            SetUnderscoresUI(word);
        }


        private void SetUnderscoresUI(string word)
        {
            underscoresText.text = new string('_', word.Length);
        }
        
        
        private void UpdateTypedWordUI()
        {
            string currentWord = typedWordValidator.CurrentWord;
            List<bool> charStates = typedWordValidator.CurrentWordCharStates;

            string formatedText = BuildFormattedWord(currentWord, charStates);
            
            currentWordText.text = formatedText;
            currentWordText.ForceMeshUpdate();
        }
        

        private void HandleWordValidation(List<bool> charStates)
        {
            string currentWord = typedWordValidator.CurrentWord;

            if (string.IsNullOrEmpty(currentWord))
                return;

            string formattedText = BuildFormattedWord(currentWord, charStates);
            currentWordText.text = formattedText;
            currentWordText.ForceMeshUpdate();

            AnimateLastTypedCharacter(currentWord.Length);
        }

        
        private string BuildFormattedWord(string word, List<bool> charStates)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < word.Length; i++)
            {
                if (i < charStates.Count && !charStates[i])
                {
                    sb.Append(AddColorTag(word[i], "red"));
                }
                else
                {
                    sb.Append(word[i]);
                }
            }

            return sb.ToString();
        }

        
        private void AnimateLastTypedCharacter(int length)
        {
            if (length == 0) return;

            textAnimator.DOFadeChar(length - 1, 1, 0.2f).From(0);
        }

        
        private string AddColorTag(char c, string colorName)
        {
            return $"<color={colorName}>{c}</color>";
        }
    }
}
