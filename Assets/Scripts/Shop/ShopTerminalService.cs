using System;
using System.Text;
using Cysharp.Threading.Tasks;
using Input;
using StaticData.Data;
using TerminalCommands;
using UnityEngine;


namespace Shop
{
    public class ShopTerminalService : IDisposable
    {
        private readonly InputService inputService;
        private readonly TerminalCommandsService terminalCommandsService;
        private int cursorPositionIndex;
        private string commandInput;
        private readonly StringBuilder currentStringBuilder = new StringBuilder();
        private bool backspaceHold;
        public event Action<string> OnStringUpdated;
        public event Action<int> OnCursorPositionUpdated;
        public event Action OnCursorDefaultPositionSet;
        public event Action<string> OnCommandExecuted;
        public event Action OnCommandOutputContainerOverfilled;
        public event Action OnCommandOutputCleared;
        private int commandOutputCounter = 1;
        

        public ShopTerminalService(InputService inputService, TerminalCommandsService terminalCommandsService)
        {
            this.inputService = inputService;
            this.terminalCommandsService = terminalCommandsService;
        }


        public void Init()
        {
            inputService.EnableReadingTextInput();
            inputService.OnTextInput += TryTypeChar;
            inputService.OnBackspacePressed += RemoveChar;
            inputService.OnBackSpaceHold += StartBackspaceHold;
            inputService.OnBackspaceReleased += CancelBackspaceHold;
            inputService.OnRightArrowPressed += MoveCursorRight;
            inputService.OnLeftArrowPressed += MoveCursorLeft;
            inputService.OnEnterPressed += SubmitCommand;
        }


        private void TryTypeChar(char charToType)
        {
            currentStringBuilder.Append(charToType);
            commandInput = currentStringBuilder.ToString();
            OnStringUpdated?.Invoke(commandInput);
            MoveCursorRight();
        }


        private void SetDefaultPosition()
        {
            cursorPositionIndex = 0;
            OnCursorDefaultPositionSet?.Invoke();
        }


        private void MoveCursorRight()
        {
            if (cursorPositionIndex < commandInput.Length - 1)
            {
                cursorPositionIndex++;
                OnCursorPositionUpdated?.Invoke(cursorPositionIndex);
            }
        }


        private void SubmitCommand()
        {
            CommandResult commandResult = terminalCommandsService.TryParseCommand(commandInput);
            OnCommandExecuted?.Invoke(commandResult.Output);
            ClearCommandInput();
            commandOutputCounter++;

            if (commandOutputCounter > 10)
            {
                OnCommandOutputContainerOverfilled?.Invoke();
            }
        }


        private void ClearCommandInput()
        {
            currentStringBuilder.Clear();
            commandInput = currentStringBuilder.ToString();
            OnStringUpdated?.Invoke(commandInput);
            SetDefaultPosition();
        }



        public void ClearCommandOutput()
        {
            OnCommandOutputCleared?.Invoke();
        }


        private void MoveCursorLeft()
        {
            if (cursorPositionIndex > 0)
            {
                cursorPositionIndex--;
                OnCursorPositionUpdated?.Invoke(cursorPositionIndex);
            }
        }


        private void RemoveChar()
        {
            if (commandInput.Length > 0)
            {
                currentStringBuilder.Remove(commandInput.Length - 1, 1);
                commandInput = currentStringBuilder.ToString();
                OnStringUpdated?.Invoke(commandInput);
                MoveCursorLeft();
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


        public void Dispose()
        {
            inputService.OnTextInput -= TryTypeChar;
            inputService.OnBackspacePressed -= RemoveChar;
            inputService.OnBackSpaceHold -= StartBackspaceHold;
            inputService.OnBackspaceReleased -= CancelBackspaceHold;
            inputService.OnRightArrowPressed -= MoveCursorRight;
            inputService.OnLeftArrowPressed -= MoveCursorLeft;
            inputService.OnEnterPressed -= SubmitCommand;
        }
    }
}
