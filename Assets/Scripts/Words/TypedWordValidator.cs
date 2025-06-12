using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using Input;
using Typewriter;


namespace Words
{
    public class TypedWordValidator : IDisposable
    {
        private readonly InputService inputService;
        private readonly TargetWordService targetWordService;
        private readonly StringBuilder currentWordBuilder = new StringBuilder();
        public string CurrentWord { get; private set; } = "";
        public List<bool> CurrentWordCharStates { get; private set; } = new List<bool>();
        private bool backspaceHold;
        private const string ALLOWED_SYMBOLS = "!@#$%^&*()-_=+[]{}|;:,'<.>/?";
        private bool isFirstCharTyped;
        public event Action OnWordUpdated;
        public event Action<List<bool>> OnWordValidated;
        public event Action OnFirstCharTyped;
        public event Action OnCharTyped;


        public TypedWordValidator(InputService inputService, TargetWordService targetWordService)
        {
            this.inputService = inputService;
            this.targetWordService = targetWordService;
        }


        public void Init()
        {
            inputService.EnableReadingTextInput();
            inputService.OnTextInput += TryTypeChar;
            inputService.OnBackspacePressed += RemoveChar;
            inputService.OnBackSpaceHold += StartBackspaceHold;
            inputService.OnBackspaceReleased += CancelBackspaceHold;
            OnWordUpdated?.Invoke();
        }


        private void RemoveChar()
        {
            if (CurrentWord.Length > 0)
            {
                currentWordBuilder.Remove(CurrentWord.Length - 1, 1);
                CurrentWord = currentWordBuilder.ToString();
                CurrentWordCharStates.RemoveAt(CurrentWordCharStates.Count - 1);
                OnWordUpdated?.Invoke();
            }
        }


        private void StartBackspaceHold()
        {
            RemoveCharsContinuously().Forget();
        }


        private void CancelBackspaceHold()
        {
            backspaceHold = false;
        }


        private async UniTaskVoid RemoveCharsContinuously()
        {
            backspaceHold = true;

            while (backspaceHold)
            {
                RemoveChar();
                await UniTask.WaitForSeconds(0.1f);
            }
        }


        private bool IsAllowedChar(char character)
        {
            if (char.IsLetterOrDigit(character))
            {
                return true;
            }

            if (ALLOWED_SYMBOLS.Contains(character))
            {
                return true;
            }

            return false;
        }


        private void TryTypeChar(char charToType)
        {
            if (CurrentWord.Length < targetWordService.TargetWord.Length)
            {
                if (IsAllowedChar(charToType))
                {
                    if (!isFirstCharTyped)
                    {
                        isFirstCharTyped = true;
                        OnFirstCharTyped?.Invoke();
                    }
                    currentWordBuilder.Append(charToType);
                    OnCharTyped?.Invoke();
                    CurrentWord = currentWordBuilder.ToString();
                    CompareChars();
                }
            }
        }


        private void CompareChars()
        {
            string targetWord = targetWordService.TargetWord;
            CurrentWordCharStates.Clear();

            for (int i = 0; i < CurrentWord.Length; i++)
            {
                if (CurrentWord[i] == targetWord[i])
                {
                    CurrentWordCharStates.Add(true);
                }
                else
                {
                    CurrentWordCharStates.Add(false);
                }
            }

            OnWordValidated?.Invoke(CurrentWordCharStates);
        }


        public void ClearCurrentWord()
        {
            currentWordBuilder.Clear();
            CurrentWord = currentWordBuilder.ToString();
            OnWordUpdated?.Invoke();
        }


        public void ClearCharStates()
        {
            CurrentWordCharStates.Clear();
        }


        public void Dispose()
        {
            inputService.DisableReadingTextInput();
            inputService.OnTextInput -= TryTypeChar;
            inputService.OnBackspacePressed -= RemoveChar;
            inputService.OnBackSpaceHold -= StartBackspaceHold;
            inputService.OnBackspaceReleased -= CancelBackspaceHold;
        }
    }
}
