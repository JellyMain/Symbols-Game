using System;
using UnityEngine.InputSystem;


namespace Input
{
    public class InputService
    {
        private Keyboard keyboard;
        public event Action<char> OnTextInput;
        private InputActions inputActions;
        public event Action OnBackspacePressed;
        public event Action OnEscapePressed;
        public event Action OnBackSpaceHold;
        public event Action OnBackspaceReleased;
        public event Action OnEnterPressed;
        public event Action OnRightArrowPressed;
        public event Action OnLeftArrowPressed;
        private const string ALLOWED_SYMBOLS = "!@#$%^&*()-_=+[]{}|;:,'<.>/?";


        public InputService()
        {
            inputActions = new InputActions();
            inputActions.Typewriter.Enable();
            EnableReadingTextInput();
            inputActions.Typewriter.EscapePressed.performed += EscapePressed;
            inputActions.Typewriter.BackspacePressed.performed += BackSpacePressed;
            inputActions.Typewriter.BackspaceHold.performed += BackSpaceHold;
            inputActions.Typewriter.BackspaceHold.canceled += BackSpaceReleased;
            inputActions.Typewriter.EnterPressed.performed += EnterPressed;
            inputActions.Typewriter.RightArrowPressed.performed += RightArrowPressed;
            inputActions.Typewriter.LeftArrowPressed.performed += LeftArrowPressed;
        }


        public void EnableReadingTextInput()
        {
            keyboard = Keyboard.current;
            keyboard.onTextInput += TextInput;
        }



        private void EscapePressed(InputAction.CallbackContext callbackContext)
        {
            OnEscapePressed?.Invoke();
        }


        private void BackSpacePressed(InputAction.CallbackContext callbackContext)
        {
            OnBackspacePressed?.Invoke();
        }


        private void BackSpaceHold(InputAction.CallbackContext callbackContext)
        {
            OnBackSpaceHold?.Invoke();
        }


        private void BackSpaceReleased(InputAction.CallbackContext callbackContext)
        {
            OnBackspaceReleased?.Invoke();
        }


        private void EnterPressed(InputAction.CallbackContext callbackContext)
        {
            OnEnterPressed?.Invoke();
        }


        private void RightArrowPressed(InputAction.CallbackContext callbackContext)
        {
            OnRightArrowPressed?.Invoke();
        }


        private void LeftArrowPressed(InputAction.CallbackContext callbackContext)
        {
            OnLeftArrowPressed?.Invoke();
        }


        private void TextInput(char character)
        {
            if (IsAllowedChar(character))
            {
                OnTextInput?.Invoke(character);
            }
        }


        public void DisableReadingTextInput()
        {
            keyboard.onTextInput -= TextInput;
        }


        private bool IsAllowedChar(char character)
        {
            if (char.IsLetterOrDigit(character))
            {
                return true;
            }

            if (character == ' ')
            {
                return true;
            }

            if (ALLOWED_SYMBOLS.Contains(character))
            {
                return true;
            }

            return false;
        }
    }
}
